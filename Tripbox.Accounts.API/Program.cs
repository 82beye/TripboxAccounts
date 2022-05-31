using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Tripbox.Accounts.API.Helper;
using Tripbox.Accounts.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.OAuth;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container.
//builder.Services.AddControllersWithViews();

services.AddRouting();

services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.ReportApiVersions = true;
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ApiVersionReader = new HeaderApiVersionReader("X-Version");
    SwaggerConfig.UseCustomHeaderApiVersion("X-Version");
});

services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
}));

services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

services.AddMvc(option => option.EnableEndpointRouting = false);

services.AddMvc().AddRazorPagesOptions(o =>
{
    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});


services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tripbox.Accounts.API", Version = "v1" });
    c.OperationFilter<SwaggerParameterFilters>();
    c.DocumentFilter<SwaggerVersionMapping>();
    c.DocInclusionPredicate((version, desc) =>
    {
        if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
        var versions = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions);
        var maps = methodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions).ToArray();
        version = version.Replace("v", "");
        return versions.Any(v => v.ToString() == version && maps.Any(v => v.ToString() == version));
    });

    var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
    c.IncludeXmlComments(filePath);

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

    c.AddSecurityRequirement(securityRequirement);
});

services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/signin";
    options.LogoutPath = "/signout";
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["OAuth:Google:ClientId"];
    options.ClientSecret = builder.Configuration["OAuth:Google:ClientSecret"];
    //options.CallbackPath = "/callback/google";
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.SaveTokens = true;
    options.Events = new OAuthEvents()
    {
        OnTicketReceived = context =>
        {
            // I have fixed my claim here 
            return Task.CompletedTask;
        }
        //},
        //OnRemoteFailure = context =>
        //{
        //    context.Response.Redirect("/");
        //    context.HandleResponse();
        //    return System.Threading.Tasks.Task.CompletedTask;
        //}
    };
})
.AddNaver(options =>
{
    options.ClientId = builder.Configuration["OAuth:Naver:ClientId"];
    options.ClientSecret = builder.Configuration["OAuth:Naver:ClientSecret"];
    //options.CallbackPath = "/callback/naver";
    options.SaveTokens = true;
    options.Events = new OAuthEvents()
    {
        OnTicketReceived = context =>
        {
            // I have fixed my claim here 
            return Task.CompletedTask;
        }
            //},
            //OnRemoteFailure = context =>
            //{
            //    context.Response.Redirect("/");
            //    context.HandleResponse();
            //    return System.Threading.Tasks.Task.CompletedTask;
            //}
        };
})
.AddKakaoTalk(options =>
{
    options.ClientId = builder.Configuration["OAuth:Kakaotalk:ClientId"];
    options.ClientSecret = builder.Configuration["OAuth:Kakaotalk:ClientSecret"];
    //options.CallbackPath = "/callback/kakaotalk";
    options.SaveTokens = true;
    options.Events = new OAuthEvents()
    {
        OnTicketReceived = context =>
        {
            // I have fixed my claim here 
            return Task.CompletedTask;
        }
        //},
        //OnRemoteFailure = context =>
        //{
        //    context.Response.Redirect("/");
        //    context.HandleResponse();
        //    return System.Threading.Tasks.Task.CompletedTask;
        //}
    };
});



services.AddMvc();


var app = builder.Build();
IConfiguration configuration = app.Configuration;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
     
}

var options = new StaticFileOptions()
{
    ServeUnknownFileTypes = true,
};

//app.Use((context, next) =>
//{
//    var chkContext = context;
//    var chkNext = next;

//    context.Request.Host = new HostString("tripboxaccounts.azurewebsites.net");
//    context.Request.PathBase = new PathString("/callback"); //if you need this
//    context.Request.Scheme = "https";
//    return next();
//});

app.UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json");

app.UseSwaggerUI(ui =>
{
    //ui.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Tripbox API Endpoint");
    //
    ui.DocumentTitle = "TripBox Accounts API";
    ui.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
    //ui.RoutePrefix = "/swagger";
});

app.Use((context, next) =>
{
    var controller = context.Request.RouteValues["controller"];

    return next(context);
});


app.UseStaticFiles(options);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseCors("CorsPolicy");

app.UseMiddleware<APILoggersMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});

app.MapControllerRoute(
    name: "CallBack",
    pattern: "{controller=Auth}/{action=Callback}/{provider?}");


app.Run();
