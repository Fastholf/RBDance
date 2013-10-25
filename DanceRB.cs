using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Diagnostics;
using System.Windows.Forms;

namespace DanceDance
{
    public class DanceRB
    {

        /* Структура данных, описывающая один файл танца. Связывает имя файла
         * с номерами роботов, которые будут его исполнять. */ 
        public class ControlFile
        {
            public string fileName;
            public List<int> robotNums;
            public ControlFile()
            {
                fileName = "";
                robotNums = new List<int>();
            }
        }
        List<ControlFile> controlFiles;
        List<SerialPort> SPList;
        List<FilePlayer> filePlayers;
        bool done, paused;
        public int longestFile = -1;
        public Label frames_label;
        public TrackBar frames_trackBar;

        public DanceRB(List<ControlFile> controlFiles_, List<SerialPort> SPList_) 
        {
            controlFiles = controlFiles_;
            SPList = SPList_;
            filePlayers = new List<FilePlayer>();
            for (int i = 0; i < controlFiles.Count; ++i)
            {
                List<SerialPort> temp_SPList = new List<SerialPort>();
                for (int j = 0; j < controlFiles[i].robotNums.Count; ++j)
                    temp_SPList.Add(SPList[controlFiles[i].robotNums[j]]);

                filePlayers.Add(new FilePlayer(temp_SPList, controlFiles[i].fileName));
            }
            done = true;
            paused = false;
        }
        ~DanceRB() { }
        static List<FilePlayer> servoControl = new List<FilePlayer>();

        public void setFramesLabel(Label frames_label_)
        {
            frames_label = frames_label_;
        }

        public void setFramesTrackBar(TrackBar frames_trackBar_)
        {
            frames_trackBar = frames_trackBar_;
        }

        public bool isDanceRunning()
        {
            return !done;
        }

        public void Abort()
        {
            done = true;
        }

        public void Pause()
        {
            paused = true;
        }

        public void Resume()
        {
            paused = false;
        }

        public void PrepareDance()
        {
            int maxTime = -1;
            for (int i = 0; i < filePlayers.Count; ++i)
            {
                filePlayers[i].precount();
                if (maxTime < filePlayers[i].getTimeLength())
                {
                    maxTime = filePlayers[i].getTimeLength();
                    longestFile = i;
                }
            }
        }

        public int GetDanceLength()
        {
            return filePlayers[longestFile].getFrameCount();
        }

        public void SetCurrentFrame(int cFrame)
        {
            frames_label.Invoke((MethodInvoker)(() => frames_label.Text = cFrame.ToString() + "/" +
                filePlayers[longestFile].getFrameCount().ToString()));
            if (cFrame <= frames_trackBar.Maximum)
            {
                frames_trackBar.Invoke((MethodInvoker)(() => frames_trackBar.Value = cFrame));
            }
            
        }

        public void SetToFrame(int frameNum)
        {
            int time = filePlayers[longestFile].DP.frames[frameNum].time;
            filePlayers[longestFile].setFrame(frameNum);
            Trace.Write(time.ToString() + " _ ");
            for (int i = 0; i < filePlayers.Count; ++i)
            {
                if (i != longestFile)
                {
                    int j = 0;
                    while (filePlayers[i].DP.frames.Count > j && filePlayers[i].DP.frames[j].time < time) ++j;
                    filePlayers[i].setFrame(j);
                    Trace.Write(filePlayers[i].DP.frames[j].time.ToString() + " _ ");
                }
            }
            Trace.WriteLine("");
        }

        public void LetsDance()
        {
            done = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (!done)
            {
                int closestFrameTime = 1000000000;
                bool working = false;
                for (int i = 0; i < filePlayers.Count; ++i)
                {
                    if (!filePlayers[i].isDone())
                    {
                        closestFrameTime = Math.Min(filePlayers[i].getNextFrameTime(), closestFrameTime);
                        working = true;
                    }
                }
                if (!working)
                {
                    done = true;
                    break;
                }
                for (int i = 0; i < filePlayers.Count; ++i)
                {
                    if (!filePlayers[i].isDone())
                    {
                        if (filePlayers[i].getNextFrameTime() == closestFrameTime)
                        {
                            filePlayers[i].setNextFrame();
                            if (i == longestFile)
                            {
                                SetCurrentFrame(filePlayers[i].getCurrentFrame());
                            }
                        }
                    }
                }
                while (stopwatch.ElapsedMilliseconds < closestFrameTime) Thread.Sleep(1);

                if (paused)
                {
                    stopwatch.Stop();
                    while (paused) Thread.Sleep(1);
                    stopwatch.Start();
                }
            }
            done = true;
        }
    } // class DanceRB
} // namespace DanceDance
