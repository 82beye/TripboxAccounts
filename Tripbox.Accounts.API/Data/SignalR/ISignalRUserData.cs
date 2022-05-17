using System.Collections.Generic;
using Tripbox.Accounts.API.Models.SignalR;

namespace Tripbox.Accounts.API.Data.SignalR
{
    interface ISignalRUserData
    {

        List<SignalRUser> getSignalRUsers();

        SignalRUser AddSignalRUser(SignalRUser signalRUser);

        void DeleteSignalRUser(SignalRUser signalRUser);

    }
}
