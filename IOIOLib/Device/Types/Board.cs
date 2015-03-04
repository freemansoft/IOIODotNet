using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Types
{
    internal class Board
    {
        static Board SPRK0015 = new Board(Hardware.IOIO0002);
        static Board SPRK0016 = new Board(Hardware.IOIO0003);
        static Board MINT0010 = new Board(Hardware.IOIO0003);
        static Board SPRK0020 = new Board(Hardware.IOIO0004);

        internal static Dictionary<string, Hardware> AllBoards = new Dictionary<string, Hardware>()
        {
            {"SPRK0015", Hardware.IOIO0002},
            {"SPRK0016", Hardware.IOIO0003},
            {"MINT0010", Hardware.IOIO0003},
            {"SPRK0020", Hardware.IOIO0004}
        };


        private Hardware hardware;

        internal Board(Hardware hardware)
        {
            this.hardware = hardware;
        }

    }
}
