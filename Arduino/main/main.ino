// Bibliothek einbinden
#include <LiquidCrystal.h>
#include "IRremote.h"

int receiver = 6; // Signal Pin des IR-Empfängers an Arduino Digital Pin 11

/*-----( Declare objects )-----*/
IRrecv irrecv(receiver);     // Erstellen Instanz von 'irrecv'
decode_results results;      // Erstellen Instanz von 'decode_results'

// Initialisiere die Bibliothek mit den Nummern der Schnittstellenpins
LiquidCrystal lcd(7, 8, 9, 10, 11, 12);

// Spalten und Zeilen des lcd
int lcdsize[] = {16, 2};

int incomingByte = 0; // for incoming serial data
String Piece;
char result = '0',start_x,start_y,end_x,end_y;
char test[4];

void setup() {
  Serial.begin(9600); // opens serial port, sets data rate to 9600 bps
  irrecv.enableIRIn(); // Startet den Empfänger
  // Richtet die Anzahl der Spalten und Zeilen der LCD ein:
  lcd.begin(lcdsize[0], lcdsize[1]);
  // Drucket eine Nachricht an das LCD.
  lcd.print("Welcome. Please");
  lcd.setCursor(0,1);
  lcd.print("start Engine!");
}

void loop() {

    //read Pieces
    while (Serial.available() <= 0);
    incomingByte = Serial.read();
    test[0] = incomingByte;
    bool gotPiece=false;
    do
    {
        switch(incomingByte)
        {
            case 'p': Piece = "Pawn:"; gotPiece=true; break;
            case 'P': Piece = "Pawn:"; gotPiece=true; break;
            case 'r': Piece = "Rook:"; gotPiece=true; break;
            case 'R': Piece = "Rook:"; gotPiece=true; break;
            case 'n': Piece = "Knight:"; gotPiece=true; break;
            case 'N': Piece = "Knight:"; gotPiece=true; break;
            case 'f': Piece = "Ferz:"; gotPiece=true; break;
            case 'F': Piece = "Ferz:"; gotPiece=true; break;
            case 'a': Piece = "Alfil:"; gotPiece=true; break;
            case 'A': Piece = "Alfil:"; gotPiece=true; break;
            case 'k': Piece = "King:"; gotPiece=true; break;
            case 'K': Piece = "King:"; gotPiece=true; break;
            case 'W': result = 'W'; gotPiece=true; break;
            case 'B': result = 'B'; gotPiece=true; break;
            default: gotPiece=false;break;
        }  
    }while(!gotPiece);
    
    

    if(result=='0')
    {
        //read start_x
        while (Serial.available() <= 0);
        incomingByte = Serial.read();
        test[1] = incomingByte;
        start_x = incomingByte + 49;
        //read start_y
        while (Serial.available() <= 0);
        incomingByte = Serial.read();
        test[2] = incomingByte;
        start_y = incomingByte + 1;

        //read end_x
        while (Serial.available() <= 0);
        incomingByte = Serial.read();
        test[3] = incomingByte;
        end_x = incomingByte + 49;
        //read end_y
        while (Serial.available() <= 0);
        incomingByte = Serial.read();
        test[4] = incomingByte;
        end_y = incomingByte + 1;
        
        lcd.clear();
        lcd.setCursor(0,0);
        lcd.print(Piece);

        lcd.setCursor(0,1);
        lcd.print(start_x);
        lcd.setCursor(1,1);
        lcd.print(start_y);
        lcd.setCursor(2,1);
        lcd.print("-");
        lcd.setCursor(3,1);
        lcd.print(end_x);
        lcd.setCursor(4,1);
        lcd.print(end_y);
        lcd.setCursor(8,1);
        lcd.print(test);   
    }
    else
    {
      if(result == 'W')
      {
          lcd.clear();
          lcd.setCursor(0,0);
          lcd.print("White");
          lcd.setCursor(0,1);
          lcd.print("has won!");
      }
      else
      {
          lcd.clear();
          lcd.setCursor(0,0);
          lcd.print("Black");
          lcd.setCursor(0,1);
          lcd.print("has won!");
      }
    }

    bool gotres = false;
    do
    {
        if (irrecv.decode(&results)) // Haben wir ein IR-Signal erhalten?
        {
            switch(results.value)
            {
                case 0xFF629D: Serial.println("U"); gotres=true; break;
                case 0xFFA857: Serial.println("D"); gotres=true;    break;
                case 0xFFC23D: Serial.println("N"); gotres=true;   break;
                case 0xFF22DD: Serial.println("P");  gotres=true;   break;
                case 0xFF02FD: Serial.println("M");  gotres=true;   break;
                default: gotres = false;
            }
            irrecv.resume(); // Erhalte den nächsten Wert
        }  

    }while(!gotres);
    
}
