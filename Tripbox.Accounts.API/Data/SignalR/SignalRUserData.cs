using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tripbox.Accounts.API.Models;
using Tripbox.Accounts.API.Models.SignalR;

namespace Tripbox.Accounts.API.Data.SignalR
{
    public  class SignalRUserData : ISignalRUserData
    {
        private DataContext _dataContext;

        public SignalRUserData(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public SignalRUser AddSignalRUser(SignalRUser signalRUser)
        {
            _dataContext.SignalRUsers.Add(signalRUser);
            _dataContext.SaveChanges();
            return signalRUser;
        }

        public void DeleteSignalRUser(SignalRUser signalRUser)
        {
            _dataContext.SignalRUsers.Remove(signalRUser);
            _dataContext.SaveChanges();          
        }

        public List<SignalRUser> getSignalRUsers()
        {
            return _dataContext.SignalRUsers.ToList();
        }
    }
}
