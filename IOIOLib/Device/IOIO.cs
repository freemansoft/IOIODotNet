using IOIOLib.Component;
using IOIOLib.Component.Types;
using IOIOLib.Device.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Device
{
/**
 * This interface provides control over all the IOIO board functions.
 * <p>
 * An instance of this interface is typically obtained by using the {@link IOIOFactory} class.
 * Initially, a connection should be established, by calling {@link #waitForConnect()}. This method
 * will block until the board is connected an a connection has been established.
 * <p>
 * During the connection process, this library verifies that the IOIO firmware is compatible with
 * the required version. If not, {@link #waitForConnect()} will throw a
 * {@link IncompatibilityException}, putting the {@link IOIO} instance in a "zombie" state: nothing
 * could be done with it except calling {@link #disconnect()}, or waiting for the physical
 * connection to drop via {@link #waitForDisconnect()}.
 * <p>
 * As soon as a connection is established, the IOIO can be used, typically, by calling the openXXX()
 * functions to obtain additional interfaces for controlling specific function of the board.
 * <p>
 * Whenever a connection is lost as a result of physically disconnecting the board or as a result of
 * calling {@link #disconnect()}, this instance and all the interfaces obtained from it become
 * invalid, and will throw a {@link ConnectionLostException} on every operation. Once the connection
 * is lost, those instances cannot be recycled, but rather it is required to create new ones and
 * wait for a connection again.
 * <p>
 * Initially all pins are tri-stated (floating), and all functions are disabled. Whenever a
 * connection is lost or dropped, the board will immediately return to the this initial state.
 * <p>
 * Typical usage:
 *
 * <pre>
 * IOIO ioio = IOIOFactory.create();
 * try {
 *   ioio.waitForConnect();
 *   IDigitalOutput out = ioio.openDigitalOutput(10);
 *   out.write(true);
 *   ...
 * } catch (ConnectionLostException e) {
 * } catch (Exception e) {
 *   ioio.disconnect();
 * } finally {
 *   ioio.waitForDisconnect();
 * }
 * </pre>
 *
 * @see IOIOFactory#create()
 */
    public interface IOIO
    {
    /**
     * Establishes connection with the IOIO board.
     * <p>
     * This method is blocking until connection is established. This method can be aborted by
     * calling {@link #disconnect()}. In this case, it will throw a {@link ConnectionLostException}.
     *
     * @throws ConnectionLostException
     *             An error occurred during connection or disconnect() has been called during
     *             connection. The instance state is disconnected.
     * @throws IncompatibilityException
     *             An incompatible board firmware of hardware has been detected. The instance state
     *             is disconnected.
     * @see #disconnect()
     * @see #waitForDisconnect()
     */
     void waitForConnect() ;

    /**
     * Closes the connection to the board, or aborts a connection process started with
     * waitForConnect().
     * <p>
     * Once this method is called, this IOIO instance and all the instances obtain from it become
     * invalid and will throw an exception on every operation.
     * <p>
     * This method is asynchronous, i.e. it returns immediately, but it is not guaranteed that all
     * connection-related resources has already been freed and can be reused upon return. In cases
     * when this is important, client can call {@link #waitForDisconnect()}, which will block until
     * all resources have been freed.
     */
     void disconnect();

    /**
     * Blocks until IOIO has been disconnected and all connection-related resources have been freed,
     * so that a new connection can be attempted.
     *
     * @throws InterruptedException
     *             When interrupt() has been called on this thread. This might mean that an
     *             immediate attempt to create and connect a new IOIO object might fail for resource
     *             contention.
     * @see #disconnect()
     * @see #waitForConnect()
     */
     void waitForDisconnect();

    /**
     * Gets the connections state.
     *
     * @return The connection state.
     */
     IOIOState getState();

    /**
     * Resets the entire state (returning to initial state), without dropping the connection.
     * <p>
     * It is equivalent to calling {@link Closeable#close()} on every interface obtained from this
     * instance. A connection must have been established prior to calling this method, by invoking
     * {@link #waitForConnect()}.
     *
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @see #hardReset()
     */
     void softReset();

    /**
     * Equivalent to disconnecting and reconnecting the board power supply.
     * <p>
     * The connection will be dropped and not reestablished. Full boot sequence will take place, so
     * firmware upgrades can be performed. A connection must have been established prior to calling
     * this method, by invoking {@link #waitForConnect()}.
     *
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @see #softReset()
     */
     void hardReset();

    /**
     * Query the implementation version of the system's components. The implementation version
     * uniquely identifies a hardware revision or a software build. Returned version IDs are always
     * 8-character long, according to the IOIO versioning system: first 4 characters are the version
     * authority and last 4 characters are the revision.
     *
     * @param v
     *            The component whose version we query.
     * @return An 8-character implementation version ID.
     */
     String getImplVersion(IOIOVersionType v);

    /**
     * Open a pin for digital input.
     * <p>
     * A digital input pin can be used to read logic-level signals. The pin will operate in this
     * mode until close() is invoked on the returned interface. It is illegal to open a pin that has
     * already been opened and has not been closed. A connection must have been established prior to
     * calling this method, by invoking {@link #waitForConnect()}.
     *
     * @param spec
     *            Pin specification, consisting of the pin number, as labeled on the board, and the
     *            mode, which determines whether the pin will be floating, pull-up or pull-down. See
     *            {@link IDigitalInputSpec.Mode} for more information.
     * @return Interface of the assigned pin.
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @see IDigitalInput
     */
     IDigitalInput openDigitalInput(DigitalInputSpec spec);

    /**
     * Shorthand for openDigitalInput(new IDigitalInputSpec(pin)).
     *
     * @see #openDigitalInput(ioio.lib.api.DigitalInputSpec)
     */
     IDigitalInput openDigitalInput(int pin);

    /**
     * Shorthand for openDigitalInput(new IDigitalInputSpec(pin, mode)).
     *
     * @see #openDigitalInput(ioio.lib.api.DigitalInputSpec)
     */
     IDigitalInput openDigitalInput(int pin, DigitalInputSpecMode mode);

    /**
     * Open a pin for digital output.
     * <p>
     * A digital output pin can be used to generate logic-level signals. The pin will operate in
     * this mode until close() is invoked on the returned interface. It is illegal to open a pin
     * that has already been opened and has not been closed. A connection must have been established
     * prior to calling this method, by invoking {@link #waitForConnect()}.
     *
     * @param spec
     *            Pin specification, consisting of the pin number, as labeled on the board, and the
     *            mode, which determines whether the pin will be normal or open-drain. See
     *            {@link IDigitalOutput.Spec.Mode} for more information.
     * @param startValue
     *            The initial logic level this pin will generate as soon at it is open.
     * @return Interface of the assigned pin.
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @see IDigitalOutput
     */
     IDigitalOutput openDigitalOutput(DigitalOutputSpec spec, bool startValue)
            ;

    /**
     * Shorthand for openDigitalOutput(new IDigitalOutput.Spec(pin, mode), startValue).
     *
     * @see #openDigitalOutput(ioio.lib.api.DigitalOutput.Spec, bool)
     */
     IDigitalOutput openDigitalOutput(int pin, DigitalOutputSpecMode mode, bool startValue)
            ;

    /**
     * Shorthand for openDigitalOutput(new IDigitalOutput.Spec(pin), startValue). Pin mode will be
     * "normal" (as opposed to "open-drain".
     *
     * @see #openDigitalOutput(ioio.lib.api.DigitalOutput.Spec, bool)
     */
     IDigitalOutput openDigitalOutput(int pin, bool startValue)
            ;

    /**
     * Shorthand for openDigitalOutput(new IDigitalOutput.Spec(pin), false). Pin mode will be
     * "normal" (as opposed to "open-drain".
     *
     * @see #openDigitalOutput(ioio.lib.api.DigitalOutput.Spec, bool)
     */
     IDigitalOutput openDigitalOutput(int pin);

    /**
     * Open a pin for analog input.
     * <p>
     * An analog input pin can be used to measure voltage. Note that not every pin can be used as an
     * analog input. See board documentation for the legal pins and permitted voltage range.
     * <p>
     * The pin will operate in this mode until close() is invoked on the returned interface. It is
     * illegal to open a pin that has already been opened and has not been closed. A connection must
     * have been established prior to calling this method, by invoking {@link #waitForConnect()}.
     *
     * @param pin
     *            Pin number, as labeled on the board.
     * @return Interface of the assigned pin.
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @see AnalogInput
     */
     IAnalogInput openAnalogInput(int pin);

    /**
     * Open a pin for PWM (Pulse-Width Modulation) output.
     * <p>
     * A PWM pin produces a logic-level PWM signal. These signals are typically used for simulating
     * analog outputs for controlling the intensity of LEDs, the rotation speed of motors, etc. They
     * are also frequently used for controlling hobby servo motors.
     * <p>
     * Note that not every pin can be used as PWM output. In addition, the total number of
     * concurrent PWM modules in use is limited. See board documentation for the legal pins and
     * limit on concurrent usage.
     * <p>
     * The pin will operate in this mode until close() is invoked on the returned interface. It is
     * illegal to open a pin that has already been opened and has not been closed. A connection must
     * have been established prior to calling this method, by invoking {@link #waitForConnect()}.
     *
     * @param spec
     *            Pin specification, consisting of the pin number, as labeled on the board, and the
     *            mode, which determines whether the pin will be normal or open-drain. See
     *            {@link IDigitalOutput.Spec.Mode} for more information.
     * @param freqHz
     *            PWM frequency, in Hertz.
     * @return Interface of the assigned pin.
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @throws OutOfResourceException
     *             This is a runtime exception, so it is not necessary to catch it if the client
     *             guarantees that the total number of concurrent PWM resources is not exceeded.
     * @see PwmOutput
     */
     IPwmOutput openPwmOutput(DigitalOutputSpec spec, int freqHz)
            ;

    /**
     * Shorthand for openPwmOutput(new IDigitalOutput.Spec(pin), freqHz).
     *
     * @see #openPwmOutput(ioio.lib.api.DigitalOutput.Spec, int)
     */
     IPwmOutput openPwmOutput(int pin, int freqHz);

    /**
     * Open a pin for pulse input.
     * <p>
     * The pulse input module is quite flexible. It enables several kinds of timing measurements on
     * a digital signal: pulse width measurement (positive or negative pulse), and frequency of a
     * periodic signal.
     * <p>
     * Note that not every pin can be used as pulse input. In addition, the total number of
     * concurrent pulse input modules in use is limited. See board documentation for the legal pins
     * and limit on concurrent usage.
     * <p>
     * The pin will operate in this mode until close() is invoked on the returned interface. It is
     * illegal to open a pin that has already been opened and has not been closed. A connection must
     * have been established prior to calling this method, by invoking {@link #waitForConnect()}.
     *
     * @param spec
     *            Pin specification, consisting of the pin number, as labeled on the board, and the
     *            mode, which determines whether the pin will be floating, pull-up or pull-down. See
     *            {@link IDigitalInputSpec.Mode} for more information.
     * @param rate
     *            The clock rate to use for timing the signal. A faster clock rate will result in
     *            better precision but will only be able to measure narrow pulses / high
     *            frequencies.
     * @param mode
     *            The mode in which to operate. Determines whether the module will measure pulse
     *            durations or frequency.
     * @param doublePrecision
     *            Whether to open a double-precision pulse input module. Double- precision modules
     *            enable reading of much longer pulses and lower frequencies with high accuracy than
     *            single precision modules. However, their number is limited, so when possible, and
     *            if the resources are all needed, use single-precision. For more details on the
     *            exact spec of single- vs. double- precision, see {@link IPulseInput}.
     * @return An instance of the {@link IPulseInput}, which can be used to obtain the data.
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @throws OutOfResourceException
     *             This is a runtime exception, so it is not necessary to catch it if the client
     *             guarantees that the total number of concurrent PWM resources is not exceeded.
     * @see IPulseInput
     */
     IPulseInput openPulseInput(DigitalInputSpec spec, PulseInputClockRate rate,
            PulseInputMode mode, bool doublePrecision);

    /**
     * Shorthand for openPulseInput(new IDigitalInputSpec(pin), rate, mode, true), i.e. opens a
     * double-precision, 16MHz pulse input on the given pin with the given mode.
     *
     * @see #openPulseInput(ioio.lib.api.DigitalInputSpec, ioio.lib.api.PulseInput.ClockRate,
     *      ioio.lib.api.PulseInput.PulseMode, bool)
     */
     IPulseInput openPulseInput(int pin, PulseInputMode mode);

    /**
     * Open a UART module, enabling a bulk transfer of byte buffers.
     * <p>
     * UART is a very common hardware communication protocol, enabling full- duplex, asynchronous
     * point-to-point data transfer. It typically serves for opening consoles or as a basis for
     * higher-level protocols, such as MIDI RS-232, and RS-485.
     * <p>
     * Note that not every pin can be used for UART RX or TX. In addition, the total number of
     * concurrent UART modules in use is limited. See board documentation for the legal pins and
     * limit on concurrent usage.
     * <p>
     * The UART module will operate, and the pins will work in their respective modes until close()
     * is invoked on the returned interface. It is illegal to use pins that have already been opened
     * and has not been closed. A connection must have been established prior to calling this
     * method, by invoking {@link #waitForConnect()}.
     *
     * @param rx
     *            Pin specification for the RX pin, consisting of the pin number, as labeled on the
     *            board, and the mode, which determines whether the pin will be floating, pull-up or
     *            pull-down. See {@link IDigitalInputSpec.Mode} for more information. null can be
     *            passed to designate that we do not want RX input to this module.
     * @param tx
     *            Pin specification for the TX pin, consisting of the pin number, as labeled on the
     *            board, and the mode, which determines whether the pin will be normal or
     *            open-drain. See {@link IDigitalOutput.Spec.Mode} for more information. null can be
     *            passed to designate that we do not want TX output to this module.
     * @param baud
     *            The clock frequency of the UART module in Hz.
     * @param parity
     *            The parity mode, as in {@link Parity}.
     * @param stopbits
     *            Number of stop bits, as in {@link StopBits}.
     * @return Interface of the assigned module.
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @throws OutOfResourceException
     *             This is a runtime exception, so it is not necessary to catch it if the client
     *             guarantees that the total number of concurrent UART resources is not exceeded.
     * @see Uart
     */
     IUart openUart(DigitalInputSpec rx, DigitalOutputSpec tx, int baud, UartParity parity,
            UartStopBits stopbits);

    /**
     * Shorthand for
     * {@link #openUart(DigitalInputSpec, IDigitalOutput.Spec, int, Uart.Parity, Uart.StopBits)} ,
     * where the input pins use their default specs. {@link #INVALID_PIN} can be used on either pin
     * if a TX- or RX-only UART is needed.
     *
     * @see #openUart(DigitalInputSpec, IDigitalOutput.Spec, int, Uart.Parity, Uart.StopBits)
     */
     IUart openUart(int rx, int tx, int baud, UartParity parity, UartStopBits stopbits)
            ;

    /**
     * Open a SPI master module, enabling communication with multiple SPI-enabled slave modules.
     * <p>
     * SPI is a common hardware communication protocol, enabling full-duplex, synchronous
     * point-to-multi-point data transfer. It requires MOSI, MISO and CLK lines shared by all nodes,
     * as well as a SS line per slave, connected between this slave and a respective pin on the
     * master. The MISO line should operate in pull-up mode, using either the internal pull-up or an
     * external resistor.
     * <p>
     * Note that not every pin can be used for SPI MISO, MOSI or CLK. In addition, the total number
     * of concurrent SPI modules in use is limited. See board documentation for the legal pins and
     * limit on concurrent usage.
     * <p>
     * The SPI module will operate, and the pins will work in their respective modes until close()
     * is invoked on the returned interface. It is illegal to use pins that have already been opened
     * and has not been closed. A connection must have been established prior to calling this
     * method, by invoking {@link #waitForConnect()}.
     *
     * @param miso
     *            Pin specification for the MISO (Master In Slave Out) pin, consisting of the pin
     *            number, as labeled on the board, and the mode, which determines whether the pin
     *            will be floating, pull-up or pull-down. See {@link IDigitalInputSpec.Mode} for
     *            more information.
     * @param mosi
     *            Pin specification for the MOSI (Master Out Slave In) pin, consisting of the pin
     *            number, as labeled on the board, and the mode, which determines whether the pin
     *            will be normal or open-drain. See {@link IDigitalOutput.Spec.Mode} for more
     *            information.
     * @param clk
     *            Pin specification for the CLK pin, consisting of the pin number, as labeled on the
     *            board, and the mode, which determines whether the pin will be normal or
     *            open-drain. See {@link IDigitalOutput.Spec.Mode} for more information.
     * @param slaveSelect
     *            An array of pin specifications for each of the slaves' SS (Slave Select) pin. The
     *            index of this array designates the slave index, used later to refer to this slave.
     *            The spec is consisting of the pin number, as labeled on the board, and the mode,
     *            which determines whether the pin will be normal or open-drain. See
     *            {@link IDigitalOutput.Spec.Mode} for more information.
     * @param config
     *            The configuration of the SPI module. See {@link SpiMaster.Config} for details.
     * @return Interface of the assigned module.
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @throws OutOfResourceException
     *             This is a runtime exception, so it is not necessary to catch it if the client
     *             guarantees that the total number of concurrent SPI resources is not exceeded.
     * @see SpiMaster
     */
     ISpiMaster openSpiMaster(DigitalInputSpec miso, DigitalOutputSpec mosi,
            DigitalOutputSpec clk, DigitalOutputSpec[] slaveSelect, SpiMasterConfig config)
            ;

    /**
     * Shorthand for
     * {@link #openSpiMaster(ioio.lib.api.DigitalInputSpec, ioio.lib.api.DigitalOutput.Spec, ioio.lib.api.DigitalOutput.Spec, ioio.lib.api.DigitalOutput.Spec[], ioio.lib.api.SpiMaster.Config)}
     * , where the pins are all open with the default modes and default configuration values are
     * used.
     *
     * @see #openSpiMaster(ioio.lib.api.DigitalInputSpec, ioio.lib.api.DigitalOutput.Spec,
     *      ioio.lib.api.DigitalOutput.Spec, ioio.lib.api.DigitalOutput.Spec[],
     *      ioio.lib.api.SpiMaster.Config)
     */
     ISpiMaster openSpiMaster(int miso, int mosi, int clk, int[] slaveSelect,
            SpiMasterRate rate);

    /**
     * Shorthand for
     * {@link #openSpiMaster(ioio.lib.api.DigitalInputSpec, ioio.lib.api.DigitalOutput.Spec, ioio.lib.api.DigitalOutput.Spec, ioio.lib.api.DigitalOutput.Spec[], ioio.lib.api.SpiMaster.Config)}
     * , where the MISO pins is opened with pull up, and the other pins are open with the default
     * modes and default configuration values are used. In this version, a single slave is used.
     *
     * @see #openSpiMaster(ioio.lib.api.DigitalInputSpec, ioio.lib.api.DigitalOutput.Spec,
     *      ioio.lib.api.DigitalOutput.Spec, ioio.lib.api.DigitalOutput.Spec[],
     *      ioio.lib.api.SpiMaster.Config)
     */
     ISpiMaster openSpiMaster(int miso, int mosi, int clk, int slaveSelect, SpiMasterRate rate)
            ;

    /**
     * Open a TWI (Two-Wire Interface, such as I2C/SMBus) master module, enabling communication with
     * multiple TWI-enabled slave modules.
     * <p>
     * TWI is a common hardware communication protocol, enabling half-duplex, synchronous
     * point-to-multi-point data transfer. It requires a physical connection of two lines (SDA, SCL)
     * shared by all the bus nodes, where the SDA is open-drain and externally pulled-up.
     * <p>
     * Note that there is a fixed number of TWI modules, and the pins they use are static. Client
     * has to make sure these pins are not already opened before calling this method. See board
     * documentation for the number of modules and the respective pins they use.
     * <p>
     * The TWI module will operate, and the pins will work in their respective modes until close()
     * is invoked on the returned interface. It is illegal to use pins that have already been opened
     * and has not been closed. A connection must have been established prior to calling this
     * method, by invoking {@link #waitForConnect()}.
     *
     * @param twiNum
     *            The TWI module index to use. Will also determine the pins used.
     * @param rate
     *            The clock rate. Can be 100KHz / 400KHz / 1MHz.
     * @param smbus
     *            When true, will use SMBus voltage levels. When false, I2C voltage levels.
     * @return Interface of the assigned module.
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @see TwiMaster
     */
     ITwiMaster openTwiMaster(int twiNum, TwiMasterRate rate, bool smbus)
            ;

    /**
     * Open an ICSP channel, enabling Flash programming of an external PIC MCU, and in particular,
     * another IOIO board.
     * <p>
     * ICSP (In-Circuit Serial Programming) is a protocol intended for programming of PIC MCUs. It
     * is a serial protocol over three wires: PGC (clock), PGD (data) and MCLR (reset), where PGC
     * and MCLR are controlled by the master and PGD is shared by the master and slave, depending on
     * the transaction state.
     * <p>
     * Note that there is only one ICSP modules, and the pins it uses are static. Client has to make
     * sure that the ICSP module is not already in use, as well as those dedicated pins. See board
     * documentation for the actual pins used for ICSP.
     *
     * @return Interface of the ICSP module.
     * @see IcspMaster
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     */
     IIcspMaster openIcspMaster();

    /**
     * Shorthand for openCapSense(pin, CapSense.DEFAULT_COEF).
     *
     * @see #openCapSense(int, float)
     */
     ICapSense openCapSense(int pin);

    /**
     * Open a pin for cap-sense.
     * <p>
     * A cap-sense input pin can be used to measure capacitance, typically in touch sensing
     * applications. Note that not every pin can be used as cap- sense. See board documentation for
     * the legal pins.
     * <p>
     * The pin will operate in this mode until close() is invoked on the returned interface. It is
     * illegal to open a pin that has already been opened and has not been closed. A connection must
     * have been established prior to calling this method, by invoking {@link #waitForConnect()}.
     *
     * @param pin
     *            Pin number, as labeled on the board.
     * @return Interface of the assigned pin.
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @see CapSense
     */
     ICapSense openCapSense(int pin, float filterCoef);

    /**
     * Open a motion-control sequencer.
     * <p>
     * This module allows fast and precise sequencing of waveforms, primarily intended for
     * synchronized driving of various kinds of motors and other actuators. There is currently
     * support for only a single instance of this module. For more details, see {@link Sequencer}.
     *
     * @param config
     *            The sequencer configuration.
     * @return The sequencer instance.
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     */
     ISequencer openSequencer(ISequencerChannelConfig[] config);

    /**
     * Start a batch of operations. This is strictly an optimization and will not change
     * functionality: if the client knows that a sequence of several IOIO operations are going to be
     * performed immediately following each other, a call to {@link #beginBatch()} before the
     * sequence and {@link #endBatch()} after the sequence will cause the operations to be grouped
     * into one transfer to the IOIO, thus reducing latency. A matching {@link #endBatch()}
     * operation must always follow, or otherwise no operation will ever be actually executed.
     * {@link #beginBatch()} / {@link #endBatch()} blocks may be nested - the transfer will occur
     * when the outermost {@link #endBatch()} is invoked. Note that it is not guaranteed that no
     * transfers will happen while inside a batch - it should be treated as a hint. Code running
     * inside the block must be quick as it blocks <b>all</b> transfers to the IOIO, including those
     * performed from other threads.
     *
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     */
     void beginBatch();

    /**
     * End a batch of operations. For explanation, see {@link #beginBatch()}.
     *
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     */
     void endBatch();

    /**
     * Sends a message to the IOIO and waits for an echo.
     *
     * This is useful for synchronizing asynchronous calls across the entire API, for example: When
     * writing to a {@link IDigitalOutput} and then reading from a {@link IDigitalInput}, if you want
     * to guarantee that the reading was obtained after the write has taken place, call this method
     * in between.
     *
     * @throws ConnectionLostException
     *             Connection was lost before or during the execution of this method.
     * @throws InterruptedException
     *             When interrupt() has been called on this thread.
     */
     void sync() ;
   }
}
