using IOIOLib.Component.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component
{
    /**
     * A waveform sequencer.
     * <p>
     * The {@link Sequencer} interface enables generation of very precisely-timed digital waveforms,
     * primarily targeted for different kinds of actuators, such as different kinds of motors, solenoids
     * and LEDs. The output comprises one or more channels, each of which can have one of several
     * different types. The output channels type and their assignments to pins are determined when the
     * {@link Sequencer} instance is created and cannot be changed during its lifetime. A
     * {@link Sequencer} instance is obtained via {@link IOIO#openSequencer(ChannelConfig[])}.
     * <p>
     * Each channel is configured with one of the {@link ChannelConfig} subclasses. For example,
     * {@link ChannelCueBinary} specified a binary output, useful for controlling an LED or a solenoid.
     * This configuration includes, among other things, the pin specification for this channel. See the
     * individual subclasses documentation for more information.
     * <p>
     * Once instantiated, the {@link Sequencer} gets fed with a stream_ of &lt;cue, duration&gt; pairs. A
     * cue is a set of instructions telling each of the channels what to do, or more precisely, one
     * {@link ChannelCue} per channel. The concrete type of the {@link ChannelCue} depends on the
     * channel type, for example, a channel that has been configured with {@link ChannelConfigBinary},
     * expects a {@link ChannelCueBinary} for its cue. In this example, the cue will specify whether
     * this output should be high or low. When a cue is combined with a duration, it has the semantics
     * of "for this much time, generate this waveform". A stream_ of such pairs can represent a complex
     * multi-channel waveform.
     * <p>
     * The above &lt;cue, duration&gt; pairs are pushed into the {@link Sequencer} via the
     * {@link #push(ChannelCue[], int)} method. They will not yet be executed, but rather queue up.
     * There is a limited number of cues that can be queued. Attempting to push more will block until a
     * cue has been executed. In order to avoid blocking (especially during the initial fill of the
     * queue - see below), the {@link #available()} method returns the number of pushes that can be done
     * without blocking. Once a few of them have been queued up, execution may start by calling
     * {@link #start()} . During execution, more cues can be pushed, and ideally fast enough so that
     * execution never needs to stall. During execution, {@link #pause()} will suspend execution as soon
     * as the currently executed cue is done, without clearing the queue of pending cues. Operation can
     * then be resumed by calling {@link #start()} again. Calling {@link #stop()} will immediately stop
     * execution and clear the queue. Execution progress can be tracked at any time by calling
     * {@link #numCuesStarted()}, which will increment every time actual execution of a cue has begun.
     * It will be reset back to 0 following a {@link #stop()}.
     *
     * <h4>Pre-fill</h4>
     * In order to avoid stalls immediately after {@link #start()}, it is recommended to pre-fill the
     * cue FIFO priorly. The recommended sequence of operations is:
     *
     * <pre>
     * // Open the sequencer.
     * Sequencer s = ioio_.openSequencer(...);
     * // At this point, the FIFO might still be at zero capacity, wait until opening is complete.
     * s.waitEventType(Sequencer.Event.Type.STOPPED);
     * // Now our FIFO is empty and at full capacity. Fill it entirely.
     * while (s.available() > 0) {
     *   s.push(...);
     * }
     * // Now we can start!
     * s.start();
     * </pre>
     *
     * <h4>Manual Operation</h4>
     * In some cases it is useful to be able to execute some cues while the {@link Sequencer} is paused
     * or stopped without having to clear the queue. For this purpose, the
     * {@link #manualStart(ChannelCue[])} command can be used. Calling it will immediately begin
     * execution of a given cue, without changing the queue previously given. Unlike cues scheduled via
     * {@link #push(ChannelCue[], int)}, these cues have no duration, and will keep executing until
     * {@link #manualStop()} is called. After that, normal operation can be resumed by calling
     * {@link #start()} again.
     *
     * <h4>States</h4>
     * This table summarizes the different states the {@link Sequencer} can be in, and the methods which
     * can be called to change it:
     * <table border="1">
     * <tr>
     * <th>Current State</th>
     * <th>Method</th>
     * <th>Next State</th>
     * </tr>
     * <tr>
     * <td rowspan="4">Idle</td>
     * <td>{@link #start()}</td>
     * <td>Running</td>
     * </tr>
     * <tr>
     * <td>{@link #manualStart(ChannelCue[])}</td>
     * <td>Manual</td>
     * </tr>
     * <tr>
     * <td>{@link #stop()}</td>
     * <td>Idle (and clear the queue)</td>
     * </tr>
     * <tr>
     * <td>{@link #manualStop()}</td>
     * <td>Idle (no-op)</td>
     * </tr>
     * <tr>
     * <td rowspan="2">Running</td>
     * <td>{@link #pause()}</td>
     * <td>Idle</td>
     * </tr>
     * <tr>
     * <td>{@link #stop()}</td>
     * <td>Idle (and clear the queue)</td>
     * </tr>
     * <tr>
     * <td rowspan="3">Manual</td>
     * <td>{@link #manualStop()}</td>
     * <td>Idle</td>
     * </tr>
     * <tr>
     * <td>{@link #stop()}</td>
     * <td>Idle (and clear the queue)</td>
     * </tr>
     * <tr>
     * <td>{@link #manualStart()}</td>
     * <td>Manual (new cue)</td>
     * </tr>
     * </table>
     *
     * <h4>Clocking</h4>
     * All the timing information provided to a single channel is based on a clock which implies a
     * time-base. For some channel types, the clock is determined upon instantiation; for others it can
     * be set on a per-cue basis. Once a clock rate is set, all timing values used for that channel are
     * in units of this time-base. Using a faster clock rate enables faster waveforms to be generated as
     * well as have better timing resolution for everything. However, since most timing values used in
     * this API have a limited range, using a faster clock would also limit the maximum duration of some
     * signals. As a rule of thumb, choosing the highest possible clock rate that can satisfy the
     * duration requirements is the right choice.
     *
     * <h4>Stalls</h4>
     * It is possible that the client does not push cues fast enough to keep up with execution. In that
     * case, the queue will drain and execution will stall. All channel types have well-defined
     * behaviors in case of a stall, for example, a binary channel allows the user to explicitly
     * determine whether it should be go back to its initial value or retain the current value when
     * stalled. Once stalled, as soon as a new cue gets pushed, operation will resume immediately
     * without having to call {@link #start()}.
     *
     * <h4>Keeping track of execution</h4>
     * Since execution of queued events is asynchronous, it is sometimes required to track their
     * execution progress, for example, for keeping a user interface synchronized with the actual state
     * of a multi-axis machine.
     * <p>
     * The client can poll for the last event that occured via {@link #getLastEvent()}, or block until
     * the next one arrives using {@link #waitEvent()}. The latter features a limited-size queue, so
     * events are not lost as long as the client reads continuously. The queue is initially 32-events
     * long, but could be changed using {@link #setEventQueueSize(int)}. The sequencer will report the
     * following kinds of events, via the {@link Event} type:
     * <dl>
     * <dt>STOPPED</dt>
     * <dd>The sequencer has been stopped (and the cue FIFO is now empty). This is also the event that
     * is returned when calling {@link #getLastEvent()} before any event has arrived.</dd>
     * <dt>CUE_STARTED</dt>
     * <dd>A new cue has just started execution.</dd>
     * <dt>PAUSED</dt>
     * <dd>A cue has just finished execution and progress has been paused as result of an explicit pause
     * request.</dd>
     * <dt>STALLED</dt>
     * <dd>A cue has just finished execution and progress has been paused as result of the queue running
     * empty. In this case, the state of the sequencer does not change (i.e. it is still Running), and
     * pushing addition events will immediately resume execution.</dd>
     * <dt>CLOSED</dt>
     * <dd>The sequencer has been closed. This is mostly intended for gracefully exiting a thread which
     * is constantly blocking on {@link #waitEvent()}</dd>
     * </dl>
     * <p>
     *
     */
    public interface ISequencer
    {
        /**
         * Push a timed cue to the sequencer.
         * <p>
         * This method will block until there is at least one free space in the FIFO (which may be
         * forever if the sequencer is not running -- use {@link Sequencer.available()} first in this
         * case). Then, it will queue the cue for execution.
         *
         * @param cues
         *            An array of channel cues. Has to be the exact same length as the
         *            {@link ChannelConfig} array that was used to configure the sequencer. Each
         *            element's type should be the counterpart of the corresponding configuration type.
         *            For example, it element number 5 in the configuration array was of type
         *            {@link Sequencer.ChannleConfigBinary}, then cues[5] needs to be of type
         *            {@link Sequencer.ChannelCueBinary}
         * @param duration
         *            The time duration for which this cue is to be executed, before moving to the next
         *            cue (or stalling). The units are 16 microseconds. For example, passing 10 here
         *            would mean a duration of 160 microseconds. Valid values are [2..65536] (approx.
         *            1.05 seconds).
         * @throws ConnectionLostException
         *             Connection to the IOIO was lost before or during this operation.
         * @throws InterruptedException
         *             The operation was interrupted before completion.
         */
         void push(ISequencerChannelCue[] cues, int duration);

        /**
         * Execute a cue until further notice.
         * <p>
         * This method may only be called when the sequencer is not in the Running state. It will not
         * affect the queue of pending timed-cues previously filled via {@link #push(ChannelCue[], int)}
         * calls. The cue will be executed until explicitly stopped via {@link #manualStop()}. A
         * subsequent call to {@link #manualStart(ChannelCue[])} can be used to immediately have a new
         * cue take into effect.
         *
         * @param cues
         *            An array of channel cues to execute. See the description of the same argument in
         *            {@link #push(ChannelCue[], int)} for details.
         * @throws ConnectionLostException
         *             Connection to the IOIO was lost before or during this operation.
         */
         void manualStart(ISequencerChannelCue[] cues);

        /**
         * Stop a manual cue currently running.
         * <p>
         * This may be called only when a the sequencer is not in the Running state, typically in the
         * Manual state, as result of a previous {@link #manualStart(ChannelCue[])}. This causes the
         * execution to stop immediately and the sequencer is now back in paused mode, ready for another
         * manual cue or for resuming execution of its previously queued sequence. Calling while in the
         * Idle state is legal, but does nothing.
         *
         * @throws ConnectionLostException
         *             Connection to the IOIO was lost before or during this operation.
         */
         void manualStop();

        /**
         * Start execution of the sequence.
         * <p>
         * This method will cause any previously queued cues (via {@link #push(ChannelCue[], int)}) to
         * start execution in order of pushing, according to their specified timings. The sequencer must
         * be paused before calling this method.
         *
         * @throws ConnectionLostException
         *             Connection to the IOIO was lost before or during this operation.
         */
         void start();

        /**
         * Pause execution of the sequence.
         * <p>
         * This method can be called when the sequencer is running, as result of previous invocation of
         * {@link #start()}. It will cause execution to suspend as soon as the currently executing cue
         * is done. The queue of pending cues will not be affected, and operation can be resumed
         * seamlessly by calling {@link #start()} again.
         *
         * @throws ConnectionLostException
         *             Connection to the IOIO was lost before or during this operation.
         */
         void pause();

        /**
         * Stop execution of the sequence.
         * <p>
         * This will cause the sequence execution to stop immediately (without waiting for the current
         * cue to complete). All previously queued cues will be discarded. The sequence will then go to
         * paused state.
         *
         * @throws ConnectionLostException
         *             Connection to the IOIO was lost before or during this operation.
         */
         void stop();

        /**
         * Get the number of cues which can be pushed without blocking.
         * <p>
         * This is useful for pre-filling the queue before starting the sequencer, or if the client
         * wants to do other operations on the same thread that pushes cues. The value returned will
         * indicate how many calls to {@link #push(ChannelCue[], int)} can be completed without
         * blocking.
         *
         * @return The number of available slots in the cue FIFO.
         */
         int available();

        /**
         * Get the most recent execution event.
         * <p>
         * This includes the event type and the number of cues that started executing, since opening the
         * sequencer or the last call to {@link #stop()}. Immediately after opening the sequencer, the
         * event type may be {@link Event.Type.CLOSED}, and as soon as the sequencer finished opening an
         * {@link Event.Type.STOPPED} will be sent.
         *
         * @return The last event.
         *
         * @throws ConnectionLostException
         *             Connection to the IOIO was lost before or during this operation.
         */
         SequencerEvent getLastEvent();

        /**
         * Waits until an execution event occurs and returns it.
         * <p>
         * In case the client is not reading fast enough, older events will be discarded as new once
         * arrive, so that the queue always stores the most recent events.
         *
         * @throws ConnectionLostException
         *             Connection to the IOIO was lost before or during this operation.
         * @throws InterruptedException
         *             The operation was interrupted before completion.
         */
         SequencerEvent waitEvent();

        /**
         * A convenience method for blocking until an event of a certain type appears on the event
         * queue. All events proceeding this event type, including the event of the requested type will
         * be removed from the queue and discarded.
         *
         * @throws ConnectionLostException
         *             Connection to the IOIO was lost before or during this operation.
         * @throws InterruptedException
         *             The operation was interrupted before completion.
         */
         void waitEventType(SequencerEventType type);

        /**
         * Sets a new size for the incoming event queue.
         * <p>
         * Initially the size of the queue is 32, which should suffice for most purposes. If, however, a
         * client is not able to read frequently enough to not miss events, increasing the size is an
         * option.
         * <p>
         * Any pending events will be discarded. It is recommended to call this method only once,
         * immediately after opening the sequencer.
         *
         * @param size
         *            The new queue size.
         * @throws ConnectionLostException
         *             Connection to the IOIO was lost before or during this operation.
         */
         void setEventQueueSize(int size);
    }


}
