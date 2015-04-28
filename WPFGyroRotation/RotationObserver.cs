using IOIOLib.Message;
using IOIOLib.MessageFrom;
using IOIOLib.Util;
using System;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace WPFGyroRotation
{
    internal class RotationObserver :  IObserver<II2cResultFrom>, IObserverIOIO
    {
        private static IOIOLog LOG = IOIOLogManager.GetLogger(typeof(RotationObserver));


        private TextBox RawFieldX_;
        private TextBox RawFieldY_;
        private TextBox RawFiedlZ;
        private TextBox AngleFieldX_;
        private TextBox AngleFieldY_;
        private TextBox AngleFieldZ_;
        private TextBox CallibFieldX_;
        private TextBox CallibFieldY_;
        private TextBox CallibFieldZ_;
        private float GyroDpsLsb_;
        private int PollingIntervalMsec_;
        private ModelVisual3D TargetModel_;


        public RotationObserver(float gyroDpsLsb, int msecInterval, 
            TextBox fieldAngleX, TextBox fieldAngleY, TextBox fieldAngleZ,
            TextBox fieldRawX, TextBox fieldRawY, TextBox fieldRawZ,
            TextBox fieldCallibX, TextBox fieldCallibY, TextBox fieldCallibZ,
            ModelVisual3D target
            )
        {
            this.GyroDpsLsb_ = gyroDpsLsb;
            this.PollingIntervalMsec_ = msecInterval;
            this.AngleFieldX_ = fieldAngleX;
            this.AngleFieldY_ = fieldAngleY;
            this.AngleFieldZ_ = fieldAngleZ;
            this.RawFieldX_ = fieldRawX;
            this.RawFieldY_ = fieldRawY;
            this.RawFiedlZ = fieldRawZ;
            this.CallibFieldX_ = fieldCallibX;
            this.CallibFieldY_ = fieldCallibY;
            this.CallibFieldZ_ = fieldCallibZ;
            this.TargetModel_ = target;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        // http://morf.lv/modules.php?name=tutorials&lasit=32
        private float LOW_PASS_VALUE = 0.3f;
        private Tuple<float, float, float> PreviousReadings = null;
        private Tuple<float, float, float> CurrentAngles_ = new Tuple<float, float, float>(0.0f, 0.0f, 0.0f);
        private Tuple<long, long, long> CalibrationTotals_ = new Tuple<long, long, long>(0, 0, 0);
        private Tuple<short, short, short> CalibrationValues_ = new Tuple<short, short, short>(0, 0, 0);
        private static int CallibrationSteps = 15;
        private int CallibrationStepCounter = CallibrationSteps+5;

        public void OnNext(II2cResultFrom value)
        {
            if (value.NumDataBytes > 0)
            {
                Tuple<short, short, short> result = ToXYZ(value);
                if (result != null)
                {
                    if (CallibrationStepCounter > CallibrationSteps)
                    {
                        // throw awaya couple samples
                        CallibrationStepCounter--;
                    }
                    else if (CallibrationStepCounter > 0)
                    {
                        // keep a running count of the total values
                        CalibrationTotals_ = new Tuple<long, long, long>(
                            CalibrationTotals_.Item1 + result.Item1,
                            CalibrationTotals_.Item2 + result.Item2,
                            CalibrationTotals_.Item3 + result.Item3);
                        CallibrationStepCounter --;
                    } else if (CallibrationStepCounter == 0) {
                        DisplayCallibrationValues();
                        CallibrationStepCounter--;
                    } else {
                        // calculate new angle movement taking into account offsets
                        float xValue = (result.Item1 - CalibrationValues_.Item1) * this.GyroDpsLsb_;
                        float yValue = (result.Item2 - CalibrationValues_.Item2) * this.GyroDpsLsb_;
                        float zValue = (result.Item3 - CalibrationValues_.Item3) * this.GyroDpsLsb_;
                        if (PreviousReadings == null)
                        {
                            PreviousReadings = new Tuple<float, float, float>(xValue, yValue, zValue);
                        }
                        // apply smothing - integration?
                        xValue = (float)(xValue * LOW_PASS_VALUE + (PreviousReadings.Item1 * (1.0 - LOW_PASS_VALUE)));
                        yValue = (float)(yValue * LOW_PASS_VALUE + (PreviousReadings.Item2 * (1.0 - LOW_PASS_VALUE)));
                        zValue = (float)(zValue * LOW_PASS_VALUE + (PreviousReadings.Item3 * (1.0 - LOW_PASS_VALUE)));
                        // update raw fields that are actually calculated fields
                        this.RawFieldX_.Dispatcher.BeginInvoke((Action)(() => this.RawFieldX_.Text = xValue.ToString()));
                        this.RawFieldY_.Dispatcher.BeginInvoke((Action)(() => this.RawFieldY_.Text = yValue.ToString()));
                        this.RawFiedlZ.Dispatcher.BeginInvoke((Action)(() => this.RawFiedlZ.Text = zValue.ToString()));

                        CurrentAngles_ = new Tuple<float, float, float>(
                            CurrentAngles_.Item1 + xValue,
                            CurrentAngles_.Item2 + yValue,
                            CurrentAngles_.Item3 + zValue
                        );
                        // update the calculated angle fields
                        this.AngleFieldX_.Dispatcher.BeginInvoke((Action)(() => this.AngleFieldX_.Text = CurrentAngles_.Item1.ToString()));
                        this.AngleFieldY_.Dispatcher.BeginInvoke((Action)(() => this.AngleFieldY_.Text = CurrentAngles_.Item2.ToString()));
                        this.AngleFieldZ_.Dispatcher.BeginInvoke((Action)(() => this.AngleFieldZ_.Text = CurrentAngles_.Item3.ToString()));

                        this.TargetModel_.Dispatcher.BeginInvoke((Action)(() => RotateObject(CurrentAngles_)));
                        PreviousReadings = new Tuple<float, float, float>(xValue, yValue, zValue);
                    }
                }
            }
            else
            {
                // we're confused so do nothing
            }
        }

        private void RotateObject(Tuple<float, float, float> newAngles)
        {
            Matrix3D matrix = CalculateRotationMatrix(newAngles.Item1, newAngles.Item2, newAngles.Item3);
            MatrixTransform3D transform = new MatrixTransform3D(matrix);
            TargetModel_.Transform = transform;
        }

        Matrix3D CalculateRotationMatrix(double x, double y, double z)
        {
            Matrix3D matrix = new Matrix3D();

            matrix.Rotate(new Quaternion(new Vector3D(1, 0, 0), x));
            matrix.Rotate(new Quaternion(new Vector3D(0, 1, 0) * matrix, y));
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1) * matrix, z));

            return matrix;
        }

        private void DisplayCallibrationValues()
        {
            // calculate the average
            CalibrationValues_ = new Tuple<short, short, short>(
                (short)(CalibrationTotals_.Item1 / CallibrationSteps),
                (short)(CalibrationTotals_.Item2 / CallibrationSteps),
                (short)(CalibrationTotals_.Item3 / CallibrationSteps)
                );
            this.CallibFieldX_.Dispatcher.BeginInvoke((Action)(() => this.CallibFieldX_.Text = CalibrationValues_.Item1.ToString()));
            this.CallibFieldY_.Dispatcher.BeginInvoke((Action)(() => this.CallibFieldY_.Text = CalibrationValues_.Item2.ToString()));
            this.CallibFieldZ_.Dispatcher.BeginInvoke((Action)(() => this.CallibFieldZ_.Text = CalibrationValues_.Item3.ToString()));
        }


        /// <summary>
        /// Ripped from TwiI2CTest
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Tuple<short, short, short> ToXYZ(II2cResultFrom value)
        {
            if (value == null || value.Data == null || value.NumDataBytes != 6)
            {
                return null;
            }
            else
            {
                short x = (short)((value.Data[1] << 8) + value.Data[0]);
                short y = (short)((value.Data[3] << 8) + value.Data[2]);
                short z = (short)((value.Data[5] << 8) + value.Data[4]);
                Tuple<short, short, short> result = new Tuple<short, short, short>(x, y, z);
                LOG.Debug("Values:" + result);
                return result;
            }
        }
    }
}