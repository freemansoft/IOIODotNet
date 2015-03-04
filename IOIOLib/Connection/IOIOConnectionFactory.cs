using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Connection
{
    public interface IOIOConnectionFactory
    {
        IOIOConnection createConnection(string connectionString);

        /// <summary>
        /// Try to create connections to the speicfied connections
        /// </summary>
        /// <param name="connectionStrings"></param>
        /// <returns></returns>
        ICollection<IOIOConnection> createConnections(ICollection<string> connectionStrings);

        /// <summary>
        /// Attempts to identify available connections and returns them
        /// </summary>
        /// <returns></returns>
        ICollection<IOIOConnection> createConnections();
    }
}
