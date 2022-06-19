using System;
using System.Threading;
using System.Diagnostics;

namespace MortalKombat
{
    class Program
    {
        static bool block1 = false; // наличие блока 1 игрока
        static bool block2 = false; // наличие блока 2 игрока
        static double health1 = 1000; // хп 1 игрока
        static double health2 = 1000; // хп 2 игрока
        static bool pause = false; // наличие паузы
        static int ltime1 = 0; // потерянное время 1 игрока
        static int ltime2 = 0; // потерянное время 2 игрока
        static int fltime1 = 0; // возможная потеря времени 1 игрока
        static int fltime2 = 0; // возможная потеря времени 2 игрока
        static bool sitting1 = false; // сидит ли 1 игрок
        static bool sitting2 = false; // сидит ли 2 игрок
        static bool aifighting1 = false; // включён ли ИИ за 1 игрока против 2 игрока
        static bool aifighting2 = false; // включён ли ИИ за 2 игрока против 1 игрока
        static int place1 = 4; // местонахождение 1 игрока
        static int place2 = 7; // местонахождение 2 игрока
        static int time; // для отсчета после паузы
        static string player1; // для модельки перса 1 игрока
        static string player2; // для модельки перса 2 игрока
        static string positiontop; // верхнее поле боя 
        static string positionlow; // нижнее поле боя 
        static bool topplat1 = false; // верхняя или нижняя платформа у 1 игрока
        static bool topplat2 = false; // верхняя или нижняя платформа у 1 игрока
        static ConsoleKeyInfo hit; // ReadKey
        static Stopwatch stopwatch1 = new Stopwatch();
        static Stopwatch stopwatch2 = new Stopwatch();
        static Stopwatch stopwatch3 = new Stopwatch();
        static Stopwatch stopwatch4 = new Stopwatch();
        static Stopwatch stopwatch5 = new Stopwatch();
        static Stopwatch stopwatch6 = new Stopwatch();
        static Stopwatch stopwatch7 = new Stopwatch();
        static Stopwatch stopwatch8 = new Stopwatch();
        static TimeSpan ts1 = stopwatch1.Elapsed;
        static TimeSpan ts2 = stopwatch2.Elapsed;
        static TimeSpan ts3 = stopwatch3.Elapsed;
        static TimeSpan ts4 = stopwatch4.Elapsed;
        static TimeSpan ts5 = stopwatch5.Elapsed;
        static TimeSpan ts6 = stopwatch6.Elapsed;
        static TimeSpan ts7 = stopwatch7.Elapsed;
        static TimeSpan ts8 = stopwatch8.Elapsed;
        // таймеры для спецприёмов
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("                                                     MORTAL KOMBAT");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Управление :");
            Console.WriteLine("     Игрок 1: Удар - J,I,K,L, блок - O, чистый блок - ;, присесть - S, встать/прыжок - W, вперед - D, назад - A, вкл/выкл бой против ИИ - P (англ)");
            Console.WriteLine("     Игрок 2: Удар - 4,1,2,3, блок - 5, чистый блок - 6, присесть - ↓, встать/прыжок - ↑, вперёд - ←, назад - →, вкл/выкл бой против ИИ - 9");
            Console.WriteLine("     Подняться на платформу выше - прыжок");
            Console.WriteLine("     Опуститься на платформу ниже - дважды присесть");
            Console.WriteLine("     Спецприём Гарпун - назад, вперёд, удар1 (J или 4)");
            Console.WriteLine("     Спецприём Выстрел Из Арбалета - назад, вперёд, удар2 (I или 1)");
            Console.WriteLine("     Спецприём Телепорт - присесть/встать, назад, удар3 (K или 2)");
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
            Timer timer1 = new Timer(aifightvsone, null, 0, 100);
            Timer timer2 = new Timer(aifightvstwo, null, 0, 100);
            Timer timer3 = new Timer(deather, null, 0, 1); 
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
                if (pause == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("1 игрок: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(health1 + "хп, ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("второй игрок: ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(health2 + "хп");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("1 игрок стоит на " + place1 + "клетке, второй игрок на " + place2 + "клетке");
                    if (topplat1 != topplat2)
                        Console.WriteLine("Игроки стоят на разных платформах");
                    else if (topplat1 == topplat2)
                        Console.WriteLine("Игроки стоят на одной платформе");
                    if (sitting1 == sitting2)
                        Console.WriteLine("Игроки в одном положении");
                    else if (sitting1 != sitting2)
                        Console.WriteLine("Игроки в разных положениях");
                    if (block1 == true & block2 == true)
                        Console.WriteLine("Игрок 1 держит блок, Игрок 2 держит блок");
                    else if (block1 == true & block2 == false)
                        Console.WriteLine("Игрок 1 держит блок");
                    else if (block2 == true & block1 == false)
                        Console.WriteLine("Игрок 2 держит блок");
                    if (fltime2 == 1)
                        Console.WriteLine("Игрок 1 готов сделать чистый блок");
                    else if (fltime1 == 1)
                        Console.WriteLine("Игрок 2 готов сделать чистый блок");
                    Console.ForegroundColor = ConsoleColor.White;
                    positiontop = " __________";
                    positionlow = positiontop;
                    if (sitting1 == false)
                        player1 = "↑";
                    else if (sitting1 == true)
                        player1 = "↓";
                    if (sitting2 == false)
                        player2 = "▲";
                    else if (sitting2 == true)
                        player2 = "▼";
                    if (place2 > place1)
                    {
                        if (topplat1 == true)
                        {
                            positiontop = positiontop.Remove(place1, 1);
                            positiontop = positiontop.Insert(place1, player1);
                        }
                        else if (topplat1 == false)
                        {
                            positionlow = positionlow.Remove(place1, 1);
                            positionlow = positionlow.Insert(place1, player1);
                        }
                        if (topplat2 == true)
                        {
                            positiontop = positiontop.Remove(place2, 1);
                            positiontop = positiontop.Insert(place2, player2);
                        }
                        else if (topplat2 == false)
                        {
                            positionlow = positionlow.Remove(place2, 1);
                            positionlow = positionlow.Insert(place2, player2);
                        }
                    }
                    else if (place1 > place2)
                    {
                        if (topplat1 == true)
                        {
                            positiontop = positiontop.Remove(place1, 1);
                            positiontop = positiontop.Insert(place1, player1);
                        }
                        else if (topplat1 == false)
                        {
                            positionlow = positionlow.Remove(place1, 1);
                            positionlow = positionlow.Insert(place1, player1);
                        }
                        if (topplat2 == true)
                        {
                            positiontop = positiontop.Remove(place2, 1);
                            positiontop = positiontop.Insert(place2, player2);
                        }
                        else if (topplat2 == false)
                        {
                            positionlow = positionlow.Remove(place2, 1);
                            positionlow = positionlow.Insert(place2, player2);
                        }
                    }
                    Console.WriteLine(positiontop + "\n" + positionlow);
                }
                if(health2 <= 0 || health1 <= 0)
                {
                    Thread.Sleep(1); // это чтоб пикнуло когда игрок умрет, иначе если зажать клавишу и убить игрока, оно не пикнет
                    if (health2 <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Игрок 2 проиграл");
                    }
                    if (health1 <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Игрок 1 проиграл");
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    health1 = 1000;
                    health2 = 1000;
                    block1 = false;
                    block2 = false;
                    place1 = 4;
                    place2 = 7;
                    fltime1 = 0;
                    fltime2 = 0;
                    ltime1 = 0;
                    ltime2 = 0;
                    sitting1 = false;
                    sitting2 = false;
                    topplat1 = false;
                    topplat2 = false;
                    Console.WriteLine("Всё сброшено по-умолчанию у обоих игроков");
                }
                Console.ForegroundColor = ConsoleColor.White;
                while (Console.KeyAvailable)
                    Console.ReadKey(true);
                hit = Console.ReadKey(true);
                Console.WriteLine(hit.Key);
                stopwatch1.Stop();
                ts1 = stopwatch1.Elapsed;
                stopwatch2.Stop();
                ts2 = stopwatch2.Elapsed;
                stopwatch3.Stop();
                ts3 = stopwatch3.Elapsed;
                stopwatch4.Stop();
                ts4 = stopwatch4.Elapsed;
                stopwatch5.Stop();
                ts5 = stopwatch5.Elapsed;
                stopwatch6.Stop();
                ts6 = stopwatch6.Elapsed;
                stopwatch7.Stop();
                ts7 = stopwatch7.Elapsed;
                stopwatch8.Stop();
                ts8 = stopwatch8.Elapsed;
                if (pause == false)
                {
                    if (hit.Key == ConsoleKey.J || hit.Key == ConsoleKey.I || hit.Key == ConsoleKey.K || hit.Key == ConsoleKey.L)
                    {
                        if (block1 == false)
                        {
                            if (topplat1 == topplat2)
                            {
                                striker1();
                                if (hit.Key == ConsoleKey.J)
                                {
                                    buttonJ();
                                }
                                else if (hit.Key == ConsoleKey.I)
                                {
                                    buttonI();
                                }
                                else if (hit.Key == ConsoleKey.K)
                                {
                                    buttonK();
                                }
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.NumPad4 || hit.Key == ConsoleKey.NumPad1 || hit.Key == ConsoleKey.NumPad2 || hit.Key == ConsoleKey.NumPad3)
                    {
                        if (block2 == false)
                        {
                            if (topplat1 == topplat2)
                            {
                                striker2();
                                if (hit.Key == ConsoleKey.NumPad4)
                                {
                                    button4();
                                }
                                else if (hit.Key == ConsoleKey.NumPad1)
                                {
                                    button1();
                                }
                                else if (hit.Key == ConsoleKey.NumPad2)
                                {
                                    button2();
                                }
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.O)
                    {
                        blocker1();
                    }
                    else if (hit.Key == ConsoleKey.NumPad5)
                    {
                        blocker2();
                    }
                    else if (hit.Key == ConsoleKey.Oem1)
                    {
                        cblocker1();
                    }
                    else if (hit.Key == ConsoleKey.NumPad6)
                    {
                        cblocker2();
                    }
                    else if (hit.Key == ConsoleKey.S)
                    {
                        down1();
                    }
                    else if (hit.Key == ConsoleKey.W)
                    {
                        up1();
                    }
                    else if (hit.Key == ConsoleKey.DownArrow)
                    {
                        down2();
                    }
                    else if (hit.Key == ConsoleKey.UpArrow)
                    {
                        up2();
                    }
                    else if (hit.Key == ConsoleKey.P)
                    {
                        if (ltime1 == 0)
                        {
                            if (aifighting2 == false & aifighting1 == false)
                            {
                                aifighting2 = true;
                                Console.WriteLine("ИИ включён против 1 игрока");
                            }
                            else if (aifighting2 == true || aifighting1 == true)
                            {
                                aifighting1 = false;
                                aifighting2 = false;
                                Console.WriteLine("ИИ выключен");
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.NumPad9)
                    {
                        if (ltime2 == 0)
                        {
                            if (aifighting1 == false & aifighting2 == false)
                            {
                                aifighting1 = true;
                                Console.WriteLine("ИИ включён включён против 2 игрока");
                            }
                            else if (aifighting1 == true || aifighting2 == true)
                            {
                                aifighting1 = false;
                                aifighting2 = false;
                                Console.WriteLine("ИИ выключен");
                            }
                        }
                    }
                    else if (hit.Key == ConsoleKey.A)
                    {
                        left1();
                    }
                    else if (hit.Key == ConsoleKey.D)
                    {
                        right1();
                    }
                    else if (hit.Key == ConsoleKey.LeftArrow)
                    {
                        left2();
                    }
                    else if (hit.Key == ConsoleKey.RightArrow)
                    {
                        right2();
                    }
                }
                if (hit.Key == ConsoleKey.PageDown || hit.Key == ConsoleKey.Tab)
                {
                    if (pause == false)
                    {
                        pause = true;
                        Console.WriteLine("Поставлена пауза");
                        Console.WriteLine("Введите PageUp или M (англ), чтобы посмотреть управление и правила");
                        Console.WriteLine("PageDown или Tab - возобновить игру");
                    }
                    else if (pause == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Игра будет возобновлена через: ");
                        time = 4;
                        beeper();
                        Thread.Sleep(700);
                        beeper();
                        Thread.Sleep(700);
                        beeper();
                        Thread.Sleep(700);
                        beeper();
                    }
                }
                if (pause == true)
                {
                    if (hit.Key == ConsoleKey.PageUp || hit.Key == ConsoleKey.M)
                    {
                        Console.WriteLine("Управление :");
                        Console.WriteLine("     Игрок 1: Удар - J,I,K,L, блок - O, чистый блок - ;, присесть - S, встать - W, вперед - D, назад - A, вкл/выкл бой против ИИ - P (англ)");
                        Console.WriteLine("     Игрок 2: Удар - 4,1,2,3, блок - 5, чистый блок - 6, присесть - ↓, встать - ↑, вперёд - ←, назад - →, вкл/выкл бой против ИИ - 9");
                        Console.WriteLine("     Подняться на платформу выше - прыжок");
                        Console.WriteLine("     Опуститься на платформу ниже - дважды присесть");
                        Console.WriteLine("     Спецприём Гарпун - назад, вперёд, удар1 (J или 4)");
                        Console.WriteLine("     Спецприём Выстрел Из Арбалета - назад, вперёд, удар2 (I или 1)");
                        Console.WriteLine("     Спецприём Телепорт - присесть/встать, назад, удар3 (K или 2)");
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
                if (ts1.Milliseconds > 0 || ts1.Seconds > 0)
                {
                    stopwatch1.Restart();
                    stopwatch1.Stop();
                }
                if (ts2.Milliseconds > 0 || ts2.Seconds > 0)
                {
                    stopwatch2.Restart();
                    stopwatch2.Stop();
                }
                if (ts3.Milliseconds > 0 || ts3.Seconds > 0)
                {
                    stopwatch3.Restart();
                    stopwatch3.Stop();
                }
                if (ts4.Milliseconds > 0 || ts4.Seconds > 0)
                {
                    stopwatch4.Restart();
                    stopwatch4.Stop();
                }
                if (ts5.Milliseconds > 0 || ts5.Seconds > 0)
                {
                    stopwatch5.Restart();
                    stopwatch5.Stop();
                }
                if (ts6.Milliseconds > 0 || ts6.Seconds > 0)
                {
                    stopwatch6.Restart();
                    stopwatch6.Stop();
                }
                if (ts7.Milliseconds > 0 || ts7.Seconds > 0)
                {
                    stopwatch7.Restart();
                    stopwatch7.Stop();
                }
                if (ts8.Milliseconds > 0 || ts8.Seconds > 0)
                {
                    stopwatch8.Restart();
                    stopwatch8.Stop();
                }
                // чтоб таймеры обнулялись после нажатия последующей клавишы
                if (hit.Key == ConsoleKey.Escape || hit.Key == ConsoleKey.End)
                    return;
            } while (true);
        }
        public static void aifightvsone(object obj) // ИИ против 1 игрока
        {
            if (aifighting2 == true)
            {
                if (pause == false)
                {
                    Console.WriteLine("4");
                    if (topplat1 == topplat2)
                    {
                        if (place2 - place1 == 1 || place1 - place2 == 1)
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
                                if (block1 == false)
                                {
                                    health1 -= 10;
                                    Console.WriteLine("Игрок 1 получил урон");
                                }
                                else if (block1 == true)
                                {
                                    health1 -= 0.5;
                                    Console.WriteLine("Игрок 1 заблокировал удар");
                                }
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
                    Console.Write("1 игрок: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(health1 + "хп, ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("второй игрок: ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(health2 + "хп");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("1 игрок стоит на " + place1 + "клетке, второй игрок на " + place2 + "клетке");
                    if (topplat1 != topplat2)
                        Console.WriteLine("Игроки стоят на разных платформах");
                    else if (topplat1 == topplat2)
                        Console.WriteLine("Игроки стоят на одной платформе");
                    if (sitting1 == sitting2)
                        Console.WriteLine("Игроки в одном положении");
                    else if (sitting1 != sitting2)
                        Console.WriteLine("Игроки в разных положениях");
                    if (block1 == true & block2 == true)
                        Console.WriteLine("Игрок 1 держит блок, Игрок 2 держит блок");
                    else if (block1 == true & block2 == false)
                        Console.WriteLine("Игрок 1 держит блок");
                    else if (block2 == true & block1 == false)
                        Console.WriteLine("Игрок 2 держит блок");
                    if (fltime2 == 1)
                        Console.WriteLine("Игрок 1 готов сделать чистый блок");
                    else if (fltime1 == 1)
                        Console.WriteLine("Игрок 2 готов сделать чистый блок");
                    Console.ForegroundColor = ConsoleColor.White;
                    positiontop = " __________";
                    positionlow = positiontop;
                    if (sitting1 == false)
                        player1 = "↑";
                    else if (sitting1 == true)
                        player1 = "↓";
                    if (sitting2 == false)
                        player2 = "▲";
                    else if (sitting2 == true)
                        player2 = "▼";
                    if (place2 > place1)
                    {
                        if (topplat1 == true)
                        {
                            positiontop = positiontop.Remove(place1, 1);
                            positiontop = positiontop.Insert(place1, player1);
                        }
                        else if (topplat1 == false)
                        {
                            positionlow = positionlow.Remove(place1, 1);
                            positionlow = positionlow.Insert(place1, player1);
                        }
                        if (topplat2 == true)
                        {
                            positiontop = positiontop.Remove(place2, 1);
                            positiontop = positiontop.Insert(place2, player2);
                        }
                        else if (topplat2 == false)
                        {
                            positionlow = positionlow.Remove(place2, 1);
                            positionlow = positionlow.Insert(place2, player2);
                        }
                    }
                    else if (place1 > place2)
                    {
                        if (topplat1 == true)
                        {
                            positiontop = positiontop.Remove(place1, 1);
                            positiontop = positiontop.Insert(place1, player1);
                        }
                        else if (topplat1 == false)
                        {
                            positionlow = positionlow.Remove(place1, 1);
                            positionlow = positionlow.Insert(place1, player1);
                        }
                        if (topplat2 == true)
                        {
                            positiontop = positiontop.Remove(place2, 1);
                            positiontop = positiontop.Insert(place2, player2);
                        }
                        else if (topplat2 == false)
                        {
                            positionlow = positionlow.Remove(place2, 1);
                            positionlow = positionlow.Insert(place2, player2);
                        }
                    }
                    Console.WriteLine(positiontop + "\n" + positionlow);
                    if (health2 <= 0 || health1 <= 0)
                    {
                        Thread.Sleep(1); // это чтоб пикнуло когда игрок умрет, иначе если зажать клавишу и убить игрока, оно не пикнет
                        if (health2 <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Игрок 2 проиграл");
                        }
                        if (health1 <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Игрок 1 проиграл");
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        health1 = 1000;
                        health2 = 1000;
                        block1 = false;
                        block2 = false;
                        place1 = 4;
                        place2 = 7;
                        fltime1 = 0;
                        fltime2 = 0;
                        ltime1 = 0;
                        ltime2 = 0;
                        sitting1 = false;
                        sitting2 = false;
                        topplat1 = false;
                        topplat2 = false;
                        Console.WriteLine("Всё сброшено по-умолчанию у обоих игроков");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        public static void aifightvstwo(object obj) // ИИ против 2 игрока
        {
            if (aifighting1 == true)
            {
                if (pause == false)
                {
                    Console.WriteLine("J");
                    if (topplat1 == topplat2)
                    {
                        if (place2 - place1 == 1 || place1 - place2 == 1)
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
                                if (block2 == false)
                                {
                                    health2 -= 10;
                                    Console.WriteLine("Игрок 2 получил урон");
                                }
                                else if (block2 == true)
                                {
                                    health2 -= 0.5;
                                    Console.WriteLine("Игрок 2 заблокировал удар");
                                }
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
                    Console.Write("1 игрок: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(health1 + "хп, ");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("второй игрок: ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(health2 + "хп");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("1 игрок стоит на " + place1 + "клетке, второй игрок на " + place2 + "клетке");
                    if (topplat1 != topplat2)
                        Console.WriteLine("Игроки стоят на разных платформах");
                    else if (topplat1 == topplat2)
                        Console.WriteLine("Игроки стоят на одной платформе");
                    if (sitting1 == sitting2)
                        Console.WriteLine("Игроки в одном положении");
                    else if (sitting1 != sitting2)
                        Console.WriteLine("Игроки в разных положениях");
                    if (block1 == true & block2 == true)
                        Console.WriteLine("Игрок 1 держит блок, Игрок 2 держит блок");
                    else if (block1 == true & block2 == false)
                        Console.WriteLine("Игрок 1 держит блок");
                    else if (block2 == true & block1 == false)
                        Console.WriteLine("Игрок 2 держит блок");
                    if (fltime2 == 1)
                        Console.WriteLine("Игрок 1 готов сделать чистый блок");
                    else if (fltime1 == 1)
                        Console.WriteLine("Игрок 2 готов сделать чистый блок");
                    Console.ForegroundColor = ConsoleColor.White;
                    positiontop = " __________";
                    positionlow = positiontop;
                    if (sitting1 == false)
                        player1 = "↑";
                    else if (sitting1 == true)
                        player1 = "↓";
                    if (sitting2 == false)
                        player2 = "▲";
                    else if (sitting2 == true)
                        player2 = "▼";
                    if (place2 > place1)
                    {
                        if (topplat1 == true)
                        {
                            positiontop = positiontop.Remove(place1, 1);
                            positiontop = positiontop.Insert(place1, player1);
                        }
                        else if (topplat1 == false)
                        {
                            positionlow = positionlow.Remove(place1, 1);
                            positionlow = positionlow.Insert(place1, player1);
                        }
                        if (topplat2 == true)
                        {
                            positiontop = positiontop.Remove(place2, 1);
                            positiontop = positiontop.Insert(place2, player2);
                        }
                        else if (topplat2 == false)
                        {
                            positionlow = positionlow.Remove(place2, 1);
                            positionlow = positionlow.Insert(place2, player2);
                        }
                    }
                    else if (place1 > place2)
                    {
                        if (topplat1 == true)
                        {
                            positiontop = positiontop.Remove(place1, 1);
                            positiontop = positiontop.Insert(place1, player1);
                        }
                        else if (topplat1 == false)
                        {
                            positionlow = positionlow.Remove(place1, 1);
                            positionlow = positionlow.Insert(place1, player1);
                        }
                        if (topplat2 == true)
                        {
                            positiontop = positiontop.Remove(place2, 1);
                            positiontop = positiontop.Insert(place2, player2);
                        }
                        else if (topplat2 == false)
                        {
                            positionlow = positionlow.Remove(place2, 1);
                            positionlow = positionlow.Insert(place2, player2);
                        }
                    }
                    Console.WriteLine(positiontop + "\n" + positionlow);
                    if (health2 <= 0 || health1 <= 0)
                    {
                        Thread.Sleep(1); // это чтоб пикнуло когда игрок умрет, иначе если зажать клавишу и убить игрока, оно не пикнет
                        if (health2 <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Игрок 2 проиграл");
                        }
                        if (health1 <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Игрок 1 проиграл");
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        health1 = 1000;
                        health2 = 1000;
                        block1 = false;
                        block2 = false;
                        place1 = 4;
                        place2 = 7;
                        fltime1 = 0;
                        fltime2 = 0;
                        ltime1 = 0;
                        ltime2 = 0;
                        sitting1 = false;
                        sitting2 = false;
                        topplat1 = false;
                        topplat2 = false;
                        Console.WriteLine("Всё сброшено по-умолчанию у обоих игроков");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        public static void beeper() // отсчёт после паузы
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
                pause = false;
                Console.WriteLine();
                Console.WriteLine("Игра возобновлена");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void deather(object obj) // звук при смерти
        {
            if (health1 <= 0 || health2 <= 0)
                Console.Beep(3000,300);
        }
        public static void musicer() // пики при отсчёте после паузы
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
        public static void striker1() // удар 1 игрока
        {
            if (ts2.Milliseconds > 299 || ts2.Seconds > 0 || ts2.Milliseconds == 0)
            {
                if (place2 - place1 == 1 || place1 - place2 == 1)
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
                        if (block2 == false)
                        {
                            health2 -= 10;
                            Console.WriteLine("Игрок 2 получил урон");
                        }
                        else if (block2 == true)
                        {
                            health2 -= 0.5;
                            Console.WriteLine("Игрок 2 заблокировал удар");
                        }
                    }
                }
            }
        }
        public static void striker2() // удар 2 игрока
        {
            if (ts4.Milliseconds > 299 || ts4.Seconds > 0 || ts4.Milliseconds == 0)
            {
                if (place2 - place1 == 1 || place1 - place2 == 1)
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
                        if (block1 == false)
                        {
                            health1 -= 10;
                            Console.WriteLine("Игрок 1 получил урон");
                        }
                        else if (block1 == true)
                        {
                            health1 -= 0.5;
                            Console.WriteLine("Игрок 1 заблокировал удар");
                        }
                    }
                }
            }
        }
        public static void blocker1() // блок 1 игрока
        {
            if (ltime1 == 0)
            {
                if (block1 == false)
                {
                    block1 = true;
                    Console.WriteLine("Игрок 1 поставил блок");
                }
                else if (block1 == true)
                {
                    block1 = false;
                    Console.WriteLine("Игрок 1 отпустил блок");
                }
            }
        }
        public static void blocker2() // блок 2 игрока
        {
            if (ltime2 == 0)
            {
                if (block2 == false)
                {
                    block2 = true;
                    Console.WriteLine("Игрок 2 поставил блок");
                }
                else if (block2 == true)
                {
                    block2 = false;
                    Console.WriteLine("Игрок 2 отпустил блок");
                }
            }
        }
        public static void cblocker1() // чистый блок 1 игрока
        {
            if (ltime1 == 0)
            {
                if (block1 == true)
                {
                    block1 = false;
                }
                fltime2 = 2;
                Console.WriteLine("Игрок 1 приготовился использовать чистый блок");
            }
        }
        public static void cblocker2() // чистый блок 2 игрока
        {
            if (ltime2 == 0)
            {
                if (block2 == true)
                {
                    block2 = false;
                }
                fltime1 = 2;
                Console.WriteLine("Игрок 2 приготовился использовать чистый блок");
            }
        }
        public static void down1() // присесть за 1 игрока
        {
            if (ltime1 == 0)
            {
                if (sitting1 == false)
                {
                    sitting1 = true;
                    Console.WriteLine("Игрок 1 присел");
                }
                stopwatch5.Start();
                if (ts5.Milliseconds > 0 & ts5.Milliseconds < 300 & ts5.Seconds == 0)
                {
                    if (topplat1 == true)
                    {
                        topplat1 = false;
                        Console.WriteLine("Игрок 1 слез с платформы");
                    }
                }
            }
        }
        public static void down2() // присесть за 2 игрока
        {
            if (ltime2 == 0)
            {
                if (sitting2 == false)
                {
                    sitting2 = true;
                    Console.WriteLine("Игрок 2 присел");
                }
                stopwatch7.Start();
                if (ts7.Milliseconds > 0 & ts7.Milliseconds < 300 & ts7.Seconds == 0)
                {
                    if (topplat2 == true)
                    {
                        topplat2 = false;
                        Console.WriteLine("Игрок 2 слез с платформы");
                    }
                }
            }
        }
        public static void left1() // налево за 1 игрока
        {
            if (ltime1 == 0)
            {
                if (block1 == false)
                {
                    if (place1 > 1)
                    {
                        if (place1 - place2 > 1 || place2 - place1 >= 1 || topplat1 != topplat2)
                        {
                            place1--;
                            if (place2 > place1)
                                Console.WriteLine("Игрок 1 переместился на клетку назад");
                            else if (place1 > place2)
                                Console.WriteLine("Игрок 1 переместился на клетку вперёд");
                            if (place2 == place1 & topplat1 != topplat2)
                            {
                                if (place2 != 1)
                                    place1--;
                                else if (place2 == 1)
                                    place1++;
                            }
                        }
                    }
                    if (place2 > place1)
                    {
                        stopwatch1.Start();
                        if (ts5.Milliseconds < 300 & ts5.Milliseconds > 0 & ts5.Seconds == 0)
                        {
                            stopwatch6.Start();
                        }
                    }
                    else if (place1 > place2)
                    {
                        if (ts1.Milliseconds < 300 & ts1.Milliseconds > 0 & ts1.Seconds == 0)
                        {
                            stopwatch2.Start();
                        }
                    }
                }
            }
        }
        public static void left2() // налево за 2 игрока
        {
            if (ltime2 == 0)
            {
                if (block2 == false)
                {
                    if (place2 > 1)
                    {
                        if (place1 - place2 >= 1 || place2 - place1 > 1 || topplat1 != topplat2)
                        {
                            place2--;
                            if (place2 > place1)
                                Console.WriteLine("Игрок 2 переместился на клетку вперёд");
                            else if (place1 > place2)
                                Console.WriteLine("Игрок 2 переместился на клетку назад");
                            if (place2 == place1 & topplat1 != topplat2)
                                if (place2 == place1 & topplat1 != topplat2)
                                {
                                    if (place1 != 1)
                                        place2--;
                                    else if (place1 == 1)
                                        place2++;
                                }
                        }
                    }
                    if (place2 > place1)
                    {
                        if (ts3.Milliseconds < 300 & ts3.Milliseconds > 0 & ts3.Seconds == 0)
                        {
                            stopwatch4.Start();
                        }
                    }
                    else if (place1 > place2)
                    {
                        stopwatch3.Start();
                        if (ts7.Milliseconds < 300 & ts7.Milliseconds > 0 & ts7.Seconds == 0)
                        {
                            stopwatch8.Start();
                        }
                    }
                }
            }
        }
        public static void right1() // направо за 1 игрока
        {
            if (ltime1 == 0)
            {
                if (block1 == false)
                {
                    if (place1 < 10)
                    {
                        if (place1 - place2 >= 1 || place2 - place1 > 1 || topplat1 != topplat2)
                        {
                            place1++;
                            if (place2 > place1)
                                Console.WriteLine("Игрок 1 переместился на клетку вперёд");
                            else if (place1 > place2)
                                Console.WriteLine("Игрок 1 переместился на клетку назад");
                            if (place2 == place1 & topplat1 != topplat2)
                            {
                                if (place2 != 10)
                                    place1++;
                                else if (place2 == 10)
                                    place1--;
                            }
                        }
                    }
                    if (place2 > place1)
                    {
                        if (ts1.Milliseconds < 300 & ts1.Milliseconds > 0 & ts1.Seconds == 0)
                        {
                            stopwatch2.Start();
                        }
                    }
                    else if (place1 > place2)
                    {
                        stopwatch1.Start();
                        if (ts5.Milliseconds < 300 & ts5.Milliseconds > 0 & ts5.Seconds == 0)
                        {
                            stopwatch6.Start();
                        }
                    }
                }
            }
        }
        public static void right2() // направо за 2 игрока
        {
            if (ltime2 == 0)
            {
                if (block2 == false)
                {
                    if (place2 < 10)
                    {
                        if (place1 - place2 > 1 || place2 - place1 >= 1 || topplat1 != topplat2)
                        {
                            place2++;
                            if (place2 > place1)
                                Console.WriteLine("Игрок 2 переместился на клетку назад");
                            else if (place1 > place2)
                                Console.WriteLine("Игрок 2 переместился на клетку вперёд");
                            if (place2 == place1 & topplat1 != topplat2)
                            {
                                if (place1 != 10)
                                    place2++;
                                else if (place1 == 10)
                                    place2--;
                            }
                        }
                    }
                    if (place2 > place1)
                    {
                        stopwatch3.Start();
                        if (ts7.Milliseconds < 300 & ts7.Milliseconds > 0 & ts7.Seconds == 0)
                        {
                            stopwatch8.Start();
                        }
                    }
                    else if (place1 > place2)
                    {
                        if (ts3.Milliseconds < 300 & ts3.Milliseconds > 0 & ts3.Seconds == 0)
                        {
                            stopwatch4.Start();
                        }
                    }
                }
            }
        }
        public static void up1() // наверх за 1 игрока
        {
            if (ltime1 == 0)
            {
                if (sitting1 == true)
                {
                    sitting1 = false;
                    Console.WriteLine("Игрок 1 встал");
                }
                else if (sitting1 == false)
                {
                    if (topplat1 == false)
                    {
                        topplat1 = true;
                        Console.WriteLine("Игрок 1 залез на верхнюю платформу");
                    }
                }
            }
        }
        public static void up2() // наверх за 2 игрока
        {
            if (ltime2 == 0)
            {
                if (sitting2 == true)
                {
                    sitting2 = false;
                    Console.WriteLine("Игрок 2 встал");
                }
                else if (sitting2 == false)
                {
                    if (topplat2 == false)
                    {
                        topplat2 = true;
                        Console.WriteLine("Игрок 2 залез на верхнюю платформу");
                    }
                }
            }
        }
        public static void buttonJ() // если нажал на J (для спецприёма Гарпун)
        {
            if (ts2.Milliseconds < 300 & ts2.Milliseconds > 0 & ts2.Seconds == 0)
            {
                if (ltime1 == 0)
                {
                    if (block2 == false)
                    {
                        if (sitting1 == sitting2)
                        {
                            if (place2 > place1)
                                place2 = place1 + 1;
                            else if (place1 > place2)
                                place2 = place1 - 1;
                            Console.WriteLine("Игрок 1 использовал спецприём Гарпун");
                            Console.WriteLine("GET OVER HERE!");
                        }
                        else if (sitting1 != sitting2)
                        {
                            Console.WriteLine("Игрок 2 увернулся от цепи");
                        }
                    }
                    else if (block2 == true)
                    {
                        Console.WriteLine("Игрок 2 заблокировал цепь");
                    }
                }
            }
        }
        public static void buttonI() // если нажал на I (для спецприёма Выстрел Из Арбалета)
        {
            if (ts2.Milliseconds < 300 & ts2.Milliseconds > 0 & ts2.Seconds == 0)
            {
                if (ltime1 == 0)
                {
                    if (block2 == false)
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
                    else if (block2 == true)
                    {
                        health2 -= 0.5;
                        Console.WriteLine("Игрок 2 заблокировал стрелу");
                    }
                }
            }
        }
        public static void buttonK() // если нажал на K (для спецприёма Телепорт)
        {
            if (ts6.Milliseconds < 300 & ts6.Milliseconds > 0 & ts6.Seconds == 0)
            {
                if (ltime1 == 0)
                {
                    if (block2 == false)
                    {
                        health2 -= 10;
                        Console.WriteLine("Игрок 1 использовал спецприём Телепорт");
                    }
                    else if (block2 == true)
                    {
                        Console.WriteLine("Игрок 2 заблокировал телепорт");
                    }
                    if (place2 > place1)
                    {
                        if (place2 != 10)
                            place1 = place2 + 1;
                        else if (place2 == 10)
                        {
                            place2 = 9;
                            place1 = 10;
                        }
                    }
                    else if (place1 > place2)
                    {
                        if (place2 != 1)
                            place1 = place2 - 1;
                        else if (place2 == 1)
                        {
                            place2 = 2;
                            place1 = 1;
                        }
                    }
                }
            }
        }
        public static void button4() // если нажал на 4 (для спецприёма Гарпун)
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
                    if (block1 == false)
                    {
                        if (sitting1 == sitting2)
                        {
                            if (place2 > place1)
                                place1 = place2 - 1;
                            else if (place1 > place2)
                                place1 = place2 + 1;
                            Console.WriteLine("Игрок 2 использовал спецприём Гарпун");
                            Console.WriteLine("GET OVER HERE!");
                        }
                        else if (sitting1 != sitting2)
                        {
                            Console.WriteLine("Игрок 1 увернулся от цепи");
                        }
                    }
                    else if (block1 == true)
                    {
                        Console.WriteLine("Игрок 1 заблокировал цепь");
                    }
                }
            }
        }
        public static void button1() // если нажал на 1 (для спецприёма Выстрел Из Арбалета)
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
                    if (block1 == false)
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
                    else if (block1 == true)
                    {
                        health1 -= 0.5;
                        Console.WriteLine("Игрок 1 заблокировал стрелу");
                    }
                }
            }
        }
        public static void button2() // если нажал на 2 (для спецприёма Телепорт)
        {
            if (ts8.Milliseconds < 300 & ts8.Milliseconds > 0 & ts8.Seconds == 0)
            {
                if (ltime2 == 0)
                {
                    if (block1 == false)
                    {
                        health1 -= 10;
                        Console.WriteLine("Игрок 2 использовал спецприём Телепорт");
                    }
                    else if (block2 == true)
                    {
                        Console.WriteLine("Игрок 1 заблокировал телепорт");
                    }
                    if (place2 > place1)
                    {
                        if (place1 != 1)
                            place2 = place1 - 1;
                        else if (place1 == 1)
                        {
                            place1 = 2;
                            place2 = 1;
                        }
                    }
                    else if (place1 > place2)
                    {
                        if (place1 != 10)
                            place2 = place1 + 1;
                        else if (place1 == 10)
                        {
                            place1 = 9;
                            place2 = 10;
                        }
                    }
                }
            }
        }
    }
}
