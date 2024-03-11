using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadyTechBrewingCoApi.Interfaces;

namespace ReadyTechBrewingCoApi.Wrappers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}