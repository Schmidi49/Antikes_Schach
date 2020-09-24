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

            while (true)
            {
                int pieceToMove, pieceToTake, xOld=-1, yOld=-1, xNew=-1, yNew=-1;

                do
                {
                    do
                    {
                        Console.Write("x: ");
                        try
                        {
                            xOld = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception e) { }
                    }
                    while (xOld < 0 || xOld > 7);
                    do
                    {
                        Console.Write("y: ");
                        try
                        {
                            yOld = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception e) { }
                    }
                    while (yOld < 0 || yOld > 7);
                    pieceToMove = piece.findSquare(xOld, yOld);
                } while (pieceToMove == -1);


                Console.WriteLine("Destination:");
                do
                {
                    Console.Write("x: ");
                    try
                    {
                        xNew = Convert.ToInt32(Console.ReadLine());
                    }
                    catch(Exception e) { }

                }
                while (xNew < 0 || xNew > 7);
                do
                {
                    Console.Write("y: ");
                    try
                    {
                        yNew = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e) { }
                }
                while (yNew < 0 || yNew > 7);

                pieceToTake = piece.findSquare(xNew, yNew);
                if(pieceToTake==-1)
                {
                    Pieces[pieceToMove].move(xNew, yNew, pieceToTake);
                }
                else
                {
                    Pieces[pieceToMove].move(xNew, yNew, pieceToTake);
                }
                
                Board.generate();
                Board.print();
            }
        }

        static void startposition()//gets all pieces of the startposition
        {
            Pieces.Clear();
            for (int i = 0; i < 8; i++)
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
        public int x { get; set; }
        public int y { get; set; }

        public int value()
        {            
            switch (kind)
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

        //functions for one piece
        public bool validPiece()
        {
            if (kind == 'K' || kind == 'k' || kind == 'F' || kind == 'f' || kind == 'R' || kind == 'r' || kind == 'N' || kind == 'n' || kind == 'A' || kind == 'a' || kind == 'P' || kind == 'p')
            {
                if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
                {
                    return true;
                }
            }
            return false;
        }

        public bool move(int xNew, int yNew, int pieceToTake)
        {
            if (kind=='P')
            {
                if(piece.findSquare(xNew, yNew)==-1)
                {
                    if(yNew == y + 1 && xNew == x)
                    {
                        if (yNew == 7)
                        {
                            kind = 'F';
                        }
                        y++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if(yNew == y + 1 && (xNew == x + 1 || xNew == x - 1))
                    {
                        if(!take(pieceToTake))
                        {
                            return false;
                        }
                        if (yNew == 7)
                        {
                            kind = 'F';
                        }
                        x = xNew;
                        y = yNew;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if(kind=='p')
            {
                if (piece.findSquare(xNew, yNew) == -1)
                {
                    if (yNew == y - 1 && xNew == x)
                    {
                        if (yNew == 0)
                        {
                            kind = 'f';
                        }
                        y--;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (yNew == y - 1 && (xNew == x + 1 || xNew == x - 1))
                    {
                        if (!take(pieceToTake))
                        {
                            return false;
                        }
                        if (yNew == 0)
                        {
                            kind = 'f';
                        }
                        x = xNew;
                        y = yNew;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if(kind=='K'||kind=='k')
            {
                if(xNew - x < 2 && xNew - x > -2 && yNew - y < 2 && yNew - y > -2)
                {
                    if (pieceToTake != -1)
                    {
                        if (!take(pieceToTake))
                        {
                            return false;
                        }
                    }
                    x = xNew;
                    y = yNew;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if(kind=='F'||kind=='f')
            {
                if (xNew - x == 1 && yNew - y == 1 || xNew - x == 1 && yNew - y == -1 || xNew - x == -1 && yNew - y == 1 || xNew - x == -1 && yNew - y == -1)
                {
                    if (pieceToTake != -1)
                    {
                        if (!take(pieceToTake))
                        {
                            return false;
                        }
                    }
                    x = xNew;
                    y = yNew;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            x = xNew;
            y = yNew;
            if (pieceToTake!=-1)
            {
                Program.Pieces.RemoveAt(pieceToTake);
            }
            return true;
        }

        public bool take(int pieceToTake)
        {
            if((kind<91)!=Program.Pieces[pieceToTake].kind<91)
            {
                Program.Pieces.RemoveAt(pieceToTake);
                return true;
            }
            return false;
        }

        //functions for all existing pieces
        public static int findSquare(int x, int y)
        {
            for(int i=0;i< Program.Pieces.Count;i++)
            {
                if(x == Program.Pieces[i].x && y == Program.Pieces[i].y)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public class board
    {
        public char[,] squares = new char[8, 8];

        public void generate()
        {
            for(int i=0; i<64;i++)
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
