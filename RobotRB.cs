using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace DanceDance
{
    /* Класс, реализующий интерфейс общения с роботом.
     * RobotRoboBuilder */
    class RobotRB
    {
/*
*** Packet structure ***
+-----------------------------------------------------------------------------------------------------------------------------------------+
|                          Header (8)                   | Command Type (1)|Platform(1)|Command Size(4)|Command Contents (Size)|CheckSum(1)|
+-------------------------------------------------------|-----------------|-----------|---------------|-----------------------|-----------|
| 0xFF | 0xFF | 0xAA | 0x55 | 0xAA | 0x55 | 0x37 | 0xBA |                 |           |               |                       |           |
+-----------------------------------------------------------------------------------------------------------------------------------------+
*/ 
        public SerialPort serialPort;
        private byte[] header = new byte[] { 0xFF, 0xFF, 0xAA, 0x55, 0xAA, 0x55, 0x37, 0xBA };
        private byte[] response = new byte[32];
        public bool DCModeOn = false; // DC - Direct Control
        public string serialNumber, robotName;

        public void setPort(string portName)
        {
            serialPort.PortName = portName;
            serialPort.BaudRate = 115200;
            serialPort.WriteTimeout = 500;
            serialPort.ReadTimeout = 1500;
            serialPort.StopBits = StopBits.One;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
        }

        public SerialPort getPort()
        {
            return serialPort;
        }

        public bool openPort()
        {
            try
            {
                serialPort.Open();
            }
            catch(Exception e1)
            {
                Trace.WriteLine("OpenPort: " + e1.Message);
            }
            return serialPort.IsOpen;
        }

        public void closePort()
        {
            if (serialPort != null)
                if (serialPort.IsOpen)
                    serialPort.Close();
        }

        /* Отправка на робота команды. Для отправки необходимо отправить тип команды и собственно
         * команду. Процедура предназначена для отправки команды размером в 1 байт.
		 * Список возможных типов команд следует смотреть в документации. */
        private bool sendCommandToRB_1B(byte type, byte cmd)
        {
            try
            {
                // Пакет обязан начинаться с заголовка. См. структуру пакета в начале этого файла.
                serialPort.Write(header, 0, 8);
                serialPort.Write(new byte[] { 
                                type,                       // type (1)
                                0x00,                       // platform (1)
                                0x00, 0x00, 0x00, 0x01,     // command size (4)
                                cmd,                        // command contents (1)
                                cmd                         // checksum
                            }, 0, 8);

            }
            catch (Exception e1)
            {
                Trace.WriteLine("sendCommandToRB_1B: " + e1.Message);
            }
            return true;
        }

        /* 20 - тип команды, говорящий, что это базовое движение. */
        private void runMotion(byte motionNumber)
        {
            sendCommandToRB_1B(20, motionNumber);
        }

        /* 7 - номер базового движения - базовая стойка. */
        public void basicPosture()
        {
            runMotion(7);
        }

        /* Читаем пакет из серийного порта. Робот записывает пакет в порт в ответ на какую-либо команду.
         * Чтение производится побайтово. */
        private bool getResponseFromRB()
        {
            int b = 0;
            int l = 1;

            while (b < 32 && b < (15 + l))
            {
                try
                {
                    response[b] = (byte)serialPort.ReadByte();
                }
                catch (Exception e1)
                {
                    Trace.WriteLine("getResponse: " + e1.Message);
                    return false;
                }

                /* Если во время чтения заголовка, проскакивает байт, не принадлежащий заголовку,
                 * сбрасываем чтение. Пропускаем возможно тем самым пакет. */
                if ((b < header.Length) && (response[b] != header[b]))
                {
                    b = 0;
                    continue;
                }

                // Посчитаем длинну считываемой команды. Длина лежит в 10-13 байтах пакета. счет с 0.
                if (b == 13)
                    l = (response[b - 3] << 24) + (response[b - 2] << 16) +
                        (response[b - 1] << 8) + response[b];

                ++b;
            }

            return true;
        }

        /* Включение Direct Control Mode. Данная операция нужна для того,
         * чтобы роботом можно было управлять через Bluetooth. В данном режиме
         * нельзя управлять роботом с пульта. */
        public bool setDCModeOn()
        {
            if (serialPort.IsOpen)
            {
                int i = 0;
                while (true)
                {
                    // Отправляем на робота команду (0x10, 0x01), которая иннициирует DC.
                    sendCommandToRB_1B(0x10, 0x01);
                    // Считываем ответ, чтобы понять успешно прошло переключение или нет.
                    getResponseFromRB();
                    // Если переключение прошло - выходим из цикла, иначе повторяем.
                    if ((response[14] == 0x01) && (response[8] == 0x10))
                        break;
                    Trace.WriteLine("DC mode failed on port " + serialPort.PortName);
                    ++i;
                    if (i > 9)
                        return false;
                }
                DCModeOn = true;
                return true;
            }
            else
            {
                DCModeOn = false;
                Trace.WriteLine("DC mode failed due to serial port is closed.");
                return false;
            }
        }

        /* Выключение Direct Control Mode. */
        public void setDCModeOff()
        {
            try
            {
                DCModeOn = false;
                if (serialPort.IsOpen)
                    serialPort.Write(new byte[] { 0xFF, 0xE0, 0xFB, 0x1, 0x00, 0x1A }, 0, 6);
            }
            catch (Exception e1)
            {
                Trace.WriteLine("RobotRB: SetDCModeOff: " + e1.Message);
            }
        }

        /* Для удобства работы с роботами, после подключения к роботу можно запросить
         * его серийный номер, и связать его с именем, которое хранится в файле. */
        public string readSerialNumber()
        {
            serialNumber = "";
            if (serialPort.IsOpen)
            {
                sendCommandToRB_1B(0x0C, 0x01);
                if (getResponseFromRB())
                    for (int n0 = 0; n0 < 13; ++n0)
                        serialNumber += Convert.ToString((char)response[14 + n0]);
            }
            robotName = "Unknown";
            if (File.Exists("serialNumbers.txt"))
            {
                StreamReader sr = new StreamReader("serialNumbers.txt");
                string temp = "";
                while ((temp = sr.ReadLine()) != null)
                {
                    temp.Trim();
                    if (temp[0] == ';') // comment
                        continue;
                    string[] str = temp.Split(' ');
                    if (serialNumber.CompareTo(str[0]) == 0)
                        robotName = str[1];
                }
            }

            return serialNumber;
        }

        public string getName()
        {
            return robotName;
        }

        public RobotRB() 
        {
            serialPort = new SerialPort();
        }

    }
}
