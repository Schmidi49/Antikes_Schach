using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Antikes_Schach_ConsoleUI
{
    class Program
    {
        private static Move curMove = new Move(); //move, which user inputs
        private static int xOld = -1, yOld = -1, xNew = -1, yNew = -1; //coordinates of the users move

        static void Main(string[] args)
        {
            //Generates a new Position and prints it
            Gamestate.cur.getStartposition();
            Board.generate();
            Board.print();

            //main loop, which awaits the users input
            while (true)
            {
                string a; //input of the user

                //reads user input until the input is greater than 4

                Console.Write("Move:");
                a = Console.ReadLine();

                //input, if user wants to load in an FEN
                if(a=="load")
                {
                    load();
                }
                //input, if user wants to save (has to copy) his current game as a FEN
                else if(a=="save")
                {
                    save();
                }
                //exits the programm
                else if(a=="exit")
                {
                    exit();
                }
                //if no other function is called, input is a move (prooving of the move happens in this function)
                else
                {
                    move(a);
                }
                //after the input, the new position is generated
                Board.generate();
                Board.print();

                //checks for the wni of any side
                if (Gamestate.cur.result != 0)
                {
                    if (Gamestate.cur.result > 0)
                    {
                        Console.Write("White ");
                    }
                    else
                    {
                        Console.Write("Black ");
                    }
                    Console.WriteLine("has won!");
                    Console.WriteLine("Do you want to play a new game? [y/n]");
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
                    else
                    {
                        Gamestate.cur.reset();
                        Gamestate.cur.getStartposition();
                    }
                }

            }
        }

        //function, which handles move inputs
        private static void move(string s)
        {
            try
            {
                xOld = s[0] - 97; //convert user input (char) to usable coordinate (int) (a-h->0-7)
                yOld = s[1] - 49; //convert user input (char) to usable coordinate (int) (1-8->0-7)
                xNew = s[2] - 97; //convert user input (char) to usable coordinate (int) (a-h->0-7)
                yNew = s[3] - 49; //convert user input (char) to usable coordinate (int) (1-8->0-7)

                curMove.getMove(xOld, yOld, xNew, yNew); //generates the (internal) Move out of the input

                //if the move is leagl, execut it
                if (curMove.tryMove())
                {
                    curMove.execute();
                }
                //if move is illeagl, write this back
                else
                {
                    Console.WriteLine("Illegal Move!\nPress any key to continue!");
                    Console.ReadKey();
                }
            }
            //handling of any other input error 
            catch(Exception e)
            {
                Console.Write("Illeagal input! Exception: {0}\nPress any key to continue!", e);
                Console.ReadKey();
            }
        }

        //function for loading a FEN
        private static void load()
        {
            //reads the FEN
            string input;
            Console.Write("FEN: ");
            input = Console.ReadLine();

            //if the fen is correct, it gets load into the Programm
            if (Gamestate.cur.getFEN(input))
            {
                Console.WriteLine("FEN load succesfully");
                Console.WriteLine("Press any Key to continue!");
                Console.ReadKey();
            }
            //if nt correct, user is informed
            else
            {
                Console.WriteLine("illeagal FEN");
                Console.WriteLine("Press any Key to continue!");
                Console.ReadKey();
            }
        }

        //function wich gives back the FEN of the current game
        private static void save()
        {
            //prints current position as a FEN
            Console.WriteLine("Your current FEN:");
            Console.WriteLine(Gamestate.cur.genFEN());

            //asks user to continue
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

        //exits the Programm
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

    //class for generating ascii grid of a gamestate, can be used for every string-based öutput
    public class Board
    {
        //one char for each square, char represents the piece on it, ' ' if there is no piece
        public static char[,] squares = new char[8, 8];

        //generates the square array out of the current position
        public static void generate()
        {
            //ereases the last position
            for (int i = 0; i < 64; i++)
            {
                squares[i / 8, i % 8] = ' ';
            }
            //writes every piece in the correct position of the square array
            foreach (Piece Piece in Gamestate.cur.pieces)
            {
                //repaces 'p' with 'b' for better visibility
                if(Piece.kind=='p')
                {
                    squares[Piece.x, Piece.y] = 'b';
                }
                //kind is the visual representation (except black pawns)
                else
                {
                    squares[Piece.x, Piece.y] = Piece.kind;
                }
            }
        }

        //prints the square array (consiting of the current position) in an asccii grid
        public static void print()
        {
            Console.Clear();
            Console.WriteLine(topFrame());
            //writes the first seven lines of the grid
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine(inbetweenLine());
                Console.WriteLine(dataLine(7 - i));
                Console.WriteLine(inbetweenLine());
                Console.WriteLine(middleFrame());
            }
            //the last row works different
            Console.WriteLine(inbetweenLine());
            Console.WriteLine(dataLine(0));
            Console.WriteLine(inbetweenLine());
            Console.WriteLine(botFrame());
        }

        //generates the top frame of the console grid
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
        }

        //generates the bot frame of the console grid
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
        }

        //generates the vertical lines frame of the console grid
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
        }

        //generates a vertikal, empty lines of the console grid
        private static string inbetweenLine()
        {
            string s = "";
            s += '\u2551';
            for (int j = 0; j < 8; j++)
            {
                s += "       " + '\u2551';
            }
            return s;
        }

        //generates a line with the pieces in it
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
        }

    }

    //manages everything considering pieces
    public class Piece
    {
        /*
        kind of the piece is represented by the letter

        great letter->white peace
        small letter->black Piece

        K. . .King
        F. . .Ferz
        R. . .Rook
        N . . . Knight
        A . . . Alfil
        P . . . Pawn
        */
        public char kind { get; set; }
        //x position of the piece
        public int x { get; set; }
        //y position of the piece
        public int y { get; set; }

        //gives back the colour of a certain piece, true for white, wrong for black
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

        //gives back the value of the piece
        //neagtive value for balck pieces
        //value is an evaluation
        //king has high value to represent that hes the objektive o the game
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

        //proofs, if a piece is (obvieously) vali/unvalid
        //doesnt proof double seizure
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

        //finds every possible move of a certain piece
        public List<Move> findPossibleMoves()
        {
            List<Move> moves = new List<Move>();

            //finds out which piece of the current position this actually is
            int pieceNum = Gamestate.cur.findPiece(x, y);

            //theoretically possible moves changes which the kind of the piece
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
                    //generates every square within +1/-1 in x and y direction
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
                    //generates the  diagonal, 1 square away squares
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
                //generates the  diagonal, 2 square away squares
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
                    //returns the 8 knight squares
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
                    //trys out every squares of the rooks column
                    Move movex = Move.genMove(x, y, temp, y);
                    if (movex.tryMove())
                    {
                        moves.Add(movex);
                    }
                    //trys out every squares of the rooks crow
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

    //manages everything considering Gamestate, Positions and notation
    public class Gamestate
    {
        //the current Gamestate, is used by every function which needs information of the current position
        //other Gamestates can be safed by writing the in an seperate instance of the class Gamestate
        public static Gamestate cur = new Gamestate { pieces = new List<Piece>(), moveorder = new List<Move>(), result=0 };

        //List of all pieces of the position
        public List<Piece> pieces { get; set; }

        //protocoll of every made move
        public List<Move> moveorder { get; set; }

        public int result { get; set; }

        //resets a Gamestate
        public void reset()
        {
            pieces.Clear();
            moveorder.Clear();
            result = 0;
        }

        //deletes the position and return it to the startposition
        public void getStartposition()
        {
            pieces.Clear();
            moveorder.Clear();
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

        //finds a piece on a certain square
        public int findPiece(int x, int y)
        {
            //looks up every piece
            for (int i = 0; i < pieces.Count; i++)
            {
                if (x == pieces[i].x && y == pieces[i].y)
                {
                    return i;
                }
            }
            //value if there is no piece on the square
            return -1;
        }

        //generates the FEN of the current Gamestate
        public string genFEN()
        {
            StringBuilder FEN = new StringBuilder();
            int count = 0, stringPos = 0; ;
            int piece;

            //goes through every row, from top to bot
            for (int y=7;y>-1;y--)
            {
                //goes through every coloum, from right to left
                for (int x=0;x<8;x++)
                {
                    piece = findPiece(x, y);
                    //if there is no piece, raise a counter to count every empty square
                    if (piece==-1)
                    {
                        count++;
                    }
                    //if there is a piece, first print the empty squares (and reset counter) then print the kind of the piece
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
                //at the end of an  row, write the last empty squares
                if (count != 0)
                {
                    FEN.Insert(stringPos, count.ToString()[0]);
                    stringPos++;
                    count = 0;
                }
                //seperate the lines
                FEN.Insert(stringPos, '/');
                stringPos++;
            }

            //change the last linespliter (/) to the correct space
            FEN[stringPos - 1] = ' ';

            //lookup the colour on move
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

        //trys to import an FEN, returns false if FEN is illegal
        public bool getFEN(string FEN)
        {
            Gamestate temp = new Gamestate { pieces = new List<Piece>(), moveorder = new List<Move>(), result=0 };

            string[] splitLine = FEN.Split('/', ' '); //splitting the lines in the FEN 
            int count = 0; //number, in wich column the programm is currently
            int whiteKing = 0, blackKing =0; //keeps track of the number of Kings (only 1 allowed)

            //if there is less than 9 information string (8 lines + on-move-indicator
            if (splitLine.Length < 9)
            {
                return false;
            }

            //goes through every line
            for (int i = 0; i < 8; i++)
            {
                //checks for legal chars
                Regex rx = new Regex(@"[prnafk12345678]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(splitLine[i]);

                //every char must be legal
                if (matches.Count == splitLine[i].Length)
                {
                    //goes threw every chat
                    for (int j = 0; j < splitLine[i].Length; j++)
                    {
                        //adds empty fields to the coloumn indicator
                        if (splitLine[i][j] < 58)
                        {
                            count += splitLine[i][j] - 48;
                        }
                        //in the other case, char must be a piece 
                        else
                        {
                            temp.pieces.Add(new Piece { kind = splitLine[i][j], x = count, y = 7 - i });
                            count++;
                            if (splitLine[i][j] == 'K')
                            {
                                whiteKing++;
                            }
                            else if (splitLine[i][j] == 'k')
                            {
                                blackKing++;
                            }
                        }
                        //if line inicator is greater than 8, FEN is illegal
                        //line indicator is only allowed to be 8, if character was the last input of this line
                        if (count > 8 || (count == 8 && !(j == splitLine[i].Length - 1)))
                        {
                            return false;
                        }
                    }
                }
                //if there is an illegal char, FEN is illegal
                else
                {
                    return false;
                }
                //reseting the coloumn indicator
                count = 0;
            }

            //if there is not exactly 1 king on each side, FEN is illegal
            if (!(whiteKing==1 && blackKing==1))
            {
                return false;
            }

            //injecting an NULL-move for the right on-Move-detedtion
            if (splitLine[8] == "b")
            {
                temp.moveorder.Add(new Move());
            }
            //on-move-indicator must be either w or b
            else if (splitLine[8] != "w")
            {
                return false;
            }

            //writing the temporary Gamestate on the current gamesate
            Gamestate.cur = temp;
            return true;
        }
    }

    //manages moves of a gamestate
    public class Move
    {
        //destination of the move
        public int x { get; set; }
        public int y { get; set; }
        //index of hte pice to move
        public int pieceToMove { get; set; }
        //index of hte pice to take
        public int pieceToTake { get; set; }

        //generates a move (noted by coordinates) either by returning or by writingin a current move
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

        //trys a move, returns true if its a legal move
        public bool tryMove()
        {
            //if the piece to move doesnt exist, it isnt a legal move
            if (pieceToMove == -1)
            {
                return false;
            }

            //every even move white has to move
            if (Gamestate.cur.pieces[pieceToMove].colour() == (Gamestate.cur.moveorder.Count % 2 == 0) ? false : true)
            {
                return false;
            }

            //white cant take white pieces and vice versa
            if (pieceToTake!=-1)
            {
                if(Gamestate.cur.pieces[pieceToMove].colour()== Gamestate.cur.pieces[pieceToTake].colour())
                {
                    return false;
                }
            }

            //destination must be on the field
            if (!(x > -1 && x < 8 && y > -1 && y < 8))
            {
                return false;
            }
             
            //moveset changes with the kind of the piece

            if (Gamestate.cur.pieces[pieceToMove].kind == 'P')
            {
                //checks if squar ahead is free
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
                //looks if the piece to take is diagonaly
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
                //checks if squar ahead is free
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
                //looks if the piece to take is diagonaly
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
                //king can only move 1 in each direction away from his startpoint
                if (x - Gamestate.cur.pieces[pieceToMove].x < 2 && x - Gamestate.cur.pieces[pieceToMove].x > -2 && y - Gamestate.cur.pieces[pieceToMove].y < 2 && y - Gamestate.cur.pieces[pieceToMove].y > -2 && !(Gamestate.cur.pieces[pieceToMove].x == x && Gamestate.cur.pieces[pieceToMove].y == y))
                {
                    return true;
                }
            }

            if (Gamestate.cur.pieces[pieceToMove].kind == 'F' || Gamestate.cur.pieces[pieceToMove].kind == 'f')
            {
                //fertz can only move 1 and it must be diagonally
                if (x - Gamestate.cur.pieces[pieceToMove].x == 1 && y - Gamestate.cur.pieces[pieceToMove].y == 1 || x - Gamestate.cur.pieces[pieceToMove].x == 1 && y - Gamestate.cur.pieces[pieceToMove].y == -1 || x - Gamestate.cur.pieces[pieceToMove].x == -1 && y - Gamestate.cur.pieces[pieceToMove].y == 1 || x - Gamestate.cur.pieces[pieceToMove].x == -1 && y - Gamestate.cur.pieces[pieceToMove].y == -1)
                {
                    return true;
                }
            }

            if (Gamestate.cur.pieces[pieceToMove].kind == 'A' || Gamestate.cur.pieces[pieceToMove].kind == 'a')
            {
                //fertz can only move 2 and it must be diagonally (no collision test is requiered)
                if (x - Gamestate.cur.pieces[pieceToMove].x == 2 && y - Gamestate.cur.pieces[pieceToMove].y == 2 || x - Gamestate.cur.pieces[pieceToMove].x == 2 && y - Gamestate.cur.pieces[pieceToMove].y == -2 || x - Gamestate.cur.pieces[pieceToMove].x == -2 && y - Gamestate.cur.pieces[pieceToMove].y == 2 || x - Gamestate.cur.pieces[pieceToMove].x == -2 && y - Gamestate.cur.pieces[pieceToMove].y == -2)
                {
                    return true;
                }
            }

            if (Gamestate.cur.pieces[pieceToMove].kind == 'N' || Gamestate.cur.pieces[pieceToMove].kind == 'n')
            {
                //knights can move 1 in the one direction and 2 into the other (no collision test is requiered)
                if (Math.Abs(Gamestate.cur.pieces[pieceToMove].x - x) == 2 && Math.Abs(Gamestate.cur.pieces[pieceToMove].y - y) == 1 || Math.Abs(Gamestate.cur.pieces[pieceToMove].x - x) == 1 && Math.Abs(Gamestate.cur.pieces[pieceToMove].y - y) == 2)
                {
                    return true;
                }
            }

            if (Gamestate.cur.pieces[pieceToMove].kind == 'R' || Gamestate.cur.pieces[pieceToMove].kind == 'r')
            {
                //move must be either vertical or horizontlly
                if (Math.Abs(Gamestate.cur.pieces[pieceToMove].x - x) > 0 && Gamestate.cur.pieces[pieceToMove].y - y == 0 || Math.Abs(Gamestate.cur.pieces[pieceToMove].y - y) > 0 && Gamestate.cur.pieces[pieceToMove].x - x == 0)
                {
                    //proofs if there is any piece between the rook and his destination
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

        //executes a move (does NOT check if its legal
        public void execute()
        {
            //repositioning of the piece to ove
            Gamestate.cur.pieces[pieceToMove].x = x;
            Gamestate.cur.pieces[pieceToMove].y = y;
            
            //if there is a piece to take, remove it
            if(pieceToTake!=-1)
            {
                //if a king has been taken, game is won
                if (Gamestate.cur.pieces[pieceToTake].kind=='K')
                {
                    Gamestate.cur.result++;
                    Gamestate.cur.moveorder.Add(null);
                }
                if (Gamestate.cur.pieces[pieceToTake].kind == 'k')
                {
                    Gamestate.cur.result--;
                    Gamestate.cur.moveorder.Add(null);
                }
                Gamestate.cur.pieces.RemoveAt(pieceToTake);
            }
            Gamestate.cur.moveorder.Add(new Move { x = x, y = y, pieceToMove = pieceToMove, pieceToTake = pieceToTake });
        }
    }
}
