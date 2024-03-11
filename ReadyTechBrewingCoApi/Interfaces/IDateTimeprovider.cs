using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadyTechBrewingCoApi.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now {get;}
    }
}