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
using IOIOLib.Device.Types;

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
        private DigitalOutputSpec LedPinSpec_;

        public MainWindow()
        {
            InitializeComponent();
            string comPort = FindDeviceHack.TryAndFindIOIODevice();
            ComPort_Field.Text = comPort;
            if (comPort != null) { 
                IOIOConnection connection = new SerialConnectionFactory().CreateConnection(comPort);
                ObserverConnectionState handlerCaptureState = new ObserverConnectionState();
                IOIOHandlerObservable handlerNotifier = new IOIOHandlerObservable();
                handlerNotifier.Subscribe(handlerCaptureState);
                handlerNotifier.Subscribe(new MessageObserver(this.MessageLog));
                OurImpl_  = new IOIOImpl(connection, handlerNotifier);
                OurImpl_.WaitForConnect();

                IConnectedDeviceResponse device = handlerCaptureState.ConnectedDeviceDescription();
                if (device != null) {
                    BoardDetails.Text = "Bootloader:" + device.BootloaderId
                        + "\n" + "Firmware:" + device.FirmwareId
                        + "\n" + "Hardware: " + device.HardwareId;
                }
                IOIOMessageCommandFactory commandFactory = new IOIOMessageCommandFactory();
                ConfigurePwm(commandFactory);
                ConfigureLed(commandFactory);
                ConfigureDigitalInput(commandFactory);
            }
            else
            {
                //this.Close();
                BoardDetails.Text = "Unable to find an IOIO device";
            }

        }

        private void ConfigurePwm(IOIOMessageCommandFactory commandFactory)
        {
            DigitalOutputSpec pwmPinSpec = new DigitalOutputSpec(SERVO_PIN, DigitalOutputSpecMode.NORMAL);
            IPwmOutputConfigureCommand createPwm = commandFactory.CreatePwmOutputConfigure(pwmPinSpec, 100);
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
                new IOIOMessageCommandFactory().CreatePwmPulseWithOutputUpdate(this.ServoPinDef_, 600.0f);
            OurImpl_.PostMessage(command);
        }

        private void ConfigureLed(IOIOMessageCommandFactory commandFactory)
        {
            LedPinSpec_ = new DigitalOutputSpec(Spec.LED_PIN);
            IDigitalOutputConfigureCommand createLED = commandFactory.CreateDigitalOutputConfigure(LedPinSpec_,
                LEDValueForState(false));
            OurImpl_.PostMessage(createLED);
        }

        private void ConfigureDigitalInput(IOIOMessageCommandFactory commandFactory)
        {
            DigitalInputSpec InSpec_ = new DigitalInputSpec(2,DigitalInputSpecMode.PULL_UP);
            IDigitalInputConfigureCommand createDigitalInput = commandFactory.CreateDigitalInputConfigure(InSpec_, true);
            OurImpl_.PostMessage(createDigitalInput);
        }

        private bool LEDValueForState(bool state)
        {
            return !state;
        }

        private void ServoPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // only run commands once we have programmed for PWM
            if (this.ServoPinDef_ != null) { 
                IPwmOutputUpdateCommand command=
                    new IOIOMessageCommandFactory().CreatePwmPulseWithOutputUpdate(this.ServoPinDef_, (float)e.NewValue);
                OurImpl_.PostMessage(command);
                //MessageBox.Show(string.Format("Servo Slider: {0}", e.NewValue));
            }
        }

        private void LEDState_Click(object sender, RoutedEventArgs e)
        {
            // only run commands once we have programmed the system
            if (this.LedPinSpec_ != null)
            {
                IDigitalOutputValueSetCommand command =
                    new IOIOMessageCommandFactory().CreateDigitalOutputCommandSet(this.LedPinSpec_, 
                    LEDValueForState(this.LEDState.IsChecked.Value));
                OurImpl_.PostMessage(command);
                //MessageBox.Show(string.Format("LED Button: {0} LED: Inverted", this.LEDState.IsChecked.Value));
            }
        }
    }
}
