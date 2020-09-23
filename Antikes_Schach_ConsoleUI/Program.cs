using System;

namespace Antikes_Schach_ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class piece
    {
        public char kind { get; set; }
        //
        //great letter->white peace
        //small letter->black piece
        //
        //K . . . King
        //F . . . Ferz
        //R . . . Rook
        //N . . . Knight
        //A . . . Alfil
        //P . . . Pawn
        // 
        public Int16 x { get; set; }
        public Int16 y { get; set; }

        private bool validPeace()
        {
            if(kind=='K'|| kind == 'k' || kind == 'F' || kind == 'f' || kind == 'R' || kind == 'r' || kind == 'N' || kind == 'n' || kind == 'A' || kind == 'a' || kind == 'P' || kind == 'p')
            {
                if(x >= 0 && x <= 7 && y >= 0 && y <= 7)
                {
                    return true;
                }
            }
            return false;
        }

        private Int16 value()
        {
            switch(kind)
            {
                case 'K': return 4096;
                case 'k': return -4096;
                case 'R': return 70;
                case 'r': return -70;
                case 'N': return 5;
                case 'n': return -5;
                case 'A': return 3;
                case 'a': return -3;
                case 'F': return 3;
                case 'f': return -3;
                default: return 0;
            }
        }
    }

    public class board
    {
        char[,] squares { get; set; }
    }
}
