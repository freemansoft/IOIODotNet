using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Util
{
    public class LoggingUtilities
    {
        /// <summary>
        // not the fastest but sort of fast and simple
        // http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] ba, int size)
        {
            if (ba == null)
            {
                return "";
            }
            else
            {
                string hex = BitConverter.ToString(ba, 0, size);
                return hex.Replace("-", " ");
            }
        }


    }
}
