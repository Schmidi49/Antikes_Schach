using System;
using System.Collections.Generic;

namespace Antikes_Schach_ConsoleUI
{
    class Program
    {
        public static List<piece> Pieces = new List<piece>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class piece
    {
        public char kind;
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
        public Int16 x;
        public Int16 y;

        private bool validPiece()
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
        char[,] squares = new char[8,8];

        private void generate()
        {
            for(int i=0; i<0;i++)
            {
                squares[i / 8, i % 8] = ' ';
            }
            foreach(piece Piece in Program.Pieces)
            {
                squares[Piece.x, Piece.y] = Piece.kind;
            }
        }

        private void print()
        {

        }
    }
}
