using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Diagnostics;

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
            for (int i = 0; i < filePlayers.Count; ++i)
                filePlayers[i].precount();
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
                    if (!filePlayers[i].isDone())
                    {
                        closestFrameTime = Math.Min(filePlayers[i].getNextFrameTime(), closestFrameTime);
                        working = true;
                    }
                if (!working)
                {
                    done = true;
                    break;
                }
                for (int i = 0; i < filePlayers.Count; ++i)
                    if (!filePlayers[i].isDone())
                        if (filePlayers[i].getNextFrameTime() == closestFrameTime)
                            filePlayers[i].setNextFrame();
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
