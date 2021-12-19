using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDW.API.Hubs
{
    public interface IActivityClient
    {
        public Task ActivityChanged();
    }
}
