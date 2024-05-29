#include "main.hpp"

BuzzerMngr BuzzMngr = BuzzerMngr();
TasterMngr tastMngr = TasterMngr();
LEDController ledCntrl = LEDController();


void setup() {
  
  Serial.begin(9600);

  tastMngr.Setup();
  BuzzMngr.Setup();
  //ledCntrl.Setup();

  BuzzMngr.SetLED(1,true);
  BuzzMngr.SetLED(3,true);
  //ledCntrl.SetLED(1, 50,50,50);
  //ledCntrl.SetLED(3, 50,15,0);
}

void loop() 
{
  BuzzMngr.ChecknPrintPinstate();
  tastMngr.ChecknPrintPinstate();


  delay(10);
}

