using System;
using System.Collections.Generic;
using System.Text;

namespace HiSign.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
