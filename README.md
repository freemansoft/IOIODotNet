#IOIODotNet#

Version 0.1 2014 Mar 07 [Joe Freeman](http://joe.blog.freemansoft.com) You can find a high level diagram 
[in this blog article](http://joe.blog.freemansoft.com/2015/03/extremely-rough-cut-at-c-based-ioio.html).

A crazy rough cut of a [C# .Net library for the IOIO device](https://github.com/ytai/ioio/wiki) on GitHub. 
It involves code copied from the Java application though the operational model is different.  This is on purpose 
to simplifiy tracking 
future Java application and IOIO protocol changes. There is a lot of C# style work to be done.
IOIODotNet was built with VisualStudio 2013. 
Get the new Community edition if you don't have an MSDN license. There are only integration tests at this time.

![alt text](http://1.bp.blogspot.com/-l3lVEHkgJkg/VPudkk-pXSI/AAAAAAAABw0/dnOpQ0RdS1A/s1600/IOIO%2BDot%2BNet.png "Logo Title Text 1")


##What Works##
Basic Analog and Digital functions work. The integration tests flash the LED and set and receive digital pin values.
**See the integration tests** in the _IOIOLibDotNetTest_ project to see the current state and capabilities of the API.

1. This has been tested on a windows 8 pc with using an IOIO V1 over bluetooth. IOIO V2 OTG boards should also work
 * The library does support more than one device.
 * The serial factory can find devices or you can specify one explicitly. **See the integration tests**

###Outgoing##
Outgoing state is mostly implemented using the IOIOOutgoingProtocol class. 
 * IOIOImpl manages IOIOOutgoingProtocol in its own thread. Callers post _IxxxTo_ messages to the IOIO which then communicate via message queue.
 * Integration tests demonstrate direct protocol communication and message based via IOIOImpl
 * Only Digital and Analog out have been tested.
 * Other features _may work_ since the IOIOProtocol has been _mostly_ implemented.
 * Outgoing Messages
     + There are two types of tests, those that call the outgoing protocol API directly and those that post messages to IOIOImpl. The message API will be the future API
     + The tests mostly build the messages directly. That is because they all involve individual pins 
     + Outbound messages will eventually be built through the factory.  That is where all the 
    constrained resource management will eventually be managed

###Incoming###
 * State is received using  **IOIOProtocolIncoming** which runs in its own thread
   + Incoming messages are distributed via handlers.  
   + Inbound data is packaged inot _IxxxFrom_ messages
   + Incoming state is captured in its own thread similar to the way the Java library works.
   + It can be killed using the token in IOIOImpl or through its own token if run standalone. Tests show both behaviors.
   + Closing the communication device cleans up the thread also.
 * Handlers
   + **IOIOHandlerDistributor** Distributes incoming messages to other IOIOIncomingHandler objects. This is used in all the tests and in IOIOImpl
   + **IOIOHandlerCaptureConnectionState** Captures just the connection information. Used by IOIOImpl.
   + **IOIOHandlerCaptureLog** Logs a message and captures the message every time a message is received from the IOIO. Can set buffer size
   + **IOIOHandlerCaptureSeparateQueue** Captures inbound messages classified by the inbound message interface type. See the integration test. This class is used in most of the tests to check return data.
   + **IOIOHandlerCaptureSingleQueue** Captures inbound messages in a single inbount ConcurrentQueue
   + **IOIOHandlerNotifier** does nothing yet.  It will eventually post events to listeners interested in state change
 * Digital In _on state change_ has been tested.
 * The existance of Analog return values is tested but not their values.

##Build Environemnt##
Visual Studio 2013 with .Net 4 on Windows 8.

###integration testing###

* Pair your IOIO with your PC.  
* The Digital Input / Digital Output test expects that pin 31 and 32 are connected
* The LED test should flash the LED twice on your device.
* Either let the integration tests find your device or set a device name by in _IOIOLibDotNetTest.TestHarnessSetup.cs_

Connection and resource setup and teardown occur in the testing base class. 
The teardown code closes connections.  Failure to do this correctly may force you to remove and re-pair the IOIO.
The teardown code requests thread cancellation for all IOIOImpl based tests. Failure to do this results in thread abandonment messages in the Visual Studio log window.

It is important to clean up after every test.  

##What Doesn't Work##

1. Resource management is not yet implemented.  This will probably be done in _IOIOMessageToFactory_
2. Robust setup and teardown is not yet implemented. 
  * Board verification is not yet automatic
  * Higher level APIs are not yet implemented
  * IOIOImpl methods are missing code
3. None of the complex abstractions or buses have been implemented.

###Outbound Messages###
There are missing Outbound message types _xxxTo_ .  The _IOIOMessageToFactory_ may not have factory methods for all message types.

###Inbound Change Notification###
Programs must poll for changes.  Change notification for information coming from the IOIO is not yet implemented.

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







