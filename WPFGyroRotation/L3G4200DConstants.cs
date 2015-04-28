using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGyroRotation
{
    internal class L3G4200DConstants
    {
        // slave address is 7 bits, the last bit is set by SD0 line
        internal static int GyroSlaveAddress0 = Convert.ToInt32("1101000", 2);
        // the parallax board seems to pull SD0 high so this is the address we need
        internal static int GyroSlaveAddress1 = Convert.ToInt32("1101001", 2);

        internal static byte Gyro_WhoAmI_Register = 0x0f;
        internal static byte Gyro_WhoAmI_ID_L3G4200D = 0xD3;
        internal static byte Gyro_CTRL_REG1 = 0x20;
        internal static byte Gyro_CTRL_REG2 = 0x21;
        internal static byte Gyro_CTRL_REG3 = 0x22;
        internal static byte Gyro_CTRL_REG4 = 0x23;
        internal static byte Gyro_CTRL_REG5 = 0x24;
        internal static byte Gyro_First_Out_Register = 0x28;

        internal static byte Gyro_Range_DPS_250 = (byte)(0x00 << 4);   // yeah I know shift isn't necessary for 0x00
        internal static byte Gyro_Range_DPS_500 = (byte)(0x01 << 4);
        internal static byte Gyro_Range_DPS_2000 = (byte)(0x02 << 4);
        // sensitivity is higher the lower the measurement range 
        internal static float Gyro_DPS_LSB_250 =  8.75f/1000.0f; // 8.75 mdps/LSB
        internal static float Gyro_DPS_LSB_500 =  17.5f/1000.0f; // 17.50 mdps/LSB
        internal static float Gyro_DPS_LSB_2000 = 70.0f/1000.0f; // 70.0 mdps/LSB

    }
}
