using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDW.API.Hubs
{
    public class ActivityHub :  Hub<IActivityClient>
    {
        public Task ChangeActivity()
        {
            return Clients.All.ActivityChanged();
        }
    }
}
