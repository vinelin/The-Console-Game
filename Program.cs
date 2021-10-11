using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Media;

namespace Game
{

    enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }

    enum Map
    {
        map1,
        map2,
        map3,
        map_library,
    }

    class Character
    {
        public Point p;
        public Direction dir;
        public bool Gui = true;//测试用 初始为false
        public bool fo = false;
        public Map map = Map.map1;
        public int book = 0;

        public Character(int x, int y)
        {
            p = new Point(x, y, ConsoleColor.Red, ConsoleColor.Black, "主");
            dir = Direction.None;
        }
    }




    class Program
    {
        static Point[,] trans_map_1 = new Point[16, 16];
        static Point[,] trans_map_2 = new Point[16, 16];
        static Point[,] trans_map_library = new Point[16, 16];
        static Point[,] trans_qipan = new Point[16, 16];
        static string[,] dialog = new string[16, 16];
        static int Runcounter = 0;
        static string death;
        static int knock_times = 0;
        static string Boss;
        static int book_in_box = 0;

        //翻译地图，给出各种信息
        static void Translate(string map_name, Point[,] trans_map)
        {
            string[,] map = new string[16, 16];
            map = ReadCsv($"..\\..\\{map_name}.csv", map);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {

                    if (map[i, j] == "房")
                    {
                        //Debug.WriteLine($"{j} - {i}");
                        //Debug.WriteLine($"{Console.CursorLeft}={Console.CursorTop}");
                        PrintControl.DrawHouse(j * 2, i, trans_map);
                    }
                    else if (map[i, j] == "卍")
                    {
                        PrintControl.DrawTemple(j * 2, i, trans_map);
                    }
                    else if (map[i, j] == "一")
                    {
                        //Debug.WriteLine($"{Console.CursorLeft}={Console.CursorTop}");
                        //Console.SetCursorPosition(Console.CursorLeft + 2,Console.CursorTop);
                        continue;
                    }
                    else if (map[i, j] == "█")
                    {
                        //Console.ForegroundColor = ConsoleColor.DarkGreen;
                        //Console.Write(map_1[i, j]);
                        trans_map[i, j] = new Point(j, i, ConsoleColor.DarkGreen, ConsoleColor.Black, map[i, j]);
                        //Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (map[i, j] == "〓" || map[i, j] == "■")
                    {
                        trans_map[i, j] = new Point(j, i, ConsoleColor.Black, ConsoleColor.White, map[i, j]);
                    }
                    else if (map[i, j] == "箱")
                    {
                        trans_map[i, j] = new Point(j, i, ConsoleColor.DarkYellow, ConsoleColor.Black, map[i, j]);
                    }
                    else if (map[i, j] == "书")
                    {
                        trans_map[i, j] = new Point(j, i, ConsoleColor.Green, ConsoleColor.Black, map[i, j]);
                    }
                    else
                    {
                        trans_map[i, j] = new Point(j, i, ConsoleColor.White, ConsoleColor.Black, map[i, j]);
                    }

                }
            }
        }

        static void LoadQipan(string file_name)
        {
            string[,] map = new string[16, 16];
            map = ReadCsv($"..\\..\\{file_name}.csv", map);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == 1 && j == 2)
                    {
                        trans_qipan[i, j] = new Point(j, i, ConsoleColor.Blue, ConsoleColor.Black, map[i, j]);
                    }
                    else if (i == 1 && j == 3)
                    {
                        trans_qipan[i, j] = new Point(j, i, ConsoleColor.Yellow, ConsoleColor.Black, map[i, j]);
                    }
                    else if (i == 3 && j == 1)
                    {
                        trans_qipan[i, j] = new Point(j, i, ConsoleColor.Red, ConsoleColor.Black, map[i, j]);
                    }
                    else if (i == 3 && j == 4)
                    {
                        trans_qipan[i, j] = new Point(j, i, ConsoleColor.Magenta, ConsoleColor.Black, map[i, j]);
                    }
                    else if (i == 4 && j == 2)
                    {
                        trans_qipan[i, j] = new Point(j, i, ConsoleColor.Red, ConsoleColor.Black, map[i, j]);
                    }
                    else if (i == 4 && j == 3)
                    {
                        trans_qipan[i, j] = new Point(j, i, ConsoleColor.Magenta, ConsoleColor.Black, map[i, j]);
                    }
                    else if (i == 0 && j == 7)
                    {
                        trans_qipan[i, j] = new Point(j, i, ConsoleColor.Magenta, ConsoleColor.Black, map[i, j]);
                    }
                    else if (i == 1 && j == 7)
                    {
                        trans_qipan[i, j] = new Point(j, i, ConsoleColor.Yellow, ConsoleColor.Black, map[i, j]);
                    }
                    else if (i == 2 && j == 7)
                    {
                        trans_qipan[i, j] = new Point(j, i, ConsoleColor.Yellow, ConsoleColor.Black, map[i, j]);
                    }
                    else if (i == 3 && j == 7)
                    {
                        trans_qipan[i, j] = new Point(j, i, ConsoleColor.Magenta, ConsoleColor.Black, map[i, j]);
                    }
                    else
                    {
                        trans_qipan[i, j] = new Point(j, i, ConsoleColor.White, ConsoleColor.Black, map[i, j]);
                    }
                }
            }
        }

        //把对话框打印出来
        static void Dialog(string csv_name)
        {
            dialog = ReadCsv($"..\\..\\{csv_name}.csv", dialog);
            for (int i = 0; i < dialog.GetLength(0); i++)
            {
                for (int j = 0; j < dialog.GetLength(1); j++)
                {
                    Console.SetCursorPosition(j * 2 + trans_map_1.GetLength(1) * 2, i);
                    Console.Write(dialog[i, j]);
                }
            }
        }

        static void Dialog()
        {
            for (int i = 0; i < dialog.GetLength(0); i++)
            {
                for (int j = 0; j < dialog.GetLength(1); j++)
                {
                    Console.SetCursorPosition(j * 2 + trans_map_1.GetLength(1) * 2, i);
                    Console.Write(dialog[i, j]);
                }
            }
        }

        //渲染地图
        static void MapDraw(Point[,] trans_map)
        {
            for (int i = 0; i < trans_map.GetLength(0); i++)
            {
                for (int j = 0; j < trans_map.GetLength(1); j++)
                {
                    trans_map[i, j].Draw();
                }
            }
        }
        //根据方向打印主角的表示符号
        static void WritePlayer(Character player)
        {
            switch (player.dir)
            {
                case Direction.Left:
                    Console.Write("←");
                    break;
                case Direction.Right:
                    Console.Write("→");
                    break;
                case Direction.Up:
                    Console.Write("↑");
                    break;
                case Direction.Down:
                    Console.Write("↓");
                    break;
                default:
                    Console.Write("●");
                    break;
            }

        }

        static void Init()
        {

            //渲染地图
            Translate("map_1", trans_map_1);
            Translate("map_2", trans_map_2);
            Translate("map_library", trans_map_library);
            //加载棋盘
            LoadQipan("qipan");
            Dialog("tips");
            LoadBoss(ref Boss);
        }

        static void LoadBoss(ref string Boss)
        {
            int len = 0;
            byte[] bytes = new byte[10240];
            using (FileStream file = new FileStream("..\\..\\boss\\ASCII-boss_0.txt", FileMode.Open))
            {
                len = file.Read(bytes, 0, 10240);
            }
            string source = Encoding.UTF8.GetString(bytes, 0, len);
            Boss = source;
        }

        //读取CSV
        static string[,] ReadCsv(string filename, string[,] map)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filename);
                int i = 0;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');
                    for (int j = 0; j < values.Length; j++)
                    {
                        map[i, j] = values[j];
                    }
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                sr.Close();
            }
            return map;
        }


        //播放字符串动画
        static void PlayVideo(int VideoLen)
        {
            //三个临时变量:长度,file 以及用byte数组作为缓冲区
            int len = 0;
            byte[] bytes = new byte[10240];
            for (int i = 0; i < VideoLen; i++)
            {
                string file_dir = string.Format("..\\..\\new\\ASCII-frame_{0}_delay-0.2s.txt", i);
                using (FileStream file = new FileStream(file_dir, FileMode.Open))
                {
                    len = file.Read(bytes, 0, 10240);
                }
                string source = Encoding.UTF8.GetString(bytes, 0, len);
                Console.WriteLine(source);
                Thread.Sleep(50);
                Console.Clear();
            }
            string file_death = "..\\..\\ASCII-death.txt";
            using (FileStream file = new FileStream(file_death, FileMode.Open))
            {
                len = file.Read(bytes, 0, 10240);
            }
            death = Encoding.UTF8.GetString(bytes, 0, len);
        }

        //搜索周围的玩家周围的四个格子
        static bool Search(Character player, string aim, ConsoleColor color)
        {
            int y = player.p.y;
            int x = player.p.x;
            switch (player.dir)
            {
                case Direction.Right:
                    if (x > 14)
                    {
                        x = 14;
                    }
                    return trans_map_1[y, x + 1].content == aim && trans_map_1[y, x + 1].f_color == color;
                case Direction.Left:
                    if (x < 1)
                    {
                        x = 1;
                    }
                    return trans_map_1[y, x - 1].content == aim && trans_map_1[y, x - 1].f_color == color;
                case Direction.Up:
                    if (y < 1)
                    {
                        y = 1;
                    }
                    return trans_map_1[y - 1, x].content == aim && trans_map_1[y - 1, x].f_color == color;
                case Direction.Down:
                    if (y > 14)
                    {
                        y = 14;
                    }
                    return trans_map_1[y + 1, x].content == aim && trans_map_1[y + 1, x].f_color == color;
                default:
                    return false;
            }
        }

        static void DrawInitStatus()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(30, 17);
            for (int i = 0; i <= 18; i += 2)
            {
                Console.Write("●");
            }
            Console.ForegroundColor = ConsoleColor.DarkRed;
            for (int i = 20; i <= 38; i += 2)
            {
                Console.Write("¤");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        //基本上是行动的逻辑 还有对话的逻辑
        //map1上的所有逻辑
        static void PlayerMoveMap1(ref Character player)
        {
            while (true)
            {
                Point next = new Point(player.p);
                if (Console.KeyAvailable)
                {
                    Debug.WriteLine($"player:{player.p.draw_left}=={player.p.draw_top}");
                    ConsoleKeyInfo userInput = Console.ReadKey(true);

                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        player.dir = Direction.Left;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.x--;
                        next.draw_left = next.x * 2;

                    }
                    else if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        player.dir = Direction.Right;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.x++;
                        next.draw_left = next.x * 2;
                    }
                    else if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        player.dir = Direction.Up;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.y--;
                        next.draw_top = next.y;
                    }
                    else if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        player.dir = Direction.Down;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.y++;
                        next.draw_top = next.y;
                    }
                    //与物体交互的逻辑
                    else if (userInput.Key == ConsoleKey.Enter)
                    {
                        if (Search(player, "卍", ConsoleColor.Yellow) && player.fo)
                        {
                            VoiceOver.Speak(VoiceOver.Situation.knockwithfo);
                        }
                        else if (Search(player, "卍", ConsoleColor.Yellow) && knock_times < 100)
                        {
                            VoiceOver.Speak(VoiceOver.Situation.knock1);
                            knock_times++;
                            Debug.WriteLine("你磕了{0}个头", knock_times);
                            if (knock_times == 50)
                            {
                                VoiceOver.Speak(VoiceOver.Situation.knock2);
                                Thread.Sleep(3000);
                                VoiceOver.ClearDialog();
                            }
                            if (knock_times == 100)
                            {
                                VoiceOver.Speak(VoiceOver.Situation.knock3_1);
                                Thread.Sleep(3000);
                                VoiceOver.Speak(VoiceOver.Situation.knock3_2);
                                Thread.Sleep(3000);
                                player.Gui = true;
                                VoiceOver.ClearDialog();
                            }
                        }
                        if (Search(player, "█", ConsoleColor.DarkYellow))
                        {

                            player.map = Map.map_library;
                            if (player.p.x == 10)
                            {
                                player.p.x = 7;
                                player.p.draw_left = player.p.x * 2;
                                player.p.y = 12;
                                player.p.draw_top = player.p.y;
                            }
                            else if (player.p.x == 11)
                            {
                                player.p.x = 8;
                                player.p.draw_left = player.p.x * 2;
                                player.p.y = 12;
                                player.p.draw_top = player.p.y;
                            }
                            break;
                        }
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;

                    }
                    else
                    {
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                    Debug.WriteLine($"next坐标y:{next.y}------x:{next.x}");
                    //鬼打墙
                    if (next.y < 0)
                    {
                        next.y = trans_map_1.GetLength(1) - 1;
                        next.draw_top = next.y;
                        Runcounter--;
                    }
                    else if (next.y >= trans_map_1.GetLength(1))
                    {
                        next.y = 0;
                        next.draw_top = next.y;
                        Runcounter++;
                    }
                    else if (next.x > 15)
                    {
                        if (player.Gui || player.fo)
                        {
                            player.map = Map.map2;
                            player.p.x = 0;
                            player.p.draw_left = 0;
                            break;
                        }
                        else
                        {
                            VoiceOver.Speak(VoiceOver.Situation.gg);
                            Thread.Sleep(1000);
                            VoiceOver.ClearDialog();
                            continue;
                        }
                    }

                    //只能走路的字符，所以这样写
                    if (trans_map_1[next.y, next.x].content == "▓")
                    {
                        next.f_color = trans_map_1[next.y, next.x].f_color;
                        next.b_color = trans_map_1[next.y, next.x].b_color;
                        Console.SetCursorPosition(next.draw_left, next.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        Console.BackgroundColor = player.p.b_color;
                        WritePlayer(player);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = next.f_color;
                        Console.BackgroundColor = next.b_color;
                        Console.Write("▓");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        //Debug.WriteLine($"Next:{next.draw_left}=={next.draw_top}");
                        player.p.x = next.x;
                        player.p.draw_left = next.draw_left;
                        player.p.y = next.y;
                        player.p.draw_top = next.draw_top;
                        //Debug.WriteLine($"new_player:{player.p.draw_left}=={player.p.draw_top}");
                    }
                    //Debug.WriteLine($"交换完成 next坐标y:{next.y}------x:{next.x}");
                }
            }
        }
        //map2上的所有逻辑 重点是变色的背景
        static void PlayerMoveMap2(ref Character player)
        {
            while (true)
            {
                Point next = new Point(player.p);

                if (Console.KeyAvailable)
                {
                    Debug.WriteLine($"player:{player.p.draw_left}=={player.p.draw_top}");
                    ConsoleKeyInfo userInput = Console.ReadKey(true);

                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        player.dir = Direction.Left;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.x--;
                        next.draw_left = next.x * 2;

                    }
                    else if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        player.dir = Direction.Right;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.x++;
                        next.draw_left = next.x * 2;
                    }
                    else if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        player.dir = Direction.Up;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.y--;
                        next.draw_top = next.y;
                    }
                    else if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        player.dir = Direction.Down;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.y++;
                        next.draw_top = next.y;
                    }
                    else
                    {
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                    if (next.x < 0)
                    {
                        VoiceOver.Speak(VoiceOver.Situation.wantToBack);
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                    if (next.x > 15)
                    {
                        player.map = Map.map3;
                        break;
                    }
                    //只能走路的字符，所以这样写
                    if (trans_map_2[next.y, next.x].content == "▓")
                    {
                        next.f_color = trans_map_2[next.y, next.x].f_color;
                        next.b_color = trans_map_2[next.y, next.x].b_color;
                        Console.SetCursorPosition(next.draw_left, next.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        Console.BackgroundColor = player.p.b_color;
                        WritePlayer(player);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = next.f_color;
                        Console.BackgroundColor = next.b_color;
                        Console.Write("▓");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Debug.WriteLine($"Next:{next.draw_left}=={next.draw_top}");
                        player.p.x = next.x;
                        player.p.draw_left = next.draw_left;
                        player.p.y = next.y;
                        player.p.draw_top = next.draw_top;
                        Debug.WriteLine($"new_player:{player.p.draw_left}=={player.p.draw_top}");
                    }

                    //地图变色逻辑
                    if (player.p.x > 10)
                    {
                        for (int i = 0; i <= 6; i++)
                        {
                            for (int j = 0; j < trans_map_2.GetLength(1); j++)
                            {
                                Console.SetCursorPosition(trans_map_2[i, j].draw_left, trans_map_2[i, j].draw_top);
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(trans_map_2[i, j].content);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        for (int i = 9; i <= 15; i++)
                        {
                            for (int j = 0; j < trans_map_2.GetLength(1); j++)
                            {
                                Console.SetCursorPosition(trans_map_2[i, j].draw_left, trans_map_2[i, j].draw_top);
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(trans_map_2[i, j].content);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }
                    else if (player.p.x > 5)
                    {
                        for (int i = 0; i <= 6; i++)
                        {
                            for (int j = 0; j < trans_map_2.GetLength(1); j++)
                            {
                                Console.SetCursorPosition(trans_map_2[i, j].draw_left, trans_map_2[i, j].draw_top);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(trans_map_2[i, j].content);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        for (int i = 9; i <= 15; i++)
                        {
                            for (int j = 0; j < trans_map_2.GetLength(1); j++)
                            {
                                Console.SetCursorPosition(trans_map_2[i, j].draw_left, trans_map_2[i, j].draw_top);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(trans_map_2[i, j].content);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }
                    else if (player.p.x >= 0)
                    {
                        for (int i = 0; i <= 6; i++)
                        {
                            for (int j = 0; j < trans_map_2.GetLength(1); j++)
                            {
                                trans_map_2[i, j].Draw();
                            }
                        }
                        for (int i = 9; i <= 15; i++)
                        {
                            for (int j = 0; j < trans_map_2.GetLength(1); j++)
                            {
                                trans_map_2[i, j].Draw();
                            }
                        }
                    }
                }

            }
        }
        //map3上的所有逻辑 Boss战
        static void PlayerMoveGui(ref Character player)
        {
            Console.Clear();
            Console.WriteLine(Boss);
            Thread.Sleep(2000);
            Console.Clear();
            Dialog();
            VoiceOver.Speak(VoiceOver.Situation.fight);
            DrawInitStatus();
            Console.SetCursorPosition(48, 17);
            Debug.WriteLine($"{Console.CursorLeft}----{Console.CursorTop}");
            while (true)
            {
                while(Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey(true);
                    if (userInput.Key == ConsoleKey.Spacebar)
                    {
                        Debug.WriteLine("按了空格");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("●●");
                        Console.ForegroundColor = ConsoleColor.White;
                        if (Console.CursorLeft > 68)
                        {
                            VoiceOver.Speak(VoiceOver.Situation.victory);
                            Thread.Sleep(120000);
                            break;
                        }
                    }
                }
                Thread.Sleep(200);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("¤");
                Console.ForegroundColor = ConsoleColor.White;
                if (Console.CursorLeft - 4 < 30)
                {
                    VoiceOver.Speak(VoiceOver.Situation.fail);
                    Thread.Sleep(120000);
                    break;
                }
                Console.SetCursorPosition(Console.CursorLeft - 4, Console.CursorTop);
                Debug.WriteLine($"没按空格    {Console.CursorLeft}----{Console.CursorTop}");
            }
        }

        static void PlayerMoveFo(ref Character player)
        {
            Console.Clear();
            Console.WriteLine(Boss);
            Thread.Sleep(2000);
            Console.Clear();
            Dialog();
            VoiceOver.Speak(VoiceOver.Situation.enterFoEnd);
            MapDraw(trans_qipan);
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey(true);
                    if (userInput.Key == ConsoleKey.D)
                    {
                        VoiceOver.Speak(VoiceOver.Situation.FoEndVictory);
                        Thread.Sleep(120000);
                        break;
                    }
                    else if (userInput.Key == ConsoleKey.A || userInput.Key == ConsoleKey.B || userInput.Key == ConsoleKey.C)
                    {
                        VoiceOver.Speak(VoiceOver.Situation.FoEndFail);
                        Thread.Sleep(120000);
                        break;
                    }
                }
            }
        }

        static void PlayerBestEnd(ref Character player)
        {
            Console.Clear();
            Dialog();
            VoiceOver.Speak(VoiceOver.Situation.BestEnd);
            Thread.Sleep(120000);
        }

        //图书馆的逻辑

        static bool TakeBook(Character player, string aim, out int book_y, out int book_x)
        {
            book_y = player.p.y;
            book_x = player.p.x;
            switch (player.dir)
            {
                case Direction.Right:
                    book_x = book_x + 1;
                    return trans_map_library[book_y, book_x].content == aim;
                case Direction.Left:
                    book_x = book_x - 1;
                    return trans_map_library[book_y, book_x].content == aim;
                case Direction.Up:
                    book_y = book_y - 1;
                    return trans_map_library[book_y, book_x].content == aim;
                case Direction.Down:
                    book_y = book_y + 1;
                    return trans_map_library[book_y, book_x].content == aim;
                default:
                    return false;
            }
        }
        static void PlayerMoveLibrary(ref Character player)
        {
            while (true)
            {
                Point next = new Point(player.p);

                if (Console.KeyAvailable)
                {
                    Debug.WriteLine($"player:{player.p.x}=={player.p.y}");
                    ConsoleKeyInfo userInput = Console.ReadKey(true);

                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        player.dir = Direction.Left;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.x--;
                        next.draw_left = next.x * 2;

                    }
                    else if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        player.dir = Direction.Right;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.x++;
                        next.draw_left = next.x * 2;
                    }
                    else if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        player.dir = Direction.Up;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.y--;
                        next.draw_top = next.y;
                    }
                    else if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        player.dir = Direction.Down;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        next.y++;
                        next.draw_top = next.y;
                    }
                    else if (userInput.Key == ConsoleKey.Enter && player.p.y == 12)
                    {
                        player.map = Map.map1;
                        if (player.p.x == 7)
                        {
                            player.p.x = 10;
                            player.p.draw_left = player.p.x * 2;
                            player.p.y = 6;
                            player.p.draw_top = player.p.y;
                        }
                        else if (player.p.x == 8)
                        {
                            player.p.x = 11;
                            player.p.draw_left = player.p.x * 2;
                            player.p.y = 6;
                            player.p.draw_top = player.p.y;
                        }
                        break;
                    }
                    else if (userInput.Key == ConsoleKey.Enter)
                    {
                        Debug.WriteLine("捡书");
                        int book_x;
                        int book_y;
                        if (TakeBook(player, "书", out book_y, out book_x))
                        {
                            Debug.WriteLine("拿到了");
                            player.book++;
                            Console.SetCursorPosition(book_x * 2, book_y);
                            trans_map_library[book_y, book_x] = new Point(book_x, book_y, ConsoleColor.White, ConsoleColor.Black, "▓");
                            Console.Write("▓");
                            Debug.WriteLine(player.book);
                            Debug.WriteLine(book_in_box);
                        }
                        else if (TakeBook(player, "箱", out book_y, out book_x))
                        {
                            Debug.WriteLine("放书");
                            book_in_box = book_in_box + player.book;
                            player.book = 0;
                            if (book_in_box == 6)
                            {
                                player.fo = true;
                                VoiceOver.Speak(VoiceOver.Situation.finishlibrary);
                            }
                        }
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                    else
                    {
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                    //只能走路的字符，所以这样写
                    if (trans_map_library[next.y, next.x].content == "▓")
                    {
                        next.f_color = trans_map_library[next.y, next.x].f_color;
                        next.b_color = trans_map_library[next.y, next.x].b_color;
                        Console.SetCursorPosition(next.draw_left, next.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        Console.BackgroundColor = player.p.b_color;
                        WritePlayer(player);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = next.f_color;
                        Console.BackgroundColor = next.b_color;
                        Console.Write("▓");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Debug.WriteLine($"Next:{next.draw_left}=={next.draw_top}");
                        player.p.x = next.x;
                        player.p.draw_left = next.draw_left;
                        player.p.y = next.y;
                        player.p.draw_top = next.draw_top;
                        Debug.WriteLine($"new_player:{player.p.draw_left}=={player.p.draw_top}");
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Console.SetBufferSize(9000,500);
            Console.SetWindowSize(200,50);
            //开场动画 以及引入的部分 intro
            //PlayVideo(50);
            //Thread.Sleep(500);
           //Console.WriteLine(death);
            //Thread.Sleep(2000);
            //Console.Clear();

            //开局初始化地图
            Console.CursorVisible = false;
            Init();
            Character player = new Character(7, 7);
            //任务对话逻辑
            //对话框区域 对于cursor left 34-60 top 9-14 （包含上下限
            VoiceOver.Speak(VoiceOver.Situation.begin1);
            Thread.Sleep(3000);
            VoiceOver.Speak(VoiceOver.Situation.begin2);
            Thread.Sleep(2000);
            //主要逻辑
            SoundPlayer music;
            while (true)
            {            //不用引用 本来就是引用传递
                switch (player.map)
                {
                    case Map.map1:
                        music = new SoundPlayer();
                        music.SoundLocation = "..\\..\\Start the Adventure.wav";
                        music.Load();
                        music.Play();
                        MapDraw(trans_map_1);
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        PlayerMoveMap1(ref player);
                        music.Stop();
                        music.Dispose();
                        break;
                    case Map.map2:
                        music = new SoundPlayer();
                        music.SoundLocation = "..\\..\\boss.wav";
                        music.Load();
                        music.Play();
                        player.dir = Direction.None;
                        MapDraw(trans_map_2);
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        PlayerMoveMap2(ref player);
                        break;
                    case Map.map3:
                        if (player.fo && !player.Gui)
                            PlayerMoveFo(ref player);
                        else if (player.Gui && !player.fo)
                            PlayerMoveGui(ref player);
                        else if (player.fo && player.Gui)
                            PlayerBestEnd(ref player);
                        break;
                    case Map.map_library:
                        music = new SoundPlayer();
                        music.SoundLocation = "..\\..\\library.wav";
                        music.Load();
                        music.Play();
                        VoiceOver.Speak(VoiceOver.Situation.enterlibrary);
                        player.dir = Direction.None;
                        MapDraw(trans_map_library);
                        Console.SetCursorPosition(player.p.draw_left, player.p.draw_top);
                        Console.ForegroundColor = player.p.f_color;
                        WritePlayer(player);
                        Console.ForegroundColor = ConsoleColor.White;
                        PlayerMoveLibrary(ref player);
                        VoiceOver.ClearDialog();
                        music.Stop();
                        music.Dispose();
                        break;
                }
            }

        }
    }
}

