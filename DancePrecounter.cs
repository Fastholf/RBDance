using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanceDance
{
    /* Класс предназначен для превращения набора позиций и фреймов в последовательность
     * кадров и временных промежутков между ними. Это позволяет проще отслеживать
     * синхронищацию между файлами. При разработке класса в качестве предпосылки используется факт,
     * что перед началом каждого танца роботы находятся в базовой позиции. */
    public class DancePrecounter
    {
        public const int MAX_SERVO_COUNT = 21;
        public const int HUNO_SERVO_COUNT = 16;

        public class Pose
        {
            public List<int> servo; // позиции приводов для текущей позиции
            public int time; // время, которое надо подождать до установления текущей позиции от начала танца
            public Pose()
            {
                time = 0;
            }
            public Pose(List<int> servo_, int timeToSleep_)
            {
                servo = servo_;
                time = timeToSleep_;
            }
        }
        public List<Pose> frames = new List<Pose>();

        /* Верхний и нижние пределы положений приводов для модели Huno.
         * ub - up bound, lb - low bound. Используются для предотвращения перекручивания
         * приводов, в случае ошибочной записи в файле. */
        static public List<int> ub_Huno = new List<int>(new int[] {
        /* ID
          0,   1,   2,   3,   4,   5,   6,   7,   8,   9,  10,  11,  12,  13,  14,  15 */
        174, 228, 254, 130, 185, 254, 180, 126, 208, 208, 254, 224, 198, 254, 228, 254});

        static public List<int> lb_Huno = new List<int>(new int[] {
        /* ID
          0,  1,   2,  3,  4,  5,  6,  7,   8,  9, 10, 11, 12, 13, 14, 15 */
          1, 70, 124, 40, 41, 73, 22,  1, 120, 57,  1, 23,  1,  1, 25, 40});

        public List<int> basicPosition_Huno = new List<int>(new int[] {
        /* ID
          0,   1,   2,  3,   4,   5,  6,  7,   8,   9, 10, 11, 12,  13,  14,  15 */
        125, 179, 199, 88, 108, 126, 72, 49, 163, 141, 51, 47, 49, 199, 205, 205});

        private Matrix m;

        public DancePrecounter(Matrix matrix_)
        {
            m = matrix_;
        }

        /* Проверяем не находится ли позиция двигателя за допустимыми пределами.
         * Если находится то заменяем ее на предельно допустимую. */
        static public List<int> checkHunoBounds(List<int> servo_poses)
        {
            for (int i = 0; i < HUNO_SERVO_COUNT; ++i)
                servo_poses[i] = (byte)Math.Min(Math.Max(servo_poses[i], lb_Huno[i]), ub_Huno[i]);

            return servo_poses;
        }

        public void precount()
        {
            /* Считаем, что изначально робот находится в базовой позиции.
             * Если впредь это условие именится, то в данной функции придется
             * вводить управление каждым роботом отдельно, либо обрабатывать старт
             * танца отдельно. */
            frames.Add(new Pose(basicPosition_Huno, 0));
            List<int> currentPos = basicPosition_Huno;
            for (int i = 0; i < m.durations.Count; ++i)
            {
                m.servo_poses[i] = checkHunoBounds(m.servo_poses[i]);
                
                int timeToSleep = Math.Max(m.durations[i] / m.step_counts[i], 25);
                for (int j = 1; j <= m.step_counts[i]; ++j)
                {
                    List<int> temp = new List<int>(new int[HUNO_SERVO_COUNT]);
                    for (int k = 0; k < HUNO_SERVO_COUNT; ++k)
                        temp[k] = (int)GetMoveValue(currentPos[k], m.servo_poses[i][k], j, m.step_counts[i]);
                    frames.Add(new Pose(temp, frames[frames.Count - 1].time + timeToSleep));
                }
                currentPos = m.servo_poses[i];
            }
        }

        /* Определение следующей позиции привода, рассчитанной по линейной
         * интерполяции. */
        static public double GetMoveValue(int startPos, int endPos, int num, int count)
        {
            int distance = endPos - startPos;
            double step = (double)distance / (double)count;
            return startPos + step * num;
        }

        public int getFullTime()
        {
            return frames.Last().time;
        }
    }
}
