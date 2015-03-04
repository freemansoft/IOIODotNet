using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Connection
{
    public interface IOIOConnection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>connection string used to create this connection</returns>
        string ConnectionString();

        void waitForConnect();
        void disconnect();

        Stream getInputStream();

        Stream getOutputStream();

        bool canClose();


    }
}
