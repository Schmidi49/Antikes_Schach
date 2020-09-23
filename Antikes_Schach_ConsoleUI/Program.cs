using System;
using System.Collections.Generic;
using System.Text;

namespace Antikes_Schach_ConsoleUI
{
    class Program
    {
        public static List<piece> Pieces = new List<piece>();

        static void Main(string[] args)
        {
            board Board = new board();
            startposition();
            Board.generate();
            Board.print();
            Console.ReadKey();
        }

        static void startposition()//gets all pieces of the startposition
        {
            Pieces.Clear();
            for(int i=0;i<8;i++)
            {
                Pieces.Add(new piece { kind = 'p', x = i, y = 6 });
                Pieces.Add(new piece { kind = 'P', x = i, y = 1 });
            }
            //generates all pawns
            Pieces.Add(new piece { kind = 'R', x = 0, y = 0 });
            Pieces.Add(new piece { kind = 'N', x = 1, y = 0 });
            Pieces.Add(new piece { kind = 'A', x = 2, y = 0 });
            Pieces.Add(new piece { kind = 'F', x = 3, y = 0 });
            Pieces.Add(new piece { kind = 'K', x = 4, y = 0 });
            Pieces.Add(new piece { kind = 'A', x = 5, y = 0 });
            Pieces.Add(new piece { kind = 'N', x = 6, y = 0 });
            Pieces.Add(new piece { kind = 'R', x = 7, y = 0 });
            //white pieces
            Pieces.Add(new piece { kind = 'r', x = 0, y = 7 });
            Pieces.Add(new piece { kind = 'n', x = 1, y = 7 });
            Pieces.Add(new piece { kind = 'a', x = 2, y = 7 });
            Pieces.Add(new piece { kind = 'f', x = 3, y = 7 });
            Pieces.Add(new piece { kind = 'k', x = 4, y = 7 });
            Pieces.Add(new piece { kind = 'a', x = 5, y = 7 });
            Pieces.Add(new piece { kind = 'n', x = 6, y = 7 });
            Pieces.Add(new piece { kind = 'r', x = 7, y = 7 });
            //black pieces
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
        public int x;
        public int y;

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

        private int value()
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
        char[,] squares = new char[8, 8];

        public void generate()
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

        public void print()
        {
            Console.Clear();
            Console.WriteLine(topFrame());

            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine(inbetweenLine());
                Console.WriteLine(dataLine(7 - i));
                Console.WriteLine(inbetweenLine());
                Console.WriteLine(middleFrame());
            }

            Console.WriteLine(inbetweenLine());
            Console.WriteLine(dataLine(0));
            Console.WriteLine(inbetweenLine());
            Console.WriteLine(botFrame());
            //the last row works different
        }//prints the piece on a grid onscreen

        string topFrame()
        {
            string s = "";
            s += '\u2554';
            for (int j = 0; j < 7; j++)
            {
                for(int i=0;i<7;i++)
                {
                    s += '\u2550';
                }
                s +='\u2566';
            }
            for (int i = 0; i < 7; i++)
            {
                s += '\u2550';
            }
            s += '\u2557';
            return s;
        }//generates the top frame of the console grid

        string middleFrame()
        {
            string s = "";
            s += '\u2560';
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    s += '\u2550';
                }
                s += '\u256C';
            }
            for (int i = 0; i < 7; i++)
            {
                s += '\u2550';
            }
            s += '\u2563';
            return s;
        }//generates the bot frame of the console grid

        string botFrame()
        {
            string s = "";
            s += '\u255A';
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    s += '\u2550';
                }
                s += '\u2569';
            }
            for (int i = 0; i < 7; i++)
            {
                s += '\u2550';
            }
            s += '\u255D';
            return s;
        }//generates the vertical lines frame of the console grid

        string inbetweenLine()
        {
            string s = "";
            s += '\u2551';
            for (int j = 0; j < 8; j++)
            {
                s += "       " + '\u2551';
            }
            return s;
        }//generates a vertikal, empty lines of the console grid

        string dataLine(int row)
        {
            string s = "";
            s += '\u2551';
            for (int j = 0; j < 8; j++)
            {
                s += "   ";
                s += squares[j,row];
                s += "   ";
                s += '\u2551';
            }
            return s;
        }//generates a line with the pieces in it

    }
}
