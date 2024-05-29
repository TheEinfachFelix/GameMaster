#include "main.hpp"

BuzzerMngr mngr = BuzzerMngr();
TasterMngr taster = TasterMngr();

void setup() {
  Serial.begin(9600);

  mngr.SetLED(1,true);

  LEDController controller = LEDController();
  controller.SetLED(1, 50,50,50);
}

void loop() 
{
  //mngr.ChecknPrintPinstate();
  taster.ChecknPrintPinstate();
  
  delay(1000);
}

