using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.MessageFrom
{
    public interface IReportAnalogPinValuesFrom : IMessageFromIOIO
    {
         int Pin { get; }
         int Value { get; }
    }
}
