using IOIOLib.Component.Types;
using IOIOLib.Connection;
using IOIOLib.Connection.Impl;
using IOIOLib.Convenience;
using IOIOLib.Device;
using IOIOLib.Device.Impl;
using IOIOLib.Message;
using IOIOLib.MessageFrom;
using IOIOLib.MessageTo;
using IOIOLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFGyroRotation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(MainWindow));

        private IOIO OurImpl_;
        private IOIOMessageCommandFactory CommandFactory_;
        // generated during setup
        private TwiSpec TwiDef_;

        private int PollingIntervalMsec = 20;

        public MainWindow()
        {
            InitializeComponent();
            string comPort = FindDeviceHack.TryAndFindIOIODevice();
            ComPort_Field.Text = comPort;
            if (comPort != null)
            {
                IOIOConnection connection = new SerialConnectionFactory().CreateConnection(comPort);

                ObserverConnectionState handlerCaptureState = new ObserverConnectionState();
                RotationObserver rotationObserver = new RotationObserver(
                    L3G4200DConstants.Gyro_DPS_LSB_2000,        // must match the sensitivity in register initialization 
                    PollingIntervalMsec,
                    this.X_Angle, this.Y_Angle, this.Z_Angle,
                    this.X_RawField, this.Y_RawField, this.Z_RawField,
                    this.X_CallibField, this.Y_CallibField, this.Z_CallibField,
                    this.Teapot
                    );

                OurImpl_ = new IOIOImpl(connection, new List<IObserverIOIO>()
                    { handlerCaptureState, rotationObserver}
                    );
                OurImpl_.WaitForConnect();

                IConnectedDeviceResponse device = handlerCaptureState.ConnectedDeviceDescription();
                if (device != null)
                {
                    // could display board details
                }
                CommandFactory_ = new IOIOMessageCommandFactory();
                ConfigureCompass();
                ConfigureTimedEvents();
            }
            else
            {
                ComPort_Field.Text = "Unable to find an IOIO device";
            }

        }

        private void ConfigureTimedEvents()
        {
            DispatcherTimer dispatcherTimer;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(RequestReading);
            dispatcherTimer.Interval = new TimeSpan(0,0, 0, 0, PollingIntervalMsec);
            dispatcherTimer.Start();
        }


        /// <summary>
        /// ripped form unit tests
        /// </summary>
        /// <param name="ourImpl_"></param>
        private void ConfigureCompass()
        {


            ITwiMasterConfigureCommand startCommand = CommandFactory_.CreateTwiConfigure(0, TwiMasterRate.RATE_400KHz, false);
            OurImpl_.PostMessage(startCommand);
            System.Threading.Thread.Sleep(50);
                TwiDef_ = startCommand.TwiDef;

            LOG.Debug("Ask for Who Am I");
            // send the whoami command - we expect the id to be Gyro_WhoAmI_ID
            byte[] ReadWhoAmiRegisterData = new byte[] { L3G4200DConstants.Gyro_WhoAmI_Register };
            ITwiMasterSendDataCommand startupCommand1 = CommandFactory_.CreateTwiSendData(TwiDef_, L3G4200DConstants.GyroSlaveAddress1, false, ReadWhoAmiRegisterData, 1);
            OurImpl_.PostMessage(startupCommand1);
            System.Threading.Thread.Sleep(50);
            // should check for Gyro_WhoAmI_ID_L3G4200D !

            // Enable x, y, z and turn off power down
            // auto increment registers
            byte ControlRegisterAutoIncrement = L3G4200DConstants.Gyro_CTRL_REG1 |= Convert.ToByte(0x80);
            byte[] RegisterConfigurationData = new byte[] { ControlRegisterAutoIncrement,
                Convert.ToByte("00001111", 2),
                Convert.ToByte("00000000", 2),
                Convert.ToByte("00000000", 2),
                (byte) (0x80 | L3G4200DConstants.Gyro_Range_DPS_2000), //Dont override values
                //Enable High pass filter
                Convert.ToByte("00000000", 2)
            };
            LOG.Debug("Updating Registers starting with " + L3G4200DConstants.Gyro_CTRL_REG1.ToString("X") + ":" + RegisterConfigurationData);
            ITwiMasterSendDataCommand ConfigureRegisters = CommandFactory_.CreateTwiSendData(TwiDef_, L3G4200DConstants.GyroSlaveAddress1, false, RegisterConfigurationData, 0);
            OurImpl_.PostMessage(ConfigureRegisters);


        }

        public void RequestReading(object sender, EventArgs e)
        {
            LOG.Debug("Send read-only command retreive xyz with auto increment sendCount: "+sender);
            byte[] ReadFromFirstOutRegisterWithAutoInc = new byte[] { L3G4200DConstants.Gyro_First_Out_Register |= Convert.ToByte(0x80) };
            ITwiMasterSendDataCommand ReadXYZ = CommandFactory_.CreateTwiSendData(this.TwiDef_, L3G4200DConstants.GyroSlaveAddress1, false, ReadFromFirstOutRegisterWithAutoInc, 6);
            OurImpl_.PostMessage(ReadXYZ);

        }

    }
}
