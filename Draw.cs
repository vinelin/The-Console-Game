using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    //光标是英文字符的光标，所以draw_left要乘2;
    struct Point
    {
        public int x;
        public int y;
        public int draw_left;
        public int draw_top;
        public ConsoleColor f_color;
        public ConsoleColor b_color;
        public string content;

        public Point(int x, int y, ConsoleColor color,ConsoleColor bgcolor,string c)
        {
            this.x = x;
            this.y = y;
            draw_left = x * 2;
            draw_top = y;
            this.f_color = color;
            this.b_color = bgcolor;
            content = c;
        }
        public Point(Point p )
        {
            this.x = p.x;
            this.y = p.y;
            draw_left = p.x * 2;
            draw_top = p.y;
            this.f_color = p.f_color;
            this.b_color = p.b_color;
            content = p.content;
        }
        public void Draw()
        {
            Console.SetCursorPosition(draw_left, draw_top);
            Console.ForegroundColor = f_color;
            Console.BackgroundColor = b_color;
            Console.Write(content);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

    }


    class PrintControl
    {
        public static void DrawHouse(int x, int y, Point[,] trans_map)
        {

            trans_map[y, x / 2] = new Point(x / 2, y, ConsoleColor.White,ConsoleColor.DarkGreen, "▃");
            trans_map[y, (x / 2) + 1] = new Point((x / 2) + 1, y, ConsoleColor.White, ConsoleColor.DarkGreen, "▅");
            trans_map[y, (x / 2) + 2] = new Point((x / 2) + 2, y, ConsoleColor.White, ConsoleColor.DarkGreen, "▅");
            trans_map[y, (x / 2) + 3] = new Point((x / 2) + 3, y, ConsoleColor.White, ConsoleColor.DarkGreen, "▃");
            trans_map[y+1, (x / 2) + 0] = new Point((x / 2) + 0, y+1, ConsoleColor.White, ConsoleColor.Black, "█");
            trans_map[y+1, (x / 2) + 1] = new Point((x / 2) + 1, y+1, ConsoleColor.DarkYellow, ConsoleColor.Black, "█");
            trans_map[y+1, (x / 2) + 2] = new Point((x / 2) + 2, y+1, ConsoleColor.DarkYellow, ConsoleColor.Black, "█");
            trans_map[y+1, (x / 2) + 3] = new Point((x / 2) + 3, y+1, ConsoleColor.White, ConsoleColor.Black, "█");
            //Console.SetCursorPosition(x, y);
            //Console.Write("▃");
            //Console.SetCursorPosition(x+2, y);
            //Console.Write("▅");
            //Console.SetCursorPosition(x + 4, y);
            //Console.Write("▅");
            //Console.SetCursorPosition(x + 6, y);
            //Console.Write("▃");
            //Console.SetCursorPosition(x, y + 1);
            //Console.Write("█");
            //Console.ForegroundColor = ConsoleColor.DarkYellow;
            //Console.SetCursorPosition(x+2, y + 1);
            //Console.Write("█");
            //Console.SetCursorPosition(x + 4, y + 1);
            //Console.Write("█");
            //Console.ForegroundColor = ConsoleColor.White;
            //Console.SetCursorPosition(x + 6, y + 1);
            //Console.Write("█");
            //Console.SetCursorPosition(x + 2, y);
        }
        public static void DrawTemple(int x, int y, Point[,] trans_map)
        {
            trans_map[y, (x / 2)] = new Point((x / 2), y, ConsoleColor.Yellow, ConsoleColor.DarkGreen, "卍");
            ////Console.ForegroundColor = ConsoleColor.Yellow;
            ////Console.SetCursorPosition(x, y);
            ////Console.Write("卍");
            ////Console.BackgroundColor = ConsoleColor.Black;
            ////Console.ForegroundColor = ConsoleColor.White;
            ////Console.SetCursorPosition(x + 2, y);
        }

    }
}
