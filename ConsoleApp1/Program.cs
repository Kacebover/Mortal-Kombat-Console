using System;
using System.Threading;
using System.Diagnostics;

namespace MortalKombat
{
    class Program
    {
        public static int block1 = 0;
        public static int block2 = 0;
        public static double health1 = 1000;
        public static double health2 = 1000;
        public static int pause = 0;
        public static int ltime1 = 0;
        public static int ltime2 = 0;
        public static int fltime1 = 0;
        public static int fltime2 = 0;
        public static int sitting1 = 0;
        public static int sitting2 = 0;
        public static int aifighting1 = 0;
        public static int aifighting2 = 0;
        public static int place1 = 4;
        public static int place2 = 7;
        public static int time;
        public static int cdown;
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("                                                     MORTAL KOMBAT");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Управление :");
            Console.WriteLine("     Игрок 1: Удар - J, блок - O, чистый блок - ;, присесть/встать - S, вперед - D, назад - A, вкл/выкл бой против ИИ - P (англ)");
            Console.WriteLine("     Игрок 2: Удар - 4, блок - 5, чистый блок - 6, присесть/встать - ↓, вперёд - ←, назад - →, вкл/выкл бой против ИИ - 9");
            Console.WriteLine("     Спецприём Гарпун - назад, вперёд, удар1 (J или 4)");
            Console.WriteLine("     Спецприём Выстрел Из Арбалета - назад, вперёд, удар2 (I или 1)");
            Console.WriteLine("     Бой с ИИ - 9 или P (англ)");
            Console.WriteLine("     PageDown или Tab - пауза");
            Console.WriteLine("     Закончить игру - esc или end");
            Console.WriteLine("     Цвет игрока 1 - красный (соответственно, при его смерти будет красный цвет)");
            Console.WriteLine("     Цвет игрока 2 - синий (соответственно, при его смерти будет синий цвет)");
            Console.WriteLine("Правила :");
            Console.WriteLine("     1000 хп");
            Console.WriteLine("     Когда держишь блок, бить и передвигаться нельзя");
            Console.WriteLine("     Незаблокированный удар наносит 10 урона");
            Console.WriteLine("     Урон в блок 0.5хп");
            Console.WriteLine("     Зажим наказывается");
            Console.WriteLine("     Чистый блок дает бонус в 1 фрейм");
            Console.WriteLine("     Чистый блок работает только в том случае, если противник в том же положении бьёт, как и ты");
            Console.WriteLine("     В бою с ИИ - ИИ будет игроком 2 и будет бить каждые 100мс");
            Console.WriteLine("     Спецприёмы работают только в том случае, если игрок, который его совершает не держит блок и игроки находятся в одном положении, но спецприём можно заблокировать");
            Console.WriteLine();
            ConsoleKeyInfo hit;
            Timer timer1 = new Timer(aifightvsone, null, 0, 100);
            Timer timer2 = new Timer(aifightvstwo, null, 0, 100);
            Timer timer3 = new Timer(beeper, null, 0, 1000);
            Timer timer4 = new Timer(deather, null, 0, 1);
            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = new Stopwatch();
            Stopwatch stopwatch3 = new Stopwatch();
            Stopwatch stopwatch4 = new Stopwatch();
            do
            {
                if (ltime1 > 0)
                    ltime1--;
                if (ltime2 > 0)
                    ltime2--;
                if (fltime1 > 0)
                    fltime1--;
                if (fltime2 > 0)
                    fltime2--;
                if (pause == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("1 игрок: " + health1 + "хп, второй игрок: " + health2 + "хп");
                    Console.WriteLine("1 игрок стоит на " + place1 + "клетке, второй игрок на " + place2 + "клетке");
                    if (sitting1 == sitting2)
                        Console.WriteLine("Игроки в одном положении");
                    else if (sitting1 != sitting2)
                        Console.WriteLine("Игроки в разных положениях");
                    if (block1 == 1 & block2 == 1)
                        Console.WriteLine("Игрок 1 держит блок, Игрок 2 держит блок");
                    else if (block1 == 1 & block2 == 0)
                        Console.WriteLine("Игрок 1 держит блок");
                    else if (block2 == 1 & block1 == 0)
                        Console.WriteLine("Игрок 2 держит блок");
                    if (fltime2 == 1)
                        Console.WriteLine("Игрок 1 готов сделать чистый блок");
                    else if (fltime1 == 1)
                        Console.WriteLine("Игрок 2 готов сделать чистый блок");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if(health2 <= 0)
                {
                    Thread.Sleep(1);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Игрок 2 проиграл");
                    Console.ForegroundColor = ConsoleColor.Green;
                    health1 = 1000;
                    health2 = 1000;
                    block1 = 0;
                    block2 = 0;
                    place1 = 4;
                    place2 = 7;
                    fltime1 = 0;
                    fltime2 = 0;
                    ltime1 = 0;
                    ltime2 = 0;
                    sitting1 = 0;
                    sitting2 = 0;
                    Console.WriteLine("Всё сброшено по-умолчанию у обоих игроков");
                }
                if (health1 <= 0)
                {
                    Thread.Sleep(1);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Игрок 1 проиграл");
                    Console.ForegroundColor = ConsoleColor.Green;
                    health1 = 1000;
                    health2 = 1000;
                    block1 = 0;
                    block2 = 0;
                    place1 = 4;
                    place2 = 7;
                    fltime1 = 0;
                    fltime2 = 0;
                    ltime1 = 0;
                    ltime2 = 0;
                    sitting1 = 0;
                    sitting2 = 0;
                    Console.WriteLine("Всё сброшено по-умолчанию у обоих игроков");
                }
                Console.ForegroundColor = ConsoleColor.White;
                hit = Console.ReadKey();
                Console.WriteLine();
                if (pause == 0)
                {
                    if (hit.Key == ConsoleKey.J || hit.Key == ConsoleKey.I || hit.Key == ConsoleKey.K || hit.Key == ConsoleKey.L)
                    {
                        stopwatch2.Stop();
                        TimeSpan ts2 = stopwatch2.Elapsed;
                        if (block1 == 0)
                        {
                            if (ts2.Milliseconds == 0 & ts2.Seconds == 0)
                            {
                                if (place2 - place1 == 1)
                                {
                                    if (sitting1 == sitting2)
                                    {
                                        if (fltime1 == 1)
                                        {
                                            ltime1 = 2;
                                            Console.WriteLine("Игрок 2 отбил атаку чистым блоком");
                                        }
                                    }
                                    else if (sitting1 != sitting2)
                                    {
                                        if (fltime1 == 1)
                                        {
                                            Console.WriteLine("Игрок 2 не туда поставил чистый блок");
                                        }
                                    }
                                    if (ltime1 == 0)
                                    {
                                        if (block2 == 0)
                                        {
                                            health2 -= 10;
                                            Console.WriteLine("Игрок 2 получил урон");
                                        }
                                        else if (block2 == 1)
                                        {
                                            health2 -= 0.5;
                                            Console.WriteLine("Игрок 2 заблокировал удар");
                                        }
                                    }
                                }
                            }
                            if (hit.Key == ConsoleKey.J)
                            {
                                if (ts2.Milliseconds < 300 & ts2.Milliseconds > 0 & ts2.Seconds == 0)
                                {
                                    if (fltime1 == 1)
                                    {
                                         ltime1 = 2;
                                         Console.WriteLine("Игрок 2 отбил атаку чистым блоком");
                                    }
                                    if (ltime1 == 0)
                                    {
                                        if (block2 == 0)
                                        {
                                            if (sitting1 == sitting2)
                                            {
                                                place2 = place1 + 1;
                                                Console.WriteLine("Игрок 1 использовал спецприём Гарпун");
                                                Console.WriteLine("GET OVER HERE!");
                                            }
                                            else if (sitting1 != sitting2)
                                            {
                                                Console.WriteLine("Игрок 2 увернулся от цепи");
                                            }
                                        }
                                        else if (block2 == 1)
                                        {
                                            Console.WriteLine("Игрок 2 заблокировал цепь");
                                        }
                                    }
                                }
                            }
                            else if (hit.Key == ConsoleKey.I)
                            {
                                if (ts2.Milliseconds < 300 & ts2.Milliseconds > 0 & ts2.Seconds == 0)
                                {
                                    if (fltime1 == 1)
                                    {
                                        ltime1 = 2;
                                        Console.WriteLine("Игрок 2 отбил атаку чистым блоком");
                                    }
                                    if (ltime1 == 0)
                                    {
                                        if (block2 == 0)
                                        {
                                            if (sitting1 == sitting2)
                                            {
                                                health2 -= 10;
                                                Console.WriteLine("Игрок 1 использовал спецприём Выстрел Из Арбалета");
                                            }
                                            else if (sitting1 != sitting2)
                                            {
                                                Console.WriteLine("Игрок 2 увернулся от стрелы");
                                            }
                                        }
                                        else if (block2 != 0)
                                        {
                                            health2 -= 0.5;
                                            Console.WriteLine("Игрок 2 заблокировал стрелу");
                                        }
                                    }
                                }
                            }
                        }
                        stopwatch2.Restart();
                        stopwatch2.Stop();
                    }
                    else if (hit.Key == ConsoleKey.NumPad4 || hit.Key == ConsoleKey.NumPad1 || hit.Key == ConsoleKey.NumPad2 || hit.Key == ConsoleKey.NumPad3)
                    {
                        stopwatch4.Stop();
                        TimeSpan ts4 = stopwatch4.Elapsed;
                        if (block2 == 0)
                        {
                            if (ts4.Milliseconds == 0 & ts4.Seconds == 0)
                            {
                                if (place2 - place1 == 1)
                                {
                                    if (sitting1 == sitting2)
                                    {
                                        if (fltime2 == 1)
                                        {
                                            ltime2 = 2;
                                            Console.WriteLine("Игрок 1 отбил атаку чистым блоком");
                                        }
                                    }
                                    else if (sitting1 != sitting2)
                                    {
                                        if (fltime2 == 1)
                                        {
                                            Console.WriteLine("Игрок 1 не туда поставил чистый блок");
                                        }
                                    }
                                    if (ltime2 == 0)
                                    {
                                        if (block1 == 0)
                                        {
                                            health1 -= 10;
                                            Console.WriteLine("Игрок 1 получил урон");
                                        }
                                        else if (block1 == 1)
                                        {
                                            health1 -= 0.5;
                                            Console.WriteLine("Игрок 1 заблокировал удар");
                                        }
                                    }
                                }
                            }
                            if (hit.Key == ConsoleKey.NumPad4)
                            {
                                if (ts4.Milliseconds < 300 & ts4.Milliseconds > 0 & ts4.Seconds == 0)
                                {
                                    if (fltime2 == 1)
                                    {
                                        ltime2 = 2;
                                        Console.WriteLine("Игрок 1 отбил атаку чистым блоком");
                                    }
                                    if (ltime2 == 0)
                                    {
                                        if (block1 == 0)
                                        {
                                            if (sitting1 == sitting2)
                                            {
                                                place1 = place2 - 1;
                                                Console.WriteLine("Игрок 2 использовал спецприём Гарпун");
                                                Console.WriteLine("GET OVER HERE!");
                                            }
                                            else if (sitting1 != sitting2)
                                            {
                                                Console.WriteLine("Игрок 1 увернулся от цепи");
                                            }
                                        }
                                        else if (block1 == 1)
                                        {
                                            Console.WriteLine("Игрок 1 заблокировал цепь");
                                        }
                                    }
                                }
                            }
                            else if (hit.Key == ConsoleKey.NumPad1)
                            {
                                if (ts4.Milliseconds < 300 & ts4.Milliseconds > 0 & ts4.Seconds == 0)
                                {
                                    if (fltime2 == 1)
                                    {
                                        ltime2 = 2;
                                        Console.WriteLine("Игрок 1 отбил атаку чистым блоком");
                                    }
                                    if (ltime2 == 0)
                                    {
                                        if (block1 == 0)
                                        {
                                            if (sitting1 == sitting2)
                                            {
                                                health1 -= 10;
                                                Console.WriteLine("Игрок 2 использовал спецприём Выстрел Из Арбалета");
                                            }
                                            else if (sitting1 != sitting2)
                                            {
                                                Console.WriteLine("Игрок 1 увернулся от стрелы");
                                            }
                                        }
                                        else if (block1 != 0)
                                        {
                                            health1 -= 0.5;
                                            Console.WriteLine("Игрок 1 заблокировал стрелу");
                                        }
                                    }
                                }
                            }
                        }
                        stopwatch4.Restart();
                        stopwatch4.Stop();
                    }
                    else if (hit.Key == ConsoleKey.O)
                    {
                        if (ltime1 == 0)
                        {
                            if (block1 == 0)
                            {
                                block1 = 1;
                                Console.WriteLine("Игрок 1 поставил блок");
                            }
                            else if (block1 == 1)
                            {
                                block1 = 0;
                                Console.WriteLine("Игрок 1 отпустил блок");
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.NumPad5)
                    {
                        if (ltime2 == 0)
                        {
                            if (block2 == 0)
                            {
                                block2 = 1;
                                Console.WriteLine("Игрок 2 поставил блок");
                            }
                            else if (block2 == 1)
                            {
                                block2 = 0;
                                Console.WriteLine("Игрок 2 отпустил блок");
                            }
                        }
                    }
                    else if (hit.KeyChar.ToString() == ";")
                    {
                        if (ltime1 == 0)
                        {
                            if (block1 == 1)
                            {
                                block1 = 0;
                            }
                            fltime2 = 2;
                            Console.WriteLine("Игрок 1 приготовился использовать чистый блок");
                        }
                    }
                    else if (hit.Key == ConsoleKey.NumPad6)
                    {
                        if (ltime2 == 0)
                        {
                            if (block2 == 1)
                            {
                                block2 = 0;
                            }
                            fltime1 = 2;
                            Console.WriteLine("Игрок 2 приготовился использовать чистый блок");
                        }
                    }
                    else if (hit.Key == ConsoleKey.S)
                    {
                        if (ltime1 == 0)
                        {
                            if (sitting1 == 0)
                            {
                                sitting1 = 1;
                                Console.WriteLine("Игрок 1 присел");
                            }
                            else if (sitting1 == 1)
                            {
                                sitting1 = 0;
                                Console.WriteLine("Игрок 1 встал");
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.DownArrow)
                    {
                        if (ltime2 == 0)
                        {
                            if (sitting2 == 0)
                            {
                                sitting2 = 1;
                                Console.WriteLine("Игрок 2 присел");
                            }
                            else if (sitting2 == 1)
                            {
                                sitting2 = 0;
                                Console.WriteLine("Игрок 2 встал");
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.P)
                    {
                        if (ltime1 == 0)
                        {
                            if (aifighting2 == 0 & aifighting1 == 0)
                            {
                                aifighting2 = 1;
                                Console.WriteLine("ИИ включён против 1 игрока");
                            }
                            else if (aifighting2 == 1 || aifighting1 == 1)
                            {
                                aifighting1 = 0;
                                aifighting2 = 0;
                                Console.WriteLine("ИИ выключен");
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.NumPad9)
                    {
                        if (ltime2 == 0)
                        {
                            if (aifighting1 == 0 & aifighting2 == 0)
                            {
                                aifighting1 = 1;
                                Console.WriteLine("ИИ включён включён против 2 игрока");
                            }
                            else if (aifighting1 == 1 || aifighting2 == 1)
                            {
                                aifighting1 = 0;
                                aifighting2 = 0;
                                Console.WriteLine("ИИ выключен");
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.A)
                    {
                        if (ltime1 == 0)
                        {
                            if (block1 == 0)
                            {
                                if (place1 > 1)
                                {
                                    place1--;
                                    Console.WriteLine("Игрок 1 переместился на клетку назад");
                                }
                                stopwatch1.Start();
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.D)
                    {
                        if (ltime1 == 0)
                        {
                            if (block1 == 0)
                            {
                                if (place1 < 10)
                                {
                                    if (place2 - place1 > 1)
                                    {
                                        place1++;
                                        Console.WriteLine("Игрок 1 переместился на клетку вперед");
                                    }
                                }
                                stopwatch1.Stop();
                                TimeSpan ts1 = stopwatch1.Elapsed;
                                if (ts1.Milliseconds < 300 & ts1.Milliseconds > 0)
                                {
                                    stopwatch2.Start();
                                }
                                stopwatch1.Restart();
                                stopwatch1.Stop();
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.LeftArrow)
                    {
                        if (ltime2 == 0)
                        {
                            if (block2 == 0)
                            {
                                if (place2 > 1)
                                {
                                    if (place2 - place1 > 1)
                                    {
                                        place2--;
                                        Console.WriteLine("Игрок 2 переместился на клетку вперёд");
                                    }
                                }
                                stopwatch3.Stop();
                                TimeSpan ts3 = stopwatch3.Elapsed;
                                if (ts3.Milliseconds < 300 & ts3.Milliseconds > 0)
                                {
                                    stopwatch4.Start();
                                }
                                stopwatch3.Restart();
                                stopwatch3.Stop();
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.RightArrow)
                    {
                        if (ltime2 == 0)
                        {
                            if (block2 == 0)
                            {
                                if (place2 < 10)
                                {
                                    place2++;
                                    Console.WriteLine("Игрок 2 переместился на клетку назад");
                                }
                                stopwatch3.Start();
                            }
                        }
                    }
                }
                if (hit.Key == ConsoleKey.PageDown || hit.Key == ConsoleKey.Tab)
                {
                    if (pause == 0 || cdown == 1)
                    {
                        cdown = 0;
                        pause = 1;
                        Console.WriteLine("Поставлена пауза");
                        Console.WriteLine("Введите PageUp или M (англ), чтобы посмотреть управление и правила");
                        Console.WriteLine("PageDown или Tab - возобновить игру");
                    }
                    else if (pause == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Игра будет возобновлена через: ");
                        time = 4;
                        cdown = 1;
                    }
                }
                if (pause == 1)
                {
                    if (hit.Key == ConsoleKey.PageUp || hit.KeyChar.ToString() == "m" || hit.KeyChar.ToString() == "M")
                    {
                        Console.WriteLine("Управление :");
                        Console.WriteLine("     Игрок 1: Удар - J, блок - O, чистый блок - ;, присесть/встать - S, вперед - D, назад - A, вкл/выкл бой против ИИ - P (англ)");
                        Console.WriteLine("     Игрок 2: Удар - 4, блок - 5, чистый блок - 6, присесть/встать - ↓, вперёд - ←, назад - →, вкл/выкл бой против ИИ - 9");
                        Console.WriteLine("     Спецприём Гарпун - назад, вперёд, удар1 (J или 4)");
                        Console.WriteLine("     Спецприём Выстрел Из Арбалета - назад, вперёд, удар2 (I или 1)");
                        Console.WriteLine("     Бой с ИИ - 9 или P (англ)");
                        Console.WriteLine("     PageDown или Tab - пауза");
                        Console.WriteLine("     Закончить игру - esc или end");
                        Console.WriteLine("     Цвет игрока 1 - красный (соответственно, при его смерти будет красный цвет)");
                        Console.WriteLine("     Цвет игрока 2 - синий (соответственно, при его смерти будет синий цвет)");
                        Console.WriteLine("Правила :");
                        Console.WriteLine("     1000 хп");
                        Console.WriteLine("     Когда держишь блок, бить и передвигаться нельзя");
                        Console.WriteLine("     Незаблокированный удар наносит 10 урона");
                        Console.WriteLine("     Урон в блок 0.5хп");
                        Console.WriteLine("     Зажим наказывается");
                        Console.WriteLine("     Чистый блок дает бонус в 1 фрейм");
                        Console.WriteLine("     Чистый блок работает только в том случае, если противник в том же положении бьёт, как и ты");
                        Console.WriteLine("     В бою с ИИ - ИИ будет игроком 2 и будет бить каждые 100мс");
                        Console.WriteLine("     Спецприёмы работают только в том случае, если игрок, который его совершает не держит блок и игроки находятся в одном положении, но спецприём можно заблокировать");
                    }
                }
                if (hit.Key == ConsoleKey.Escape || hit.Key == ConsoleKey.End)
                    return;
            } while (true);
        }
        public static void aifightvsone(object obj)
        {
            if (aifighting2 == 1)
            {
                if (pause == 0)
                {
                    Console.WriteLine("4");
                    if (place2 - place1 == 1)
                    {
                        if (sitting1 == sitting2)
                        {
                            if (fltime2 == 1)
                            {
                                ltime2 = 2;
                                Console.WriteLine("Игрок 1 отбил атаку чистым блоком");
                            }
                        }
                        else if (sitting1 != sitting2)
                        {
                            if (fltime2 == 1)
                            {
                                Console.WriteLine("Игрок 1 не туда поставил чистый блок");
                            }
                        }
                        if (ltime2 == 0)
                        {
                            if (block1 == 0)
                            {
                                health1 -= 10;
                                Console.WriteLine("Игрок 1 получил урон");
                            }
                            else if (block1 == 1)
                            {
                                health1 -= 0.5;
                                Console.WriteLine("Игрок 1 заблокировал удар");
                            }
                        }
                    }
                    if (ltime1 > 0)
                        ltime1--;
                    if (ltime2 > 0)
                        ltime2--;
                    if (fltime1 > 0)
                        fltime1--;
                    if (fltime2 > 0)
                        fltime2--;
                    if (pause == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("1 игрок: " + health1 + "хп, второй игрок: " + health2 + "хп");
                        Console.WriteLine("1 игрок стоит на " + place1 + "клетке, второй игрок на " + place2 + "клетке");
                        if (sitting1 == 0)
                            Console.Write("Игрок 1 стоит, ");
                        else if (sitting1 == 1)
                            Console.Write("Игрок 1 сидит, ");
                        if (sitting2 == 0)
                            Console.WriteLine("Игрок 2 стоит");
                        else if (sitting2 == 1)
                            Console.WriteLine("Игрок 2 стоит");
                        if (block1 == 1)
                            Console.WriteLine("Игрок 1 держит блок");
                        if (block2 == 1)
                            Console.WriteLine("Игрок 2 держит блок");
                        if (fltime2 == 1)
                            Console.WriteLine("Игрок 1 готов сделать чистый блок");
                        else if (fltime1 == 1)
                            Console.WriteLine("Игрок 2 готов сделать чистый блок");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    if (health2 <= 0)
                    {
                        Thread.Sleep(1);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Игрок 2 проиграл");
                        Console.ForegroundColor = ConsoleColor.Green;
                        health1 = 1000;
                        health2 = 1000;
                        block1 = 0;
                        block2 = 0;
                        place1 = 4;
                        place2 = 7;
                        fltime1 = 0;
                        fltime2 = 0;
                        ltime1 = 0;
                        ltime2 = 0;
                        sitting1 = 0;
                        sitting2 = 0;
                        Console.WriteLine("Всё сброшено по-умолчанию у обоих игроков");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    if (health1 <= 0)
                    {
                        Thread.Sleep(1);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Игрок 1 проиграл");
                        Console.ForegroundColor = ConsoleColor.Green;
                        health1 = 1000;
                        health2 = 1000;
                        block1 = 0;
                        block2 = 0;
                        place1 = 4;
                        place2 = 7;
                        fltime1 = 0;
                        fltime2 = 0;
                        ltime1 = 0;
                        ltime2 = 0;
                        sitting1 = 0;
                        sitting2 = 0;
                        Console.WriteLine("Всё сброшено по-умолчанию у обоих игроков");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }
        public static void aifightvstwo(object obj)
        {
            if (aifighting1 == 1)
            {
                if (pause == 0)
                {
                    Console.WriteLine("J");
                    if (place2 - place1 == 1)
                    {
                        if (sitting1 == sitting2)
                        {
                            if (fltime1 == 1)
                            {
                                ltime1 = 2;
                                Console.WriteLine("Игрок 2 отбил атаку чистым блоком");
                            }
                        }
                        else if (sitting1 != sitting2)
                        {
                            if (fltime1 == 1)
                            {
                                Console.WriteLine("Игрок 2 не туда поставил чистый блок");
                            }
                        }
                        if (ltime1 == 0)
                        {
                            if (block2 == 0)
                            {
                                health2 -= 10;
                                Console.WriteLine("Игрок 2 получил урон");
                            }
                            else if (block2 == 1)
                            {
                                health2 -= 0.5;
                                Console.WriteLine("Игрок 2 заблокировал удар");
                            }
                        }
                    }
                    if (ltime1 > 0)
                        ltime1--;
                    if (ltime2 > 0)
                        ltime2--;
                    if (fltime1 > 0)
                        fltime1--;
                    if (fltime2 > 0)
                        fltime2--;
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("1 игрок: " + health1 + "хп, второй игрок: " + health2 + "хп");
                    Console.WriteLine("1 игрок стоит на " + place1 + "клетке, второй игрок на " + place2 + "клетке");
                    if (sitting1 == 0)
                        Console.Write("Игрок 1 стоит, ");
                    else if (sitting1 == 1)
                        Console.Write("Игрок 1 сидит, ");
                    if (sitting2 == 0)
                        Console.WriteLine("Игрок 2 стоит");
                    else if (sitting2 == 1)
                         Console.WriteLine("Игрок 2 стоит");
                    if (block1 == 1)
                          Console.WriteLine("Игрок 1 держит блок");
                    if (block2 == 1)
                          Console.WriteLine("Игрок 2 держит блок");
                    if (fltime2 == 1)
                          Console.WriteLine("Игрок 1 готов сделать чистый блок");
                    else if (fltime1 == 1)
                           Console.WriteLine("Игрок 2 готов сделать чистый блок");
                    if (health2 <= 0)
                    {
                        Thread.Sleep(1);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Игрок 2 проиграл");
                        Console.ForegroundColor = ConsoleColor.Green;
                        health1 = 1000;
                        health2 = 1000;
                        Console.WriteLine("Хп сброшены до 1000 у обоих игроков");
                    }
                    if (health1 <= 0)
                    {
                        Thread.Sleep(1);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Игрок 1 проиграл");
                        Console.ForegroundColor = ConsoleColor.Green;
                        health1 = 1000;
                        health2 = 1000;
                        Console.WriteLine("Хп сброшены до 1000 у обоих игроков");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        public static void beeper(object obj)
        {
            if (cdown == 1)
            {
                time--;
                musicer();
                Console.ForegroundColor = ConsoleColor.Cyan;
                if (time == 3)
                {
                    Console.Write("3сек, ");
                }
                else if (time == 2)
                {
                    Console.Write("2сек, ");
                }
                else if (time == 1)
                {
                    Console.Write("1сек");
                }
                else if (time == 0)
                {
                    pause = 0;
                    cdown = 0;
                    Console.WriteLine();
                    Console.WriteLine("Игра возобновлена");
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public static void deather(object obj)
        {
            if (health1 <= 0 || health2 <= 0)
                Console.Beep(3000,300);
        }
        public static void musicer()
        {
            if (time == 3)
            {
                Console.Beep(2000, 300);
            }
            else if (time == 2)
            {
                Console.Beep(2000, 300);
            }
            else if (time == 1)
            {
                Console.Beep(2000, 300);
            }
            else if (time == 0)
            {
                Console.Beep(1000, 300);
            }
        }
    }
}
