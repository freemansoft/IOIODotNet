using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOIOLib.Component
{
    public interface IPulseInput
    {
        /**
         * Gets the pulse duration in case of pulse measurement mode, or the period in case of frequency
         * mode. When scaling is used, this is compensated for here, so the duration of a single cycle
         * will be returned.
         * <p>
         * The first call to this method may block shortly until the first data update arrives. The
         * client may interrupt the calling thread.
         *
         * @return The duration, in seconds.
         * @throws InterruptedException
         *             The calling thread has been interrupted.
         * @throws ConnectionLostException
         *             The connection with the IOIO has been lost.
         */
        float getDuration();

        /**
         * This is very similar to {@link #getDuration()}, but will wait for a new sample to arrive
         * before returning. This is useful in conjunction with {@link IOIO#sync()}, in cases when we
         * want to guarantee the we are looking at a sample that has been captured strictly after
         * certain other commands have been executed.
         *
         * @return The duration, in seconds.
         * @throws InterruptedException
         *             The calling thread has been interrupted.
         * @throws ConnectionLostException
         *             The connection with the IOIO is lost.
         * @see #getDuration()
         */
        float getDurationSync();

        /**
         * Reads a single measurement from the queue. If the queue is empty, will block until more data
         * arrives. The calling thread may be interrupted in order to abort the call. See interface
         * documentation for further explanation regarding the read queue.
         * <p>
         * This method may not be used if the interface has was opened in frequency mode.
         *
         * @return The duration, in seconds.
         * @throws InterruptedException
         *             The calling thread has been interrupted.
         * @throws ConnectionLostException
         *             The connection with the IOIO has been lost.
         */
        float getDurationBuffered();

        /**
         * @deprecated Please use {@link #getDurationBuffered()} instead.
         */
        float waitPulseGetDuration();

        /**
         * Gets the momentary frequency of the measured signal. When scaling is used, this is
         * compensated for here, so the true frequency of the signal will be returned.
         * <p>
         * The first call to this method may block shortly until the first data update arrives. The
         * client may interrupt the calling thread. - *
         * <p>
         * This method may only be used if the interface has been opened in frequency mode.
         *
         * @return The frequency, in Hz.
         * @throws InterruptedException
         *             The calling thread has been interrupted.
         * @throws ConnectionLostException
         *             The connection with the IOIO has been lost.
         */
        float getFrequency();

        /**
         * This is very similar to {@link #getFrequency()}, but will wait for a new sample to arrive
         * before returning. This is useful in conjunction with {@link IOIO#sync()}, in cases when we
         * want to guarantee the we are looking at a sample that has been captured strictly after
         * certain other commands have been executed.
         *
         * @return The frequency, in Hz.
         * @throws InterruptedException
         *             The calling thread has been interrupted.
         * @throws ConnectionLostException
         *             The connection with the IOIO is lost.
         * @see #getFrequency()
         */
        float getFrequencySync();
    }
}
