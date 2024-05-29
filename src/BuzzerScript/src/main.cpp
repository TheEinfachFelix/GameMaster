#include "main.hpp"


//Buzzer BuzzerList[] = {Buzzer(47,48)};
BuzzerMngr mngr = BuzzerMngr();
TasterMngr taster = TasterMngr();

void setup() {

  Serial.begin(9600);
  //BuzzerList[0].SetLED(false);

  if (false){
    mngr.SetLED(1,true);

  }
  if (true)
  {
    LEDController controller = LEDController();
    controller.SetLED(1, 50,50,50);
  }
}

void loop() 
{

  mngr.ChecknPrintPinstate();
  //Serial.write("x");
  delay(10);
}

