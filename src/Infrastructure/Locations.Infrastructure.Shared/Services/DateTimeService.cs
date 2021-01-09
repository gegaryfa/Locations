using System;
using Locations.Core.Application.Interfaces;

namespace Locations.Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
