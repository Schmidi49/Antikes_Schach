using System;
using System.Collections.Generic;
using System.Text;

namespace Antikes_Schach_ConsoleUI
{
    class Program
    {
        public static List<Piece> pieces = new List<Piece>();

        static void Main(string[] args)
        {
            board Board = new board();
            startposition();
            Board.generate();
            Board.print();

            while (true)
            {
                int PieceToMove, PieceToTake, xOld=-1, yOld=-1, xNew=-1, yNew=-1;

                foreach (Piece piece in pieces)
                {
                    List<int[]> moves=piece.findPossibleMoves();
                    foreach(int[] move in moves)
                    {
                        Console.WriteLine("{0}{1}{2} - {3}{4}", piece.kind, piece.x, piece.y, move[0], move[1]);
                    }
                }


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
                    PieceToMove = Piece.findSquare(xOld, yOld);
                } while (PieceToMove == -1);


                Console.WriteLine("Destination:");
                do
                {
                    Console.Write("x: ");
                    try
                    {
                        xNew = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e) { }

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

                PieceToTake = Piece.findSquare(xNew, yNew);

                if(PieceToTake==-1)
                {
                    if(pieces[PieceToMove].tryMove(xNew,yNew))
                    {
                        pieces[PieceToMove].x = xNew;
                        pieces[PieceToMove].y = yNew;
                    }
                }
                else
                {
                    if(pieces[PieceToMove].tryTake(xNew,yNew,PieceToTake))
                    {
                        pieces[PieceToMove].x = xNew;
                        pieces[PieceToMove].y = yNew;
                        pieces.RemoveAt(PieceToTake);
                    }
                }
                
                
                Board.generate();
                Board.print();
            }
        }

        static void startposition()//gets all pieces of the startposition
        {
            pieces.Clear();
            for (int i = 0; i < 8; i++)
            {
                pieces.Add(new Piece { kind = 'p', x = i, y = 6 });
                pieces.Add(new Piece { kind = 'P', x = i, y = 1 });
            }
            //generates all pawns
            pieces.Add(new Piece { kind = 'R', x = 0, y = 0 });
            pieces.Add(new Piece { kind = 'N', x = 1, y = 0 });
            pieces.Add(new Piece { kind = 'A', x = 2, y = 0 });
            pieces.Add(new Piece { kind = 'F', x = 3, y = 0 });
            pieces.Add(new Piece { kind = 'K', x = 4, y = 0 });
            pieces.Add(new Piece { kind = 'A', x = 5, y = 0 });
            pieces.Add(new Piece { kind = 'N', x = 6, y = 0 });
            pieces.Add(new Piece { kind = 'R', x = 7, y = 0 });
            //white pieces
            pieces.Add(new Piece { kind = 'r', x = 0, y = 7 });
            pieces.Add(new Piece { kind = 'n', x = 1, y = 7 });
            pieces.Add(new Piece { kind = 'a', x = 2, y = 7 });
            pieces.Add(new Piece { kind = 'f', x = 3, y = 7 });
            pieces.Add(new Piece { kind = 'k', x = 4, y = 7 });
            pieces.Add(new Piece { kind = 'a', x = 5, y = 7 });
            pieces.Add(new Piece { kind = 'n', x = 6, y = 7 });
            pieces.Add(new Piece { kind = 'r', x = 7, y = 7 });
            //black pieces
        }
    }

    public class Piece
    {
        public char kind { get; set; }
        //
        //great letter->white peace
        //small letter->black Piece
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

        //functions for one Piece
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
                if (Piece.findSquare(xNew, yNew) == -1)
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
                if (Piece.findSquare(xNew, yNew) == -1)
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
                        for (int i = 1; i < x - xNew; i++)
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
                        for (int i = 1; i < y - yNew; i++)
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

        public bool tryTake(int xNew, int yNew, int PieceToTake)
        {
            if(!tryMove(xNew, yNew))
            {
                return false;
            }
            if ((kind < 91) != Program.pieces[PieceToTake].kind < 91)
            {
                return true;
            }
            return false;
        }

        public List<int[]> findPossibleMoves()
        {
            List<int[]> moves = new List<int[]>();

            int PieceTemp;

            if (kind == 'P')
            {
                //pawn moves forward
                PieceTemp = findSquare(x, y+1);
                if (PieceTemp == -1)
                {
                    if(tryMove(x, y + 1))
                    {
                        moves.Add(new int[2] { x, y + 1 });
                    }
                }
                else if(tryTake(x,y+1,PieceTemp))
                {
                    moves.Add(new int[2] { x, y + 1 });
                }
                //pawn takes to the right
                PieceTemp = findSquare(x + 1, y + 1);
                if (PieceTemp == -1)
                {
                    if (tryMove(x + 1, y + 1))
                    {
                        moves.Add(new int[2] { x + 1, y + 1 });
                    }
                }
                else if (tryTake(x, y + 1, PieceTemp))
                {
                    moves.Add(new int[2] { x + 1, y + 1 });
                }
                //pawn takes to the left
                PieceTemp = findSquare(x - 1, y + 1);
                if (PieceTemp == -1)
                {
                    if (tryMove(x - 1, y + 1))
                    {
                        moves.Add(new int[2] { x - 1, y + 1 });
                    }
                }
                else if (tryTake(x, y + 1, PieceTemp))
                {
                    moves.Add(new int[2] { x - 1, y + 1 });
                }
            }

            else if (kind == 'p')
            {
                //pawn moves forward
                PieceTemp = findSquare(x, y - 1);
                if (PieceTemp == -1)
                {
                    if (tryMove(x, y + 1))
                    {
                        moves.Add(new int[2] { x, y - 1 });
                    }
                }
                else if (tryTake(x, y + 1, PieceTemp))
                {
                    moves.Add(new int[2] { x, y - 1 });
                }
                //pawn takes to the right
                PieceTemp = findSquare(x + 1, y - 1);
                if (PieceTemp == -1)
                {
                    if (tryMove(x + 1, y - 1))
                    {
                        moves.Add(new int[2] { x + 1, y - 1 });
                    }
                }
                else if (tryTake(x, y - 1, PieceTemp))
                {
                    moves.Add(new int[2] { x + 1, y - 1 });
                }
                //pawn takes to the left
                PieceTemp = findSquare(x - 1, y - 1);
                if (PieceTemp == -1)
                {
                    if (tryMove(x - 1, y - 1))
                    {
                        moves.Add(new int[2] { x - 1, y - 1 });
                    }
                }
                else if (tryTake(x, y - 1, PieceTemp))
                {
                    moves.Add(new int[2] { x - 1, y - 1 });
                }
            }

            else if (kind == 'K' || kind == 'k')
            {
                int xNew, yNew;
                for (int i = 0; i < 9; i++)
                {
                    xNew = x - 1 + i / 3;
                    yNew = y - 1 + i % 3;

                    if (xNew != -1 && xNew != 8 && yNew != -1 && yNew != 8)
                    {
                        PieceTemp = findSquare(xNew, yNew);
                        if (PieceTemp == -1)
                        {
                            if (tryMove(xNew, yNew))
                            {
                                moves.Add(new int[] { xNew, yNew });
                            }
                        }
                        else if (tryTake(xNew, yNew, PieceTemp))
                        {
                            moves.Add(new int[] { xNew, yNew });
                        }
                    }
                }

            }

            else if (kind == 'F' || kind == 'f')
            {
                int xNew, yNew;
                for (int i = 0; i < 4; i++)
                {
                    xNew = x + ((i / 2 == 0) ? 1 : -1);
                    yNew = y + ((i % 2 == 0) ? 1 : -1);

                    if (xNew != -1 && xNew != 8 && yNew != -1 && yNew != 8)
                    {
                        PieceTemp = findSquare(xNew, yNew);
                        if (PieceTemp == -1)
                        {
                            if (tryMove(xNew, yNew))
                            {
                                moves.Add(new int[] { xNew, yNew });
                            }
                        }
                        else if (tryTake(xNew, yNew, PieceTemp))
                        {
                            moves.Add(new int[] { xNew, yNew });
                        }
                    }
                }
            }

            else if (kind == 'A' || kind == 'a')
            {
                int xNew, yNew;
                for (int i = 0; i < 4; i++)
                {
                    xNew = x + ((i / 2 == 0) ? 2 : -2);
                    yNew = y + ((i % 2 == 0) ? 2 : -2);

                    if (xNew > -1 && xNew < 8 && yNew > -1 && yNew < 8)
                    {
                        PieceTemp = findSquare(xNew, yNew);
                        if (PieceTemp == -1)
                        {
                            if (tryMove(xNew, yNew))
                            {
                                moves.Add(new int[] { xNew, yNew });
                            }
                        }
                        else if (tryTake(xNew, yNew, PieceTemp))
                        {
                            moves.Add(new int[] { xNew, yNew });
                        }
                    }
                }
            }

            else if (kind == 'N' || kind == 'n')
            {
                for (int i = 0; i < 8; i++)
                {
                    int xNew, yNew;
                    xNew = x + ((i % 2 == 0) ? 1 : -1) * ((i / 4 % 2 == 0) ? 1 : 2);
                    yNew = y + ((i / 2 % 2 == 0) ? 1 : -1) * ((i / 4 % 2 == 0) ? 2 : 1);

                    if (xNew > -1 && xNew < 8 && yNew > -1 && yNew < 8)
                    {
                        PieceTemp = findSquare(xNew, yNew);
                        if (PieceTemp == -1)
                        {
                            if (tryMove(xNew, yNew))
                            {
                                moves.Add(new int[] { xNew, yNew });
                            }
                        }
                        else if (tryTake(xNew, yNew, PieceTemp))
                        {
                            moves.Add(new int[] { xNew, yNew });
                        }
                    }
                }
            }

            else if (kind == 'R' || kind == 'r')
            {
                for(int temp=0;temp<8;temp++)
                {
                    //move in x direction
                    PieceTemp = findSquare(temp, y);
                    if(PieceTemp==-1)
                    {
                        if(tryMove(temp, y))
                        {
                            moves.Add(new int[2] {temp, y});
                        }
                    }
                    else if(tryTake(temp, y, PieceTemp))
                    {
                        moves.Add(new int[2] { temp, y });
                    }
                    //move in y direction
                    PieceTemp = findSquare(x, temp);
                    if (PieceTemp == -1)
                    {
                        if (tryMove(x, temp))
                        {
                            moves.Add(new int[2] { x, temp });
                        }
                    }
                    else if (tryTake(x, temp, PieceTemp))
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
            for(int i=0;i< Program.pieces.Count;i++)
            {
                if(x == Program.pieces[i].x && y == Program.pieces[i].y)
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
            foreach(Piece Piece in Program.pieces)
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
        }//prints the Piece on a grid onscreen

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

    public class Move
    {
        public int xOld { get; set; }

        public int yOld { get; set; }

        public int xNew { get; set; }

        public int yNew { get; set; }

        public int piece { get; set; }

        public int pieceToTake { get; set; }

        public List<Piece> getPosition(List<Piece> pieces, List<Move> moves)
        {
            foreach(Move move in moves)
            {

            }
            return pieces;
        }
    }
}
