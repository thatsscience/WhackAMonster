using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhackAMonster
{
    class Arm
    {
        bool reaching = false;
        bool retreating = false;
        long elapsedTime = 0;
        int reaches = 0;
        int intervalGap = 0;
        int arm;
        Random rand = new Random();
        Program program;

        public Arm(Program p, int armNumber)
        {
            arm = armNumber;
            program = p;
            intervalGap = Interval();
        }

        public void Tick(long elapsed)
        {
            if (reaching || retreating) elapsedTime += elapsed;
            if (reaching && elapsedTime > 2000)
            {
                int penalty = intervalGap * rand.Next(1, ((reaches > 1) ? reaches : 2) / 2);
                program.SubtractFromScore(penalty);
                Console.CursorLeft = 20;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ouch! Arm number {0} got you!", arm + 1);
                Console.ForegroundColor = ConsoleColor.White;
                reaching = false;
                elapsedTime = 0;
                retreating = true;
            }

            if (retreating && elapsedTime > intervalGap)
            {
                elapsedTime = 0;
                retreating = false;
                intervalGap = Interval();
            }

        }

        private int Interval()
        {
            if (reaches < 5) return rand.Next(5000, 8000);
            if (reaches < 10) return rand.Next(3000, 5000);
            if (reaches < 15) return rand.Next(1000, 3000);
            return rand.Next(0, 1000); ;
        }

        public void Reach(int armNumber)
        {
            if (reaching || retreating) return;
            reaching = true;
            reaches++;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.CursorLeft = 0;
            Console.WriteLine("Arm {0} reaching", armNumber + 1);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public bool Click()
        {
            if (reaching)
            {
                reaching = false;
                retreating = true;
                elapsedTime = 0;
                program.AddToScore(10000 - intervalGap);
                intervalGap = Interval();
                return true;
            }
            return false;

        }


    }
}
