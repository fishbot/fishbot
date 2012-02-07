using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Timers;
using OpenCvSharp;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace WindowsFormsApplication1
{

    public partial class MainForm : Form
    {
        //int std_rect = 20;
        //Color most_color;
        //Point StartPoint;
        //Rectangle bounds;
       // Dictionary<Color, int> color_dict = new Dictionary<Color, int>();
        //int iMostColor;

        int iNumTries = 0;
        int iNumSuccess = 0;
        private volatile bool _shouldStop = false;
        Random rand = new Random();

        public FrmLog frmLog = new FrmLog();

        System.Timers.Timer aTimer = new System.Timers.Timer();
        System.Timers.Timer lurkTimer = new System.Timers.Timer();
        System.Timers.Timer rumsyTimer = new System.Timers.Timer();

        private bool active = false;
        private bool on_cursor = false;
        private bool on_bite = false;
        private bool bend = true;
        private bool bLurk = false;
        private bool bRumsy = false;

        Thread oThread;

        byte[] cursor_ethalon = Convert.FromBase64String("w5KXJzhYFpOq+34hF/ZvgA==");

        private readonly object _locker = new object();

        public void ToggleFishing()
        {
            StartFishingLoop();   
        }

        private void FishingThread()
        {
            lock (_locker)
            {
                while ( !_shouldStop )
                {
                    Monitor.Wait(_locker);

                    if (_shouldStop)
                        break;

                    Thread.Sleep(3000);

                    FishingLoop();
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();
            oThread = new Thread(new ThreadStart(this.FishingThread));
            oThread.Start();

            frmLog.Show();

            txtSens.Text = Convert.ToString(trackSens.Value);

            CheckForIllegalCrossThreadCalls = false;
        }

        private void OnFishingTimerFinishEvent(object source, ElapsedEventArgs e)
        {
            on_cursor = false;
            on_bite = false;
            aTimer.Enabled = false;
        }

        private bool TestCursor()
        {
            int iTmpI = 0, iTmpJ = 0;

            Bitmap cursor = ScreenShot.CaptureCursor(ref iTmpI, ref iTmpJ);

            if (cursor == null)
                return false;


            using (MD5CryptoServiceProvider md5c = new MD5CryptoServiceProvider())
            {
                byte[] md5 = md5c.ComputeHash(ScreenShot.imageToByteArray(cursor));

                if (md5.SequenceEqual(this.cursor_ethalon))
                {
                    cursor.Dispose();
                    return true;
                }
            }

            cursor.Dispose();
            return false;
        }

        private void OnLurkEndTimerEvent(object source, ElapsedEventArgs e)
        {
            bLurk = false;
            lurkTimer.Enabled = false;
        }

        private void OnRumsyEndTimerEvent(object source, ElapsedEventArgs e)
        {
            bRumsy = false;
            rumsyTimer.Enabled = false;
        }

        private bool StartFishing()
        {
            iNumTries++;

            if (!bRumsy)
            {
                SendKeys.SendWait("0");

                rumsyTimer.AutoReset = true;
                rumsyTimer.Interval = 3 * 60 * 1000; // 3 minutes rumsy label
                rumsyTimer.Elapsed += new ElapsedEventHandler(OnRumsyEndTimerEvent);
                rumsyTimer.Enabled = true;

                bRumsy = true;
            }

            if (!bLurk)
            {
                SendKeys.SendWait(this.txtLurk.Text);

                lurkTimer.AutoReset = true;
                lurkTimer.Interval = 10*60*1000; // 10 minutes best lurk
                lurkTimer.Elapsed += new ElapsedEventHandler(OnLurkEndTimerEvent);
                lurkTimer.Enabled = true;

                bLurk = true;

                Thread.Sleep(2500);
            }

            Thread.Sleep(1000);

            Bitmap b4Fishing = ScreenShot.CaptureImage(new Point(this.Left, this.Top), new Point(0,0), new Rectangle(this.Top, this.Left, this.Width, this.Height));

            SendKeys.SendWait(this.txtKey.Text);

            Thread.Sleep(400);

            aTimer.AutoReset = true;
            aTimer.Interval = 20000;
            aTimer.Elapsed += new ElapsedEventHandler(OnFishingTimerFinishEvent);
            aTimer.Enabled = true;

            Thread.Sleep(2000);

            Bitmap onFishing = ScreenShot.CaptureImage(new Point(this.Left, this.Top), new Point(0, 0), new Rectangle(this.Top, this.Left, this.Width, this.Height));

            IplImage i1 = FeatureDetector(b4Fishing);
            IplImage i2 = FeatureDetector(onFishing);

            IplImage diff = new IplImage(new OpenCvSharp.CvSize(i1.Size.Width, i1.Size.Height), BitDepth.U8, 1);

            IplImage normalized = new IplImage(new OpenCvSharp.CvSize(diff.Size.Width, diff.Size.Height), BitDepth.U8, 1);

            OpenCvSharp.Cv.AbsDiff((CvArr)i2, (CvArr)i1, (CvArr)diff);
            //OpenCvSharp.Cv.CvtColor(diff, gray, ColorConversion.RgbToGray);
            OpenCvSharp.Cv.Normalize(diff, normalized, 0.0, 255.0, NormType.MinMax);

            bool bEnd = false;
            bool bFound = false;
            on_cursor = true;

            for (int i = 0; i < normalized.Height; ++i)
            {
                if (!on_cursor)
                    break;

                for (int j = 0; j < normalized.Width; ++j)
                {
                    if (!on_cursor)
                        break;

                    CvScalar color = normalized.Get2D(i, j);

                    if (color.Val0 > 50)
                    {
                        Cursor.Position = new Point(this.Left + j, this.Top + i);
                        //Thread.Sleep(100);

                        if (TestCursor())
                        {

                            bEnd = true;
                            bFound = true;
                            break;
                        }
                    }
                }
                if (bEnd)
                    break;
            }

            on_cursor = false;

            Bitmap bDiff = BitmapConverter.ToBitmap(i2);

            //b4Fishing.Save("C:\\cursors\\b4fish.jpg", ImageFormat.Jpeg);
            //onFishing.Save("C:\\cursors\\onfish.jpg", ImageFormat.Jpeg);
            //bDiff.Save("C:\\cursors\\diff.jpg", ImageFormat.Jpeg);

            return bFound;
        }

        public void StartFishingLoop()
        {
            if (!active)
            {
                frmLog.AddLine("Fishing is now ON\n");

                lock (_locker)
                {
                    active = true;
                    Monitor.Pulse(_locker);
                }
            }
            else
            {
                frmLog.AddLine("Fishing is now OFF\n");
                active = false;
            }
        }

        private void FishingLoop()
        {
            active = true;
            bend = false;

            while (true)
            {
                if (!active)
                    return;

                if (StartFishing())
                    WaitForBite();

                if (bend)
                {
                    bend = false;
                    break;
                }
            }
        }

        private IplImage FeatureDetector(Bitmap bitmapIn)
        {
            double minVal, maxVal;
            IplImage i1 = BitmapConverter.ToIplImage(bitmapIn);

            IplImage gray = new IplImage(new OpenCvSharp.CvSize(i1.Size.Width, i1.Size.Height), BitDepth.U8, 1);
            IplImage blur = new IplImage(new OpenCvSharp.CvSize(i1.Size.Width, i1.Size.Height), BitDepth.U8, 1);
            IplImage gaussian = new IplImage(new OpenCvSharp.CvSize(i1.Size.Width, i1.Size.Height), BitDepth.U8, 1);
            IplImage laplace = new IplImage(new OpenCvSharp.CvSize(i1.Size.Width, i1.Size.Height), BitDepth.F32, gaussian.NChannels);
            IplImage converted = new IplImage(new OpenCvSharp.CvSize(i1.Size.Width, i1.Size.Height), BitDepth.U8, 1);
            IplImage normalized = new IplImage(new OpenCvSharp.CvSize(i1.Size.Width, i1.Size.Height), BitDepth.U8, 1);
            IplImage tresh = new IplImage(new OpenCvSharp.CvSize(i1.Size.Width, i1.Size.Height), BitDepth.U8, 1);

            OpenCvSharp.Cv.CvtColor(i1, gray, ColorConversion.RgbToGray);
            OpenCvSharp.Cv.Normalize(gray, normalized, 0.0, 255.0, NormType.MinMax);
            OpenCvSharp.Cv.Smooth(normalized, blur, SmoothType.Median, 5, 5);
            OpenCvSharp.Cv.Smooth(blur, gaussian, SmoothType.Gaussian, 5, 5);
            OpenCvSharp.Cv.Laplace(gaussian, laplace, ApertureSize.Size5);

            laplace.MinMaxLoc(out minVal, out maxVal);
            laplace.ConvertScale(converted, 255.0 / (2.0 * maxVal), 128);

            OpenCvSharp.Cv.Threshold(converted, tresh, 150.0, 255.0, ThresholdType.Binary);

            return tresh;
        }

        private List<double> MakeNormal(List<double> values)
        {
            List<double> valuesOut = new List<double>();

            if (values.Count < 1)
            {
                return valuesOut;
            }

            for (int i = 0; i < values.Count - 1; ++i)
            {
                double sqr = Math.Sqrt(-2.0*Math.Log(values[i], Math.E));

                double z0 = Math.Cos(2*Math.PI*values[i+1])*sqr;
                double z1 = Math.Sin(2*Math.PI*values[i+1])*sqr;

                valuesOut.Add(z0);
                valuesOut.Add(z1);
            }

            return valuesOut;
        }

        private double LastFromNormal(List<double> valuesIn)
        {
            if (valuesIn.Count == 0)
                return 0.0;

            List<double> values = MakeNormal(valuesIn);

            if (values.Count < 2)
            {
                return 0.0;
            }

            double sqr = Math.Sqrt(-2.0 * Math.Log(values[values.Count-2], Math.E));

            //double z0 = Math.Cos(2 * Math.PI * values[values.Count-1]) * sqr;
            return Math.Sin(2 * Math.PI * values[values.Count-1]) * sqr;
        }

        private double CalcDispersion(List<double> values, out double dMO)
        {
            double dSum = 0;
            double dSumSqare = 0;

            dMO = 0;

            if (values.Count == 0)
                return 0.0;

            for (int i = 0; i < values.Count; ++i)
            {
                dSum += values[i];
            }

            dMO = dSum / (double)values.Count;

            for (int i = 0; i < values.Count; ++i)
            {
                dSumSqare += (dMO - values[i]) * (dMO - values[i]);
            }

            return dSumSqare / (double)values.Count;
        }

        private void WaitForBite()
        {
            int iCur = 0;

            int iPrevCount = 0;
            int iCurCount = 0;

            if (!TestCursor())
            {
                return;
            }

            Bitmap prev = ScreenShot.CaptureImage(new Point(Cursor.Position.X - 32, Cursor.Position.Y - 32), new Point(0, 0), new Rectangle(Cursor.Position.X - 32, Cursor.Position.Y - 32, 64, 64));
            IplImage i1 = FeatureDetector(prev);

            Bitmap bPrev = BitmapConverter.ToBitmap(i1);

            //bPrev.Save("C:\\cursors\\prev.jpg", ImageFormat.Jpeg);

            on_bite = true;

            double dSens = Convert.ToDouble(this.txtSens.Text)/10;
            double prevNorm = 0.0;
            double curNorm = 0.0;

            double dMaxDF = 0.0;
            double dSuccessRate = 0.0;

            List<double> normValues = new List<double>();

            frmLog.LogClear();

            int iSleepTime = 50;

            while (true)
            {
                if (!on_bite)
                    break;

                Thread.Sleep(iSleepTime);

                iPrevCount = iCurCount;

                prevNorm = curNorm;

                if (iCur > 1)
                {
                    Bitmap zone = ScreenShot.CaptureImage(new Point(Cursor.Position.X - 32, Cursor.Position.Y - 32), new Point(0, 0), new Rectangle(Cursor.Position.X - 32, Cursor.Position.Y - 32, 64, 64));
                    IplImage i2 = FeatureDetector(zone);

                    curNorm = OpenCvSharp.Cv.Norm(i1, i2, NormType.L2);

                    if (iCur > (1000 / iSleepTime)*2)
                    {
                        double dMO = 0.0;

                        double dSqareDeviation = Math.Sqrt(CalcDispersion(normValues, out dMO)); // Std deviation

                        frmLog.AddLine("dSqD: " + dSqareDeviation.ToString("0.00000") + " v:" + curNorm.ToString("0.00000") + ";\n");

                        //frmLog.AddLine("DF: " + Convert.ToString(dSqareDeviation) + "\n");

                        //double normDiff = (double)Math.Abs(dSqareDeviation - curNorm);

                        /* CvMoments moments = new CvMoments();
                        CvHuMoments hu_moments = new CvHuMoments();
                        OpenCvSharp.Cv.Moments(tresh, out moments, true);
                        OpenCvSharp.Cv.GetHuMoments(moments, out hu_moments); */

                        if (dSqareDeviation > dMaxDF)
                        {
                            dMaxDF = dSqareDeviation;
                        }

                        double dMOpS = dMO + dSqareDeviation * dSens;
                        double dMOmS = dMO - dSqareDeviation;

                        if (curNorm > dMOpS)
                        {
                            iNumSuccess++;

                            Thread.Sleep(1000);

                            MouseEv.MouseRightClick(Cursor.Position.X, Cursor.Position.Y);

                            Bitmap bKlev = BitmapConverter.ToBitmap(i2);

                           // bKlev.Save("C:\\cursors\\klev.jpg", ImageFormat.Jpeg);

                            break;
                        }

                    }

                    normValues.Add(curNorm);
                }

                iCur++;
            }


            if (iNumTries > 0)
            {
                dSuccessRate = (double)iNumSuccess * 100 / (double)iNumTries;
            }

            frmLog.AddLine("\n\nDF: " + dMaxDF.ToString("0.0") + " Rate: " + dSuccessRate.ToString("0.00") + "%\n");

            aTimer.Enabled = false;
            //active = false;
            on_bite = false;

            Thread.Sleep(1000);
            if (rand.Next(100) > 90)
            {
                SendKeys.SendWait(" ");
            }
            Thread.Sleep(1000);
        }

        private void ScanForFishingCusrsor()
        {
            bool bEnd = false;

            while (true)
            {
                for (int i = this.Top + 20; i < this.Top + this.Size.Height; i += 20)
                {
                    for (int j = this.Left; j < this.Left + this.Size.Width; j += 10)
                    {
                        if (!active)
                            break;

                        int iTmpI = 0;
                        int iTmpJ = 0;


                        Cursor.Position = new Point(j, i);
                        Thread.Sleep(50);

                        Bitmap cursor = ScreenShot.CaptureCursor(ref iTmpI, ref iTmpJ);

                        if (cursor == null)
                            continue;

                        //cursor.Save("C:\\cursors\\cur" + Convert.ToString(iCur) + ".jpg", ImageFormat.Jpeg);

                        
                        using(MD5CryptoServiceProvider md5c = new MD5CryptoServiceProvider())
                        {
                            byte[] md5 = md5c.ComputeHash(ScreenShot.imageToByteArray(cursor));

                            if (md5.SequenceEqual(this.cursor_ethalon))
                            {
                                bEnd = true;
                                cursor.Dispose();
                                break;
                            }
                        }

                        cursor.Dispose();
                    }

                    if (!active)
                        break;

                    if (bEnd)
                        break;
                }

                if (!active)
                    break;

                if (bEnd)
                    break;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            StartFishingLoop();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            oThread.Join(0);
            _shouldStop = true;
            lock (_locker)
            {
                Monitor.Pulse(_locker);
            }
        }

        private void trackSens_ValueChanged(object sender, EventArgs e)
        {
            DisplaySens();
        }

        public void DisplaySens()
        {
            txtSens.Text = Convert.ToString(trackSens.Value);
            frmLog.AddLine("Sensivity set to " + Convert.ToString(trackSens.Value) + "\n");
        }

        public void IncreaseSens()
        {
            trackSens.Value++;
            DisplaySens();
        }

        public void DescreaseSens()
        {
            trackSens.Value--;
            DisplaySens();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
