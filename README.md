#IOIODotNet#

[Joe Freeman](http://joe.blog.freemansoft.com) 
You can find a high level diagram 
[in this blog article](http://joe.blog.freemansoft.com/2015/03/extremely-rough-cut-at-c-based-ioio.html).

* Version 0.2 2015 Mar 22 Added Observer Notification
* Version 0.3 2015 Apr 12 Added I2C

A crazy rough cut of a [C# .Net library for the IOIO device](https://github.com/ytai/ioio/wiki) on GitHub. It involves code copied from the Java application though the operational model is different.  This is on purpose to simplifiy tracking future Java application and IOIO protocol changes. There is a lot of C# style work to be done.

IOIODotNet was built with VisualStudio 2015 CTP6. 

![alt text](http://1.bp.blogspot.com/-l3lVEHkgJkg/VPudkk-pXSI/AAAAAAAABw0/dnOpQ0RdS1A/s1600/IOIO%2BDot%2BNet.png "Logo Title Text 1")


##What Works##
Basic Analog, Digital and Uart functions work. The integration tests flash the LED, set and receive digital pin values, read analog values and send and receive serial data via uart. **See the integration tests** in the _IOIOLibDotNetTest_ project to see the current state and capabilities of the API.

This has been tested on a windows 8 pc with using an IOIO V1 over bluetooth. IOIO V2 OTG boards _should_ also work
 * The library should support more than one device.
 * The serial factory can find devices or you can specify one explicitly. **See the integration tests**

###Resource Management###
Basic resource management has been implemented. IOIOImpl works with a ResourceManager to reserve and release.  Unit tests that bypass IOIOImpl and/or mock ResourceManagement work because they operate on a _clean_ board where each test can grab whatever it wants.

###Outgoing###

The IOIO is programmed via _messages_ of type _ICommandToIOIO_.  Programs create messages and feed them to the IOIO thorugh _IOIOImpl_. which relies on _IOIOOutgoingProtocol_ to do the actual communicaton.  

 * IOIOImpl manages IOIOOutgoingProtocol in its own thread. Callers post _IxxxTo_ messages to the IOIO which then communicate via message queue.
 * _IOIOMessageCommandFactory_ provides the public interface for creating outgoing messages.
  + This represents the public API for creating messages
 * Other features _may work_ at the raw command level since the IOIOProtocol has been _mostly_ implemented.
 * Tests
  + Integration tests demonstrate direct protocol communication  IOIOImpl
  + There are two types of tests, those that call the outgoing protocol API directly and those that post messages to IOIOImpl. The message API will be the future API
  + The tests mostly build the messages directly. That is because they all involve individual pins
 * Outgoing Messages
  + Outbound messages will eventually be built through the _IOIOMessageCommandFactory_.  That will eventually be the only public creation interface
  + Outbound messages implement the _IPostMessageCommand_ interface that binds to the _ResourceManager_ to allocate and free IOIO board pins, timers and other resources

Some messages create a peripheral identifier that must be used to identify that peripheral when sending data or closing that peripheral.  The peripheral is set on the first command and must be extracted to add to subsequent commands.

####Feature Status ####
Digital, Analog Out and Uart have been lightly tested.

###Incoming###
 * State is received using  **IOIOProtocolIncoming** which is created by **IOIOImpl** runs in its own thread
   + Incoming messages are distributed via handlers.  
   + Inbound data is packaged inot _IxxxFrom_ messages
   + Incoming state is captured in its own thread similar to the way the Java library works.
   + It can be killed using the token in IOIOImpl or through its own token if run standalone. Tests show both behaviors.
   + Closing the communication device cleans up the thread also.
 * Handlers
   + **IOIOHandlerDistributor** Distributes incoming messages to other IOIOIncomingHandler objects. This is used in all the tests and in IOIOImpl
   + **IOIOHandlerCaptureConnectionState** Captures just the connection information. Used by IOIOImpl.
   + **IOIOHandlerCaptureLog** Logs a message and captures the message every time a message is received from the IOIO. Can set buffer size
   + **IOIOHandlerCaptureSingleQueue** Captures inbound messages in a single inbount ConcurrentQueue
   + **IOIOHandlerObservable**  Observers can register with this handler. They will receive IObserver<> interface messages
   + **IOIOHandlerObservableNoWait** Similar to IHandlerObservable except that notificatons happen in separate thread from message receiver
   + **IOIOHandlerObservableNoWaitParallel** Similar to IHandlerObservable except that each notificaton happens in its' on thread

####Feature Status####
 * Digital In _on state change_ has been tested.
 * Uart data has been received
 * The existance of Analog return values is tested but not their values.
 * I2C has been verified with JeeLabs Expander

##Build Environemnt##
Visual Studio 2015 with .Net 4 on Windows 8. Get the new Community edition if you don't have an MSDN license. There are only integration tests at this time.

###integration testing###

* Pair your IOIO with your PC.  
* The Digital Input / Digital Output tests expect that pin 31 and 32 are connected
* The Uart tests expect that pin 31 and 32 are connected
* The LED test should flash the LED twice on your device.
* Either let the integration tests find your device or set a device name by in _IOIOLibDotNetTest.TestHarnessSetup.cs_
* TwiI2CTest will fail if you do not have I2C device on TWI-0 at address 0x20 -- tested with Jee labs expander.
Don't worry about it if you don't have an expander

Connection and resource setup and teardown occur in the testing base class. The teardown code closes connections.  Failure to do this correctly may force you to remove and re-pair the IOIO. The teardown code requests thread cancellation for all _IOIOImpl_ based tests. Failure to do this results in thread abandonment messages in the Visual Studio log window.

It is important to clean up after every test.  

###Example Program###
A simple winforms app project demonstrates servo contro.  It will auto-identify the IOIO COM port. 
The demo assumes a servo is hooked to pin 3 just like the Integration Tests

##What Doesn't Work##

1. Robust setup and teardown is not yet implemented. 
  * Board verification is not yet automatic
  * Higher level APIs are not yet implemented
  * IOIOImpl methods are missing code
2. ICSP and SPI have not been coded. I don't have any peripherals of that type
3. Pulse Input has not be tested
4. I2C, Uart and SPI flow control is NOT implemented. 
handleI2cReportTxStatus,  handleUartReportTxStatus, handleSpiReportTxStatus are all ignored.

###Outbound Messages###
Outbound message types_xxxTo_ have not been fully built for peripherals that have not yet been implemented.  The _IOIOMessageToFactory_ may not have factory methods for all message types.

###Inbound Change Notification###
Programs can poll for changes or be notified based on the handler.  
Change notification loosely based on Observer/Observable pattern. See unit tests for examples

###Hardening###
Board verificaton is not finished. 
Basic board verification code works including getting the version strings from the IOIO on connection and sending the version confirmation string.
This code is not yet invoked unless you do it because IOIOImpl isn't built out.  **See the integration tests** for the current setup and teardown



###Issues###
There really are too many to list at this stage

1. Cleanup is very important.  You must close your device before exit otherwise windows bluetooth will get confused and you will not bbe able to open the device again.
 You will have to remove and re-add your bluetooth device if this happens
 * Look at the integration tests to see that the "after test" method closes any known serial devices.
2. There is a lot of C# naming convention changes to make but some really ugly _javaisms_ may stay to make java module change tracking easier.







