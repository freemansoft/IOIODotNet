using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component.Types
{
    /** SPI configuration structure. */
    public class SpiMasterConfig
    {
        /** Data rate. */
        public SpiMasterRate rate {  set;  get; }
        /** Whether to invert clock polarity. */
        public bool invertClk {  set;  get; }
        /**
         * Whether to do the input and output sampling on the trailing clock
         * edge.
         */
        public bool sampleOnTrailing;

        /**
         * Constructor.
         * 
         * @param rate
         *            Data rate.
         * @param invertClk
         *            Whether to invert clock polarity.
         * @param sampleOnTrailing
         *            Whether to do the input and output sampling on the
         *            trailing clock edge.
         */
        public SpiMasterConfig(SpiMasterRate rate, bool invertClk, bool sampleOnTrailing)
        {
            this.rate = rate;
            this.invertClk = invertClk;
            this.sampleOnTrailing = sampleOnTrailing;
        }

        /**
         * Constructor with common defaults. Equivalent to Config(rate, false,
         * false)
         * 
         * @see SpiMaster.Config#Config(SpiMaster.Config.Rate, boolean, boolean)
         */
        public SpiMasterConfig(SpiMasterRate rate)
            : this(rate, false, false)
        {
        }
    }
}
