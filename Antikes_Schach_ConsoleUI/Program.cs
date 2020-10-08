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
                    if(Pieces[pieceToMove].tryMove(xNew,yNew))
                    {
                        Pieces[pieceToMove].x = xNew;
                        Pieces[pieceToMove].y = yNew;
                    }
                }
                else
                {
                    if(Pieces[pieceToMove].tryTake(xNew,yNew,pieceToTake))
                    {
                        Pieces[pieceToMove].x = xNew;
                        Pieces[pieceToMove].y = yNew;
                        Pieces.RemoveAt(pieceToTake);
                    }
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

        public bool tryMove(int xNew, int yNew)
        {
            if (kind == 'P')
            {
                if (piece.findSquare(xNew, yNew) == -1)
                {
                    if (yNew == y + 1 && xNew == x)
                    {
                        if (yNew == 7)
                        {
                            kind = 'F';
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (yNew == y + 1 && (xNew == x + 1 || xNew == x - 1))
                    {
                        if (yNew == 7)
                        {
                            kind = 'F';
                        }
                        return true;
                    }
                }
            }

            if (kind == 'p')
            {
                if (piece.findSquare(xNew, yNew) == -1)
                {
                    if (yNew == y - 1 && xNew == x)
                    {
                        if (yNew == 0)
                        {
                            kind = 'f';
                        }
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
                        if (yNew == 0)
                        {
                            kind = 'f';
                        }
                        return true;
                    }
                }
            }

            if (kind == 'K' || kind == 'k')
            {
                if (xNew - x < 2 && xNew - x > -2 && yNew - y < 2 && yNew - y > -2 && !(x == xNew && y == yNew))
                {
                    return true;
                }
            }

            if (kind == 'F' || kind == 'f')
            {
                if (xNew - x == 1 && yNew - y == 1 || xNew - x == 1 && yNew - y == -1 || xNew - x == -1 && yNew - y == 1 || xNew - x == -1 && yNew - y == -1)
                {
                    return true;
                }
            }

            if (kind == 'A' || kind == 'a')
            {
                if (xNew - x == 2 && yNew - y == 2 || xNew - x == 2 && yNew - y == -2 || xNew - x == -2 && yNew - y == 2 || xNew - x == -2 && yNew - y == -2)
                {
                    return true;
                }
            }

            if (kind == 'N' || kind == 'n')
            {
                if (Math.Abs(x - xNew) == 2 && Math.Abs(y - yNew) == 1 || Math.Abs(x - xNew) == 1 && Math.Abs(y - yNew) == 2)
                {
                    return true;
                }
            }

            if (kind == 'R' || kind == 'r')
            {
                if (Math.Abs(x - xNew) > 0 && y - yNew == 0 || Math.Abs(y - yNew) > 0 && x - xNew == 0)
                {
                    if (xNew > x)
                    {
                        for (int i = 1; i < xNew - x; i++)
                        {
                            if (findSquare(x + i, y) != -1)
                            {
                                return false;
                            }
                        }
                    }
                    else if (xNew < x)
                    {
                        for (int i = 1; i < x - xNew - 1; i++)
                        {
                            if (findSquare(x - i, y) != -1)
                            {
                                return false;
                            }
                        }
                    }
                    else if (yNew > y)
                    {
                        for (int i = 1; i < yNew - y; i++)
                        {
                            if (findSquare(x, y + i) != -1)
                            {
                                return false;
                            }
                        }
                    }
                    else if (yNew < y)
                    {
                        for (int i = 1; i < y - yNew - 1; i++)
                        {
                            if (findSquare(x, y - 1) != -1)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }

            return false;
        }

        public bool tryTake(int xNew, int yNew, int pieceToTake)
        {
            if(!tryMove(xNew, yNew))
            {
                return false;
            }
            if ((kind < 91) != Program.Pieces[pieceToTake].kind < 91)
            {
                return true;
            }
            return false;
        }

        public List<int[]> findPossibleMoves()
        {
            List<int[]> moves = new List<int[]>();

            int pieceTemp;

            if (kind == 'P')
            {
                //pawn moves forward
                pieceTemp = findSquare(x, y+1);
                if (pieceTemp == -1)
                {
                    if(tryMove(x, y + 1))
                    {
                        moves.Add(new int[2] { x, y + 1 });
                    }
                }
                else if(tryTake(x,y+1,pieceTemp))
                {
                    moves.Add(new int[2] { x, y + 1 });
                }
                //pawn takes to the right
                pieceTemp = findSquare(x + 1, y + 1);
                if (pieceTemp == -1)
                {
                    if (tryMove(x + 1, y + 1))
                    {
                        moves.Add(new int[2] { x + 1, y + 1 });
                    }
                }
                else if (tryTake(x, y + 1, pieceTemp))
                {
                    moves.Add(new int[2] { x + 1, y + 1 });
                }
                //pawn takes to the left
                pieceTemp = findSquare(x - 1, y + 1);
                if (pieceTemp == -1)
                {
                    if (tryMove(x - 1, y + 1))
                    {
                        moves.Add(new int[2] { x - 1, y + 1 });
                    }
                }
                else if (tryTake(x, y + 1, pieceTemp))
                {
                    moves.Add(new int[2] { x - 1, y + 1 });
                }
            }

            else if (kind == 'p')
            {
                //pawn moves forward
                pieceTemp = findSquare(x, y - 1);
                if (pieceTemp == -1)
                {
                    if (tryMove(x, y + 1))
                    {
                        moves.Add(new int[2] { x, y - 1 });
                    }
                }
                else if (tryTake(x, y + 1, pieceTemp))
                {
                    moves.Add(new int[2] { x, y - 1 });
                }
                //pawn takes to the right
                pieceTemp = findSquare(x + 1, y - 1);
                if (pieceTemp == -1)
                {
                    if (tryMove(x + 1, y - 1))
                    {
                        moves.Add(new int[2] { x + 1, y - 1 });
                    }
                }
                else if (tryTake(x, y - 1, pieceTemp))
                {
                    moves.Add(new int[2] { x + 1, y - 1 });
                }
                //pawn takes to the left
                pieceTemp = findSquare(x - 1, y - 1);
                if (pieceTemp == -1)
                {
                    if (tryMove(x - 1, y - 1))
                    {
                        moves.Add(new int[2] { x - 1, y - 1 });
                    }
                }
                else if (tryTake(x, y - 1, pieceTemp))
                {
                    moves.Add(new int[2] { x - 1, y - 1 });
                }
            }

            else if (kind == 'K' || kind == 'k')
            {
                int[] square = new int[2];
                for(int i = 0; i < 9; i++)
                {
                    square[0] = x - 1 + i / 3;
                    square[1] = y - 1 + i % 3;

                    if (square[0] != -1 && square[0] != 8 && square[0] != -1 && square[0] != 8)
                    {
                        pieceTemp = findSquare(square[0], square[1]);
                        if (pieceTemp == -1)
                        {
                            if (tryMove(square[0], square[1]))
                            {
                                moves.Add(square);
                            }
                        }
                        else if (tryTake(square[0], square[1], pieceTemp))
                        {
                            moves.Add(square);
                        }
                    }
                }

            }

            else if (kind == 'F' || kind == 'f')
            {
                int[] square = new int[2];
                for (int i = 0; i < 4; i++)
                {
                    square[0] = x + ((i / 2 == 0) ? 1 : -1);
                    square[1] = y + ((i % 2 == 0) ? 1 : -1);

                    if (square[0] != -1 && square[0] != 8 && square[0] != -1 && square[0] != 8)
                    {
                        pieceTemp = findSquare(square[0], square[1]);
                        if (pieceTemp == -1)
                        {
                            if (tryMove(square[0], square[1]))
                            {
                                moves.Add(square);
                            }
                        }
                        else if (tryTake(square[0], square[1], pieceTemp))
                        {
                            moves.Add(square);
                        }
                    }
                }
            }

            else if (kind == 'A' || kind == 'a')
            {
                int[] square = new int[2];
                for (int i = 0; i < 4; i++)
                {
                    square[0] = x + ((i / 2 == 0) ? 2 : -2);
                    square[1] = y + ((i % 2 == 0) ? 2 : -2);

                    if (square[0] != -1 && square[0] != 8 && square[0] != -1 && square[0] != 8)
                    {
                        pieceTemp = findSquare(square[0], square[1]);
                        if (pieceTemp == -1)
                        {
                            if (tryMove(square[0], square[1]))
                            {
                                moves.Add(square);
                            }
                        }
                        else if (tryTake(square[0], square[1], pieceTemp))
                        {
                            moves.Add(square);
                        }
                    }
                }
            }

            else if (kind == 'N' || kind == 'n')
            {
                int[] square = new int[2];
                for (int i = 0; i < 8; i++)
                {
                    square[0] = x + ((i % 2 == 0) ? 1 : -1) * ((i / 4 % 2 == 0) ? 1 : 2);
                    square[1] = y + ((i / 2 % 2 == 0) ? 1 : -1) * ((i / 4 % 2 == 0) ? 2 : 1);

                    if (square[0] != -1 && square[0] != 8 && square[0] != -1 && square[0] != 8)
                    {
                        pieceTemp = findSquare(square[0], square[1]);
                        if (pieceTemp == -1)
                        {
                            if (tryMove(square[0], square[1]))
                            {
                                moves.Add(square);
                            }
                        }
                        else if (tryTake(square[0], square[1], pieceTemp))
                        {
                            moves.Add(square);
                        }
                    }
                }
            }

            else if (kind == 'R' || kind == 'r')
            {
                for(int temp=0;temp<8;temp++)
                {
                    //move in x direction
                    pieceTemp = findSquare(temp, y);
                    if(pieceTemp==-1)
                    {
                        if(tryMove(temp, y))
                        {
                            moves.Add(new int[2] {temp, y});
                        }
                    }
                    else if(tryTake(temp, y, pieceTemp))
                    {
                        moves.Add(new int[2] { temp, y });
                    }
                    //move in y direction
                    pieceTemp = findSquare(x, temp);
                    if (pieceTemp == -1)
                    {
                        if (tryMove(x, temp))
                        {
                            moves.Add(new int[2] { x, temp });
                        }
                    }
                    else if (tryTake(x, temp, pieceTemp))
                    {
                        moves.Add(new int[2] { x, temp });
                    }
                }
            }

            return moves;
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
