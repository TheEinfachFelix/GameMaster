#include "main.hpp"


Buzzer BuzzerList[] = {Buzzer(47,48,1)};

void setup() {

  Serial.begin(9600);
  BuzzerList[0].SetLED(false);

  if (false){
    BuzzerMngr mngr = BuzzerMngr();
    mngr.AddBuzzer(47,48);
  }
  if (true)
  {
    LEDController controller = LEDController();
    controller.SetLED(1, 50,50,50);
  }
}

void loop() 
{

  BuzzerList[0].PrintRead();
  //Serial.write("x");
  delay(10);
}

