using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace DanceDance
{
    /* Класс предназначен непосредственно для проигрывания одного файла танца.
     * Для перехода от каждой позы к послеующей рассчитываются промежуточные позиции. */
    public class FilePlayer
    {
        List<SerialPort> serialPorts;
        byte[] response = new byte[32];
        int[][] current_poses;
        public String fileName;
        int currentFrameIndex = 1;
        public DancePrecounter DP;
        int roboCount;
        
        public FilePlayer(List<SerialPort> serialPorts_, String fileName_)
        {
            serialPorts = serialPorts_;
            fileName = fileName_;
            roboCount = serialPorts.Count;

            current_poses = new int[serialPorts.Count][];
        }

        public bool isDone()
        {
            return currentFrameIndex >= DP.frames.Count;
        }

        public void precount()
        {
            Matrix matrix = new Matrix();
            matrix.ParseHMotionBuilderFile(fileName);
            DP = new DancePrecounter(matrix);
            DP.precount();
        }

        public void setNextFrame()
        {
            for (int i = 0; i < roboCount; ++i)
                ServoPosesSend(i, DancePrecounter.HUNO_SERVO_COUNT, 4, DP.frames[currentFrameIndex].servo);
            ++currentFrameIndex;
        }

        public int getNextFrameTime()
        {
            return DP.frames[currentFrameIndex].time;
        }

        public int getFrameCount()
        {
            return DP.frames.Count;
        }

        /* Заполняем положение приводов робота значениями,
         * получаемыми с самого робота. */
        void ReadCurrentPose(int portNum, int servo_count)
        {
            servo_count = Math.Min(servo_count, DancePrecounter.MAX_SERVO_COUNT);
            current_poses[portNum] = new int[servo_count];
            for (int id = 0; id < servo_count; ++id)
                if (ServoReadPos(portNum, id))
                    current_poses[portNum][id] = (byte)((response[1] < 255) ? response[1] : 0);
        }

        /* Чтение текущего значения положения привода под номером id.
         * Для этого на робота отправляется команда, которая согласно
         * протоколу инициирует отправку необходимого значения. */ 
        bool ServoReadPos(int portNum, int id)
        {
            // Status read from wCK protocol definition
            byte[] buff = new byte[4];
            buff[0] = 0xFF; // header
            buff[1] = (byte)(5 << 5 | id); // mode - 5
            buff[2] = (byte)0; // произвольный
            buff[3] = (byte)((buff[1] ^ buff[2]) & 0x7f); // checksum

            try
            {
                serialPorts[portNum].Write(buff, 0, 4);
            }
            catch (Exception e1)
            {
                Trace.WriteLine("ServoReadPos writing: " + e1.Message + " " + serialPorts[portNum].PortName);
                return false;
            }
            try
            {
                response[0] = (byte)serialPorts[portNum].ReadByte();
                response[1] = (byte)serialPorts[portNum].ReadByte();
            }
            catch (Exception e1)
            {
                Trace.WriteLine("ServoReadPos reading from port: " + serialPorts[portNum].PortName + " " + e1.Message);
                return false;
            }
                
            return true;
        }

        /* Согласно протоколу формируем из массива позиций приводов пакет
         * и отправляем его на робота. */
        void ServoPosesSend(int portNum, int servoCount, int speedLevel, List<int> servoPoses)
        {
            // Syncronized Position Move from wCK protocol definition
            byte[] buff = new byte[5 + servoCount];
            buff[0] = 0xFF; // header
            buff[1] = (byte)((speedLevel << 5) | 0x1f); // speed [0, 4] | 31
            buff[2] = (byte)(servoCount + 1); // lastID + 1

            byte CheckSum = 0;
            int i = 0;
            for (; i < servoCount; ++i)
            {
                buff[3 + i] = (byte)servoPoses[i];
                CheckSum ^= (byte)servoPoses[i];
            }
            CheckSum = (byte)(CheckSum & 0x7f);
            buff[3 + i] = CheckSum;

            try
            {
                serialPorts[portNum].Write(buff, 0, buff.Length);
                return;
            }
            catch (Exception e1)
            {
                Trace.WriteLine("SyncPosSend: " + e1.Message + " " + serialPorts[portNum].PortName.ToString());
                return;
            }
        }

    }// class ServoControl
}// namespace DanceDance
