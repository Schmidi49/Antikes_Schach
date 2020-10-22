using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Antikes_Schach_ConsoleUI
{
    class Program
    {
        private static Move curMove = new Move();
        private static int xOld = -1, yOld = -1, xNew = -1, yNew = -1;

        static void Main(string[] args)
        {
            Gamestate.cur.getStartposition();
            Board.generate();
            Board.print();

            while (true)
            {
                string a;
                bool inputOK;

                do
                {
                    inputOK = true;
                    Console.Write("Move:");
                    a = Console.ReadLine();
                    if (a.Length < 4)
                    {
                        Console.WriteLine("Input to short!");
                        inputOK = false;
                    }
                }
                while (!inputOK);

                if(a=="load")
                {
                    load();
                }
                else if(a=="save")
                {
                    save();
                }
                else if(a=="exit")
                {
                    exit();
                }
                else
                {
                    move(a);
                }

                Board.generate();
                Board.print();
            }
        }

        private static void move(string s)
        {
            try
            {
                xOld = s[0] - 97;
                yOld = s[1] - 49;
                xNew = s[2] - 97;
                yNew = s[3] - 49;

                curMove.getMove(xOld, yOld, xNew, yNew);

                if (curMove.tryMove())
                {
                    curMove.execute();
                }
                else
                {
                    Console.WriteLine("Illegal Move!\nPress any key to continue!");
                    Console.ReadKey();
                }
            }
            catch(Exception e)
            {
                Console.Write("Illeagal input! Exception: {0}\nPress any key to continue!", e);
                Console.ReadKey();
            }
        }

        private static void load()
        {
            string input;
            Console.Write("FEN: ");
            input = Console.ReadLine();

            if (Gamestate.cur.getFEN(input))
            {
                Console.WriteLine("FEN load succesfully");
                Console.WriteLine("Press any Key to continue!");
                Console.ReadKey();
            }
        }

        private static void save()
        {
            Console.WriteLine("Your current FEN:");
            Console.WriteLine(Gamestate.cur.genFEN());
            Console.WriteLine("Do you want to continue? [y/n]");
            char c;
            do
            {
                c = Console.ReadKey().KeyChar;
            }
            while (c != 'y' && c != 'n');
            if (c == 'n')
            {
                Environment.Exit(0);
            }
        }

        private static void exit()
        {
            Console.WriteLine("Do you want to exit? [y/n]");
            char c;
            do
            {
                c = Console.ReadKey().KeyChar;
            }
            while (c != 'y' && c != 'n');
            if (c == 'y')
            {
                Environment.Exit(0);
            }
        }

    }

    public class Board
    {
        public static char[,] squares = new char[8, 8];

        public static void generate()
        {
            for (int i = 0; i < 64; i++)
            {
                squares[i / 8, i % 8] = ' ';
            }
            foreach (Piece Piece in Gamestate.cur.pieces)
            {
                squares[Piece.x, Piece.y] = Piece.kind;
            }
        }

        public static void print()
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

        private static string topFrame()
        {
            string s = "";
            s += '\u2554';
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    s += '\u2550';
                }
                s += '\u2566';
            }
            for (int i = 0; i < 7; i++)
            {
                s += '\u2550';
            }
            s += '\u2557';
            return s;
        }//generates the top frame of the console grid

        private static string middleFrame()
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

        private static string botFrame()
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

        private static string inbetweenLine()
        {
            string s = "";
            s += '\u2551';
            for (int j = 0; j < 8; j++)
            {
                s += "       " + '\u2551';
            }
            return s;
        }//generates a vertikal, empty lines of the console grid

        private static string dataLine(int row)
        {
            string s = "";
            s += '\u2551';
            for (int j = 0; j < 8; j++)
            {
                s += "   ";
                s += squares[j, row];
                s += "   ";
                s += '\u2551';
            }
            return s;
        }//generates a line with the pieces in it

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

        public bool colour()
        {
            if(kind<91)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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

        public List<Move> findPossibleMoves()
        {
            List<Move> moves = new List<Move>();

            int pieceNum = Gamestate.cur.findPiece(x, y);

            if (kind == 'P')
            {
                //pawn takes to the left
                Move move0 = Move.genMove(x, y, x - 1, y + 1);
                if (move0.tryMove())
                {
                    moves.Add(move0);
                }
                //pawn moves forward
                Move move1 = Move.genMove(x, y, x, y + 1);
                if (move1.tryMove())
                {
                    moves.Add(move1);
                }
                //pawn takes to the right
                Move move2 = Move.genMove(x, y, x + 1, y + 1);
                if (move2.tryMove())
                {
                    moves.Add(move2);
                }
            }

            else if (kind == 'p')
            {
                //pawn takes to the left
                Move move0 = Move.genMove(x, y, x - 1, y - 1);
                if (move0.tryMove())
                {
                    moves.Add(move0);
                }
                //pawn moves forward
                Move move1 = Move.genMove(x, y, x, y - 1);
                if (move1.tryMove())
                {
                    moves.Add(move1);
                }
                //pawn takes to the right
                Move move2 = Move.genMove(x, y, x + 1, y - 1);
                if (move2.tryMove())
                {
                    moves.Add(move2);
                }
            }

            else if (kind == 'K' || kind == 'k')
            {
                int xNew, yNew;
                for (int i = 0; i < 9; i++)
                {
                    xNew = x - 1 + i / 3;
                    yNew = y - 1 + i % 3;

                    Move move = Move.genMove(x, y, xNew, yNew);
                    if (move.tryMove())
                    {
                        moves.Add(move);
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

                    Move move = Move.genMove(x, y, xNew, yNew);
                    if (move.tryMove())
                    {
                        moves.Add(move);
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

                    Move move = Move.genMove(x, y, xNew, yNew);
                    if (move.tryMove())
                    {
                        moves.Add(move);
                    }
                }
            }

            else if (kind == 'N' || kind == 'n')
            {

                int xNew, yNew;
                for (int i = 0; i < 8; i++)
                {
                    xNew = x + ((i % 2 == 0) ? 1 : -1) * ((i / 4 % 2 == 0) ? 1 : 2);
                    yNew = y + ((i / 2 % 2 == 0) ? 1 : -1) * ((i / 4 % 2 == 0) ? 2 : 1);

                    Move move = Move.genMove(x, y, xNew, yNew);
                    if (move.tryMove())
                    {
                        moves.Add(move);
                    }
                }
            }

            else if (kind == 'R' || kind == 'r')
            {
                for (int temp = 0; temp < 8; temp++)
                {
                    //move in x direction
                    Move movex = Move.genMove(x, y, temp, y);
                    if (movex.tryMove())
                    {
                        moves.Add(movex);
                    }
                    //move in y direction
                    Move movey = Move.genMove(x, y, x, temp);
                    if (movey.tryMove())
                    {
                        moves.Add(movey);
                    }
                }
            }

            return moves;
        }
    }

    public class Gamestate
    {
        public static Gamestate cur = new Gamestate { pieces = new List<Piece>(), moveorder = new List<Move>() };

        public List<Piece> pieces { get; set; }

        public List<Move> moveorder { get; set; }

        public void getStartposition()
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
        }

        public int findPiece(int x, int y)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (x == pieces[i].x && y == pieces[i].y)
                {
                    return i;
                }
            }
            return -1;
        }

        public string genFEN()
        {
            StringBuilder FEN = new StringBuilder();
            int count = 0, stringPos = 0; ;
            int piece;

            for(int y=7;y>-1;y--)
            {
                for(int x=0;x<8;x++)
                {
                    piece = findPiece(x, y);
                    if (piece==-1)
                    {
                        count++;
                    }
                    else
                    {
                        if(count!=0)
                        {
                            FEN.Insert(stringPos, count.ToString()[0]);
                            stringPos++;
                            count = 0;
                        }
                        FEN.Insert(stringPos, pieces[piece].kind);
                        stringPos++;
                    }
                }
                if (count != 0)
                {
                    FEN.Insert(stringPos, count.ToString()[0]);
                    stringPos++;
                    count = 0;
                }
                FEN.Insert(stringPos, '/');
                stringPos++;
            }

            FEN[stringPos - 1] = ' ';

            if(moveorder.Count%2==0)
            {
                FEN.Insert(stringPos, 'w');
                stringPos++;
            }
            else
            {
                FEN.Insert(stringPos, 'b');
                stringPos++;
            }

            return FEN.ToString();
        }

        public bool getFEN(string FEN)
        {
            Gamestate temp = new Gamestate { pieces = new List<Piece>(), moveorder = new List<Move>() };

            string[] splitLine = FEN.Split('/', ' ');
            int count = 0;
            bool whiteKing = false, blackKing = false;
            char[] posPieces = new char[] { 'p', 'r', 'n', 'a', 'f', 'k', 'P', 'R', 'N', 'A', 'F', 'K' };

            if (splitLine.Length < 9)
            {
                return false;
            }

            for (int i = 0; i < 8; i++)
            {
                Regex rx = new Regex(@"[prnafk12345678]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(splitLine[i]);

                if (matches.Count == splitLine[i].Length)
                {
                    for (int j = 0; j < splitLine[i].Length; j++)
                    {
                        if (47 < splitLine[i][j] && splitLine[i][j] < 58)
                        {
                            count += splitLine[i][j] - 48;
                        }
                        else
                        {
                            temp.pieces.Add(new Piece { kind = splitLine[i][j], x = count, y = 7 - i });
                            count++;
                            if (splitLine[i][j] == 'K')
                            {
                                whiteKing = true;
                            }
                            else if (splitLine[i][j] == 'k')
                            {
                                blackKing = true;
                            }
                        }

                        if (count > 8 || (count == 8 && !(j == splitLine[i].Length - 1)))
                        {
                            ;
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }

                count = 0;
            }

            if (!(whiteKing && blackKing))
            {
                return false;
            }

            if (splitLine[8] == "b")
            {
                temp.moveorder.Add(new Move());
            }
            else if (splitLine[8] != "w")
            {
                return false;
            }

            Gamestate.cur = temp;
            return true;
        }
    }

    public class Move
    {
        public int x { get; set; }

        public int y { get; set; }

        public int pieceToMove { get; set; }

        public int pieceToTake { get; set; }

        public void getMove(int xOld, int yOld, int xNew, int yNew)
        {
            pieceToMove = Gamestate.cur.findPiece(xOld, yOld);
            pieceToTake = Gamestate.cur.findPiece(xNew, yNew);

            x = xNew;
            y = yNew;
        }

        public static Move genMove(int xOld, int yOld, int xNew, int yNew)
        {
            Move move = new Move();

            move.pieceToMove = Gamestate.cur.findPiece(xOld, yOld);
            move.pieceToTake = Gamestate.cur.findPiece(xNew, yNew);

            move.x = xNew;
            move.y = yNew;

            return move;
        }

        public bool tryMove()
        {
            if (pieceToMove == -1)
            {
                return false;
            }

            if (Gamestate.cur.pieces[pieceToMove].colour() == (Gamestate.cur.moveorder.Count % 2 == 0) ? false : true)
            {
                return false;
            }

            if(pieceToTake!=-1)
            {
                if(Gamestate.cur.pieces[pieceToMove].colour()== Gamestate.cur.pieces[pieceToTake].colour())
                {
                    return false;
                }
            }

            if (!(x > -1 && x < 8 && y > -1 && y < 8))
            {
                return false;
            }

            if (Gamestate.cur.pieces[pieceToMove].kind == 'P')
            {
                if (pieceToTake == -1)
                {
                    if (y == Gamestate.cur.pieces[pieceToMove].y + 1 && x == Gamestate.cur.pieces[pieceToMove].x)
                    {
                        if (Gamestate.cur.pieces[pieceToMove].y == 7)
                        {
                            Gamestate.cur.pieces[pieceToMove].kind = 'F';
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
                    if (y == Gamestate.cur.pieces[pieceToMove].y + 1 && (x == Gamestate.cur.pieces[pieceToMove].x + 1 || x == Gamestate.cur.pieces[pieceToMove].x - 1))
                    {
                        if (y == 7)
                        {
                            Gamestate.cur.pieces[pieceToMove].kind = 'F';
                        }
                        return true;
                    }
                }
            }

            if (Gamestate.cur.pieces[pieceToMove].kind == 'p')
            {
                if (pieceToTake == -1)
                {
                    if (y == Gamestate.cur.pieces[pieceToMove].y - 1 && x == Gamestate.cur.pieces[pieceToMove].x)
                    {
                        if (y == 0)
                        {
                            Gamestate.cur.pieces[pieceToMove].kind = 'f';
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
                    if (y == Gamestate.cur.pieces[pieceToMove].y - 1 && (x == Gamestate.cur.pieces[pieceToMove].x + 1 || x == Gamestate.cur.pieces[pieceToMove].x - 1))
                    {
                        if (y == 0)
                        {
                            Gamestate.cur.pieces[pieceToMove].kind = 'f';
                        }
                        return true;
                    }
                }
            }

            if (Gamestate.cur.pieces[pieceToMove].kind == 'K' || Gamestate.cur.pieces[pieceToMove].kind == 'k')
            {
                if (x - Gamestate.cur.pieces[pieceToMove].x < 2 && x - Gamestate.cur.pieces[pieceToMove].x > -2 && y - Gamestate.cur.pieces[pieceToMove].y < 2 && y - Gamestate.cur.pieces[pieceToMove].y > -2 && !(Gamestate.cur.pieces[pieceToMove].x == x && Gamestate.cur.pieces[pieceToMove].y == y))
                {
                    return true;
                }
            }

            if (Gamestate.cur.pieces[pieceToMove].kind == 'F' || Gamestate.cur.pieces[pieceToMove].kind == 'f')
            {
                if (x - Gamestate.cur.pieces[pieceToMove].x == 1 && y - Gamestate.cur.pieces[pieceToMove].y == 1 || x - Gamestate.cur.pieces[pieceToMove].x == 1 && y - Gamestate.cur.pieces[pieceToMove].y == -1 || x - Gamestate.cur.pieces[pieceToMove].x == -1 && y - Gamestate.cur.pieces[pieceToMove].y == 1 || x - Gamestate.cur.pieces[pieceToMove].x == -1 && y - Gamestate.cur.pieces[pieceToMove].y == -1)
                {
                    return true;
                }
            }

            if (Gamestate.cur.pieces[pieceToMove].kind == 'A' || Gamestate.cur.pieces[pieceToMove].kind == 'a')
            {
                if (x - Gamestate.cur.pieces[pieceToMove].x == 2 && y - Gamestate.cur.pieces[pieceToMove].y == 2 || x - Gamestate.cur.pieces[pieceToMove].x == 2 && y - Gamestate.cur.pieces[pieceToMove].y == -2 || x - Gamestate.cur.pieces[pieceToMove].x == -2 && y - Gamestate.cur.pieces[pieceToMove].y == 2 || x - Gamestate.cur.pieces[pieceToMove].x == -2 && y - Gamestate.cur.pieces[pieceToMove].y == -2)
                {
                    return true;
                }
            }

            if (Gamestate.cur.pieces[pieceToMove].kind == 'N' || Gamestate.cur.pieces[pieceToMove].kind == 'n')
            {
                if (Math.Abs(Gamestate.cur.pieces[pieceToMove].x - x) == 2 && Math.Abs(Gamestate.cur.pieces[pieceToMove].y - y) == 1 || Math.Abs(Gamestate.cur.pieces[pieceToMove].x - x) == 1 && Math.Abs(Gamestate.cur.pieces[pieceToMove].y - y) == 2)
                {
                    return true;
                }
            }

            if (Gamestate.cur.pieces[pieceToMove].kind == 'R' || Gamestate.cur.pieces[pieceToMove].kind == 'r')
            {
                if (Math.Abs(Gamestate.cur.pieces[pieceToMove].x - x) > 0 && Gamestate.cur.pieces[pieceToMove].y - y == 0 || Math.Abs(Gamestate.cur.pieces[pieceToMove].y - y) > 0 && Gamestate.cur.pieces[pieceToMove].x - x == 0)
                {
                    if (x > Gamestate.cur.pieces[pieceToMove].x)
                    {
                        for (int i = 1; i < x - Gamestate.cur.pieces[pieceToMove].x; i++)
                        {
                            if (Gamestate.cur.findPiece(Gamestate.cur.pieces[pieceToMove].x + i, Gamestate.cur.pieces[pieceToMove].y) != -1)
                            {
                                return false;
                            }
                        }
                    }
                    else if (x < Gamestate.cur.pieces[pieceToMove].x)
                    {
                        for (int i = 1; i < Gamestate.cur.pieces[pieceToMove].x - x; i++)
                        {
                            if (Gamestate.cur.findPiece(Gamestate.cur.pieces[pieceToMove].x - i, Gamestate.cur.pieces[pieceToMove].y) != -1)
                            {
                                return false;
                            }
                        }
                    }
                    else if (y > Gamestate.cur.pieces[pieceToMove].y)
                    {
                        for (int i = 1; i < y - Gamestate.cur.pieces[pieceToMove].y; i++)
                        {
                            if (Gamestate.cur.findPiece(Gamestate.cur.pieces[pieceToMove].x, Gamestate.cur.pieces[pieceToMove].y + i) != -1)
                            {
                                return false;
                            }
                        }
                    }
                    else if (y < Gamestate.cur.pieces[pieceToMove].y)
                    {
                        for (int i = 1; i < Gamestate.cur.pieces[pieceToMove].y - y; i++)
                        {
                            if (Gamestate.cur.findPiece(Gamestate.cur.pieces[pieceToMove].x, Gamestate.cur.pieces[pieceToMove].y - 1) != -1)
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

        public void execute()
        {
            Gamestate.cur.pieces[pieceToMove].x = x;
            Gamestate.cur.pieces[pieceToMove].y = y;
            if(pieceToTake!=-1)
            {
                if (Gamestate.cur.pieces[pieceToTake].kind=='K')
                {
                    Console.Clear();
                    Console.WriteLine("Black won!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                if (Gamestate.cur.pieces[pieceToTake].kind == 'k')
                {
                    Console.Clear();
                    Console.WriteLine("White won!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                Gamestate.cur.pieces.RemoveAt(pieceToTake);
            }
            Gamestate.cur.moveorder.Add(new Move { x = x, y = y, pieceToMove = pieceToMove, pieceToTake = pieceToTake });
        }
    }
}
