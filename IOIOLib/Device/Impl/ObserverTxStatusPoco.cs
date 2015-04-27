using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Impl
{
    /// <summary>
    /// IEquatable<> interface used by the concurrent map to verify old value against new
    /// </summary>
    public class ObserverTxStatusPoco : IEquatable<ObserverTxStatusPoco>
    {
        public int NumBytesRemaining { get; private set; } = 0;

        /// <summary>
        /// used for testing and send / receive matching
        /// </summary>
        public int NumSent { get; private set; } = 0;
        /// <summary>
        /// used for testing and send / receive matching
        /// </summary>
        public int NumReceived { get; private set; } = 0;

        public ObserverTxStatusPoco(int numBytesRemaining, int numSent, int numReceived)
        {
            this.NumBytesRemaining = numBytesRemaining;
            this.NumSent = numSent;
            this.NumReceived = numReceived;
        }

        public ObserverTxStatusPoco() : this(0,0,0)
        {
            //move along. nothing is happening here
        }

        public override string ToString()
        {
            return this.GetType().Name + " Space:" + NumBytesRemaining+ " Sent:" + NumSent + " Recv:" + NumReceived;
        }


        public bool Equals(ObserverTxStatusPoco other)
        {
            if (this.NumBytesRemaining == other.NumBytesRemaining
                && this.NumReceived == other.NumReceived
                && this.NumSent == other.NumSent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
