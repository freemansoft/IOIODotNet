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
using IOIOLib.Convenience;
using IOIOLib.Connection;
using IOIOLib.Connection.Impl;
using IOIOLib.Device;
using IOIOLib.Device.Impl;
using IOIOLib.MessageFrom;
using IOIOLib.Component.Types;
using IOIOLib.MessageTo;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int SERVO_PIN = 3;
        private IOIO OurImpl_;
        private PwmOutputSpec ServoPinDef_;

        public MainWindow()
        {
            InitializeComponent();
            string comPort = FindDeviceHack.TryAndFindIOIODevice();
            ComPort_Field.Text = comPort;
            if (comPort != null) { 
                IOIOConnection connection = new SerialConnectionFactory().CreateConnection(comPort);                
                IOIOHandlerCaptureConnectionState handler = new IOIOHandlerCaptureConnectionState();
                OurImpl_  = new IOIOImpl(connection, handler);
                OurImpl_.WaitForConnect();

                IConnectedDeviceResponse device = handler.ConnectedDeviceDescription();
                if (device != null) {
                    BoardDetails.Text = "Bootloader:" + device.BootloaderId
                        + "\n" + "Firmware:" + device.FirmwareId
                        + "\n" + "Hardware: " + device.HardwareId;
                }
                DigitalOutputSpec pwmPinSpec = new DigitalOutputSpec(SERVO_PIN, DigitalOutputSpecMode.NORMAL);
                IOIOMessageCommandFactory commandFactory = new IOIOMessageCommandFactory();
                IPwmOutputConfigureCommand createPwm = commandFactory.CreateConfigurePwmOutput(pwmPinSpec, 100);
                OurImpl_.PostMessage(createPwm);
                // message post fills in pin def so pick that up.  runs in a thread so wait until command completes
                // have to capture the PwmDef to get the frequencey
                while (createPwm.PwmDef.Frequency < 0)
                {
                    System.Threading.Thread.Sleep(10);
                }
                this.ServoPinDef_ = createPwm.PwmDef;

                // value should match minimum of the slider
                IPwmOutputUpdatePulseWidthCommand command =
                    new IOIOMessageCommandFactory().CreateUpdatePwmPulseWithOutput(this.ServoPinDef_, 600.0f);
                OurImpl_.PostMessage(command);
            } else
            {
                //this.Close();
                BoardDetails.Text = "Unable to find an IOIO device";
            }

        }

        private void ServoPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // only run commands once we have programmed for PWM
            if (this.ServoPinDef_ != null) { 
                IPwmOutputUpdateCommand command=
                    new IOIOMessageCommandFactory().CreateUpdatePwmPulseWithOutput(this.ServoPinDef_, (float)e.NewValue);
                OurImpl_.PostMessage(command);
            }
        }
    }
}
