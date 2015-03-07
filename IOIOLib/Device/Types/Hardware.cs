using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device.Types
{
    public class Hardware
    {
        internal static bool[,] MAP_IOIO0002_IOIO0003 = {
            // p. out  p. in  analog
            { true,    true,  false },  // 0
            { false,  false,  false },  // 1
            { false,  false,  false },  // 2
            { true,    true,  false },  // 3
            { true,    true,  false },  // 4
            { true,    true,  false },  // 5
            { true,    true,  false },  // 6
            { true,    true,  false },  // 7
            { false,  false,  false },  // 8
            { false,  true,  false },  // 9
            { true,    true,  false },  // 10
            { true,    true,  false },  // 11
            { true,    true,  false },  // 12
            { true,    true,  false },  // 13
            { true,    true,  false },  // 14
            { false,  false,  false },  // 15
            { false,  false,  false },  // 16
            { false,  false,  false },  // 17
            { false,  false,  false },  // 18
            { false,  false,  false },  // 19
            { false,  false,  false },  // 20
            { false,  false,  false },  // 21
            { false,  false,  false },  // 22
            { false,  false,  false },  // 23
            { false,  false,  false },  // 24
            { false,  false,  false },  // 25
            { false,  false,  false },  // 26
            { true,    true,  false },  // 27
            { true,    true,  false },  // 28
            { true,    true,  false },  // 29
            { true,    true,  false },  // 30
            { true,    true,  true  },  // 31
            { true,    true,  true  },  // 32
            { false,  false,  true  },  // 33
            { true,    true,  true  },  // 34
            { true,    true,  true  },  // 35
            { true,    true,  true  },  // 36
            { true,    true,  true  },  // 37
            { true,    true,  true  },  // 38
            { true,    true,  true  },  // 39
            { true,    true,  true  },  // 40
            { false,  false,  true  },  // 41
            { false,  false,  true  },  // 42
            { false,  false,  true  },  // 43
            { false,  false,  true  },  // 44
            { true,    true,  true  },  // 45
            { true,    true,  true  },  // 46
            { true,    true,  false },  // 47
            { true,    true,  false }   // 48
        };
        internal static bool[,] MAP_IOIO0004 = {
            // p. out  p. in  analog
            { false,  false,  false },  // 0
            { true,    true,  false },  // 1
            { true,    true,  false },  // 2
            { true,    true,  false },  // 3
            { true,    true,  false },  // 4
            { true,    true,  false },  // 5
            { true,    true,  false },  // 6
            { true,    true,  false },  // 7
            { false,  false,  false },  // 8
            { false,  true,  false },  // 9
            { true,    true,  false },  // 10
            { true,    true,  false },  // 11
            { true,    true,  false },  // 12
            { true,    true,  false },  // 13
            { true,    true,  false },  // 14
            { false,  false,  false },  // 15
            { false,  false,  false },  // 16
            { false,  false,  false },  // 17
            { false,  false,  false },  // 18
            { false,  false,  false },  // 19
            { false,  false,  false },  // 20
            { false,  false,  false },  // 21
            { false,  false,  false },  // 22
            { false,  false,  false },  // 23
            { false,  false,  false },  // 24
            { false,  false,  false },  // 25
            { false,  false,  false },  // 26
            { true,    true,  false },  // 27
            { true,    true,  false },  // 28
            { true,    true,  false },  // 29
            { true,    true,  false },  // 30
            { true,    true,  true  },  // 31
            { true,    true,  true  },  // 32
            { false,  false,  true  },  // 33
            { true,    true,  true  },  // 34
            { true,    true,  true  },  // 35
            { true,    true,  true  },  // 36
            { true,    true,  true  },  // 37
            { true,    true,  true  },  // 38
            { true,    true,  true  },  // 39
            { true,    true,  true  },  // 40
            { false,  false,  true  },  // 41
            { false,  false,  true  },  // 42
            { false,  false,  true  },  // 43
            { false,  false,  true  },  // 44
            { true,    true,  true  },  // 45
            { true,    true,  true  }   // 46
        };

        internal static Hardware IOIO0002 = new Hardware(MAP_IOIO0002_IOIO0003,
                9, 4, 3, new int[] { 0, 2, 4 }, new int[] { 6, 7, 8 },
                new int[,] { { 4, 5 }, { 47, 48 }, { 26, 25 } },
                new int[] { 36, 37, 38 });
        internal static Hardware IOIO0003 = IOIO0002;
        internal static Hardware IOIO0004 = new Hardware(MAP_IOIO0004,
                9, 4, 3, new int[] { 0, 2, 4 }, new int[] { 6, 7, 8 },
                new int[,] { { 4, 5 }, { 1, 2 }, { 26, 25 } },
                new int[] { 36, 37, 38 });

        internal bool[,] PinMap { get; private set; }
        internal int NumPwmModules { get; private set; }
        internal int NumUartModules { get; private set; }
        internal int NumSpiModules { get; private set; }
        internal int[] IncapDoubleModules { get; private set; }
        internal int[] IncapSingleModules { get; private set; }
        internal int[,] TwiPins { get; private set; }
        internal int[] IcspPins { get; private set; }

        public Hardware(bool[,] map, int numPwmModules, int numUartModules, int numSpiModules, int[] incapDoubleModules, int[] incapSingleModules, int[,] twiPins, int[] icspPins)
        {
            // TODO: Complete member initialization
            this.PinMap = map;
            this.NumPwmModules = numPwmModules;
            this.NumUartModules = numUartModules;
            this.NumSpiModules = numSpiModules;
            this.IncapDoubleModules = incapDoubleModules;
            this.IncapSingleModules = incapSingleModules;
            this.TwiPins = twiPins;
            this.IcspPins = icspPins;
        }

        int numPins()
        {
            return PinMap.GetLength(0);
        }

        int numAnalogPins()
        {
            int result = 0;
            for (int index = 0; index < PinMap.GetLength(0); index++)
            {
                if (PinMap[index, (int)Function.ANALOG_IN])
                {
                    ++result;
                }
            }
            return result;
        }

        int numPwmModules()
        {
            return NumPwmModules;
        }

        int numUartModules()
        {
            return NumUartModules;
        }

        int numSpiModules()
        {
            return NumSpiModules;
        }

        int numTwiModules()
        {
            return TwiPins.Length;
        }

        int[] incapSingleModules()
        {
            return IncapSingleModules;
        }

        int[] incapDoubleModules()
        {
            return IncapDoubleModules;
        }

        int[,] twiPins()
        {
            return TwiPins;
        }

        int[] icspPins()
        {
            return IcspPins;
        }

        void checkSupportsAnalogInput(int pin)
        {
            checkValidPin(pin);
            if (!PinMap[pin, (int)Function.ANALOG_IN])
            {
                throw new ArgumentException("Pin " + pin
                        + " does not support analog input");
            }
        }

        void checkSupportsPeripheralInput(int pin)
        {
            checkValidPin(pin);
            if (!PinMap[pin, (int)Function.PERIPHERAL_IN])
            {
                throw new ArgumentException("Pin " + pin
                        + " does not support peripheral input");
            }
        }

        void checkSupportsPeripheralOutput(int pin)
        {
            checkValidPin(pin);
            if (!PinMap[pin, (int)Function.PERIPHERAL_OUT])
            {
                throw new ArgumentException("Pin " + pin
                        + " does not support peripheral output");
            }
        }

        void checkValidPin(int pin)
        {
            if (pin < 0 || pin >= PinMap.GetLength(0))
            {
                throw new ArgumentException("Illegal pin: " + pin);
            }
        }

        void checkSupportsCapSense(int pin)
        {
            checkValidPin(pin);
            // Currently, all analog pins are also cap-sense.
            if (!PinMap[pin, (int)Function.ANALOG_IN])
            {
                throw new ArgumentException("Pin " + pin
                        + " does not support cap-sense");
            }
        }

    }
}
