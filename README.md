#IOIODotNet#

Version 0.1 2014 Mar 03 joe@freemansoft.com

A crazy rough cut of a C# .Net library for the [IOIO device](https://github.com/ytai/ioio/wiki). 
It involves a **lot** of code copied from the Java application.  This is on purpose to simplifiy tracking 
future Java application and IOIO protocol changes. There is a lot of C# style work to be done.
IOIODotNet was built with VisualStudio 2013. 
Get the new Community edition if you don't have an MSDN license. There are only integration tests at this time.

You can send and receive messages from the board. The unit tests flash the LED and set and receive digital pin values.
**See the Unit tests** in the _IOIOLibDotNetTest_ project to see the current state and capabilities of the API.

##What Works##

1. This has been tested on a windows 8 pc with using an IOIO V1 over bluetooth. IOIO V2 OTG boards should also work
 * The library does support more than one device.
 * The serial factory can find devices or you can specify one explicitly. **See the unit tests**
2. Outgoing state does work using the IOIOOutgoingProtocol class.
 * Only Digital out has been tested.
 * Other features _may just work_ since the IOIOProtocol has been _mostly_ implemented.
3. Incoming state is received using the IOIOIncomingProtocol classes and and processed
  * IOIOIncomingHandlerDistributor can pass the incoming data messages to a set of other IOIOIncomingHandler objects
  * IOIOIncomingHandlerCaptureLog logs a message and captures the message every time a message is received from teh IOIO
  * IOIOIncomingHandlerCaptureStatus captures the status of each message retaining the last copy for a given pin or device
  * IOIOIncomingHandlerNotifier does nothing yet.  It will eventually post events to listeners interested in state change
4. Incoming state is captured in its own thread similar to the way the Java library works.
 * Only Digital In _on state change_ has been tested.
 * The thread/Task has a cancellation token that can be used to kill the inbound process.
 * Closing the communication device cleans up the thread also.
5. Basic board verification code works including getting the version strings from the IOIO on connection and sending the version confirmation string.
 * This code is not yet invoked unless you do it because IOIOImpl isn't built yet.  **See the unit tests for the current setup and teardown**


##What Doesnt Work##

1. IOIOImpl is not yet implemented. 
* This means board verification is not yet automatic
* This means the higher level APIs are not yet implemented
2. None of the feature abstractions have been implemented.
3. Change notification for information coming from the IOIO is not yet implemented.


##Issues##
There really are too many to list at this stage

1. Cleanup is very important.  You must close your device before exit otherwise windows bluetooth will get confused and you will not bbe able to open the device again.
 You will have to remove and re-add your bluetooth device if this happens
 * Look at the unit tests to see that the "after test" method closes any known serial devices.
2. There is a lot of C# naming convention changes to make but some really ugly _javaisms_ may stay to make java module change tracking easier.

##Unit Testing##

* Pair your IOIO with your PC.  
* The Digital Input / Digital Output test expects that pin 31 and 32 are connected
* The LED test should flash the LED twice on your device.
* Either let the unit tests find your device or set a device name by in _IOIOLibDotNetTest.TestHarnessSetup.cs_






