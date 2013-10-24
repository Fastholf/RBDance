using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DanceDance
{
    /* Класс используется для получения танца из файла *.h Motion Builder 
     * и его хранения в удобном для использования виде. 
     * Что такое танец. Робот состоит из 16 двигателей. Положение 
     * каждого двигателя описывается одним числом, укзывающим 
     * насколько двигатель повернут. Танец это последовательность положений
     * двигателей - поз. Таким образом поза представляется набором из
     * 16 чисел. От позы к позе робот переходит не сразу, а за указанное время
     * и за определенное количество фреймов - промежуточных поз.
     * Фреймы можно рассчитывать по разному, в данной программе
     * используется линейная интерполяция. */
    public class Matrix
    {
        public List<List<int>> servo_poses = new List<List<int>>();
        public List<int> durations;
        public List<int> step_counts;

        public Matrix() { }

        /* Используется когда нужно пропустить в файле N вхождений заданного символа. */
        private int skipNsymbols(StreamReader sr, char symbol, int n)
        {
            int symbolToSkipFound = 0;
            while (symbolToSkipFound < n)
            {
                if (sr.Peek() == -1)
                    return 1;
                if (sr.Read() == symbol)
                    ++symbolToSkipFound;
            }
            return 0;
        }

        /* Удаление комментария из конца указанной строки.
         * Реализовано из предпосылки, что в коде символ '/' не встречается. */
        private string RemoveCommentFromString(string s)
        {
            int i = s.IndexOf('/');
            if (i != -1) // Если символ '/' был найден, то удалить все до конца строки, начиная с его позиции.
                s = s.Remove(i);
            return s;
        }

        /* Вытаскивание из строки списка чисел. Числа в строке разделены запятыми. */
        private List<int> ParseStringToListInt(string s)
        {
            List<int> res = new List<int>();

            s = Regex.Replace(s, "[^0-9,]", ""); // Удалить все, кроме цифр и запятых.
            string[] nums = s.Split(',');              

            for (int j = 0; j < nums.Length; ++j)
            {
                if (nums[j].CompareTo("") == 0)
                    continue;
                try
                {
                    res.Add(Int32.Parse(nums[j]));
                }
                catch (Exception e1)
                {
                    Trace.WriteLine("ParseStringToListInt: " + e1.Message + " !!! " + nums[j]);
                }
            }

            return res;
        }

        /* Парсим файл *.h, сгенерированный Motion Builder.
         * Процедура основывается на знании структуры файла и 
         * неизменности этой структуры.
         * return 0 in OK case */
        public int ParseHMotionBuilderFile(string fileName)
        {
            if (!File.Exists(fileName))
                return 1;
            StreamReader sr = new StreamReader(fileName);
            string ts, t = "";
            List<int> list;

            /* Читаем количества фреймов. Для этого сначала ищем 6 символов '{', 
             * пропускаем их. Затем переходим на другую строку. Далее, до тех пор
             * пока не встретится строка, содержащая только "};", читаем строки,
             * удаляя из них комментарии и складывая все в одну строку. После чего
             * парсим получившуюся строку. */
            if (skipNsymbols(sr, '{', 6) != 0)
                return 1;
            sr.ReadLine();
            while (true)
            {
                if (sr.Peek() == -1)
                    return 1;
                ts = sr.ReadLine();
                if (ts.CompareTo("};") == 0)
                    break;
                t += RemoveCommentFromString(ts);
                
            }
            step_counts = ParseStringToListInt(t);

            /* То же самое для продолжительностей поз. */
            if (skipNsymbols(sr, '{', 1) != 0)
                return 1;
            sr.ReadLine();
            t = "";
            while (true)
            {
                if (sr.Peek() == -1)
                    return 1;
                ts = sr.ReadLine();
                if (ts.CompareTo("};") == 0)
                    break;
                t += RemoveCommentFromString(ts);

            }
            durations = ParseStringToListInt(t);

            /* Чтение позиций немного отличается, тут мы парсим каждую считанную строку. */
            if (skipNsymbols(sr, '{', 1) != 0)
                return 1;
            for (int i = 0; i < 4; ++i)
                sr.ReadLine();
            int j = 0;
            while (true)
            {
                if (sr.Peek() == -1)
                    return 1;
                ts = sr.ReadLine();
                if (ts.CompareTo("};") == 0)
                    break;
                list = ParseStringToListInt(RemoveCommentFromString(ts));
                List<int> temp = new List<int>(new int[16]);
                servo_poses.Add(temp);
                for (int i = 0; i < list.Count; ++i)
                    servo_poses[j][i] = (byte)list[i];
                ++j;
            }

            sr.Close();
            return 0;
        }
    }
}
