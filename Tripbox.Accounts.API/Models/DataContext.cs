using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tripbox.Accounts.API.Models.SignalR;

namespace Tripbox.Accounts.API.Models;

    public class DataContext: DbContext
    {

        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //public DataContext(DbContextOptions<DataContext> options)
        //    : base(options)
        //{ 
        //}

        public DbSet<SignalRUser> SignalRUsers { get; set; }
    }

