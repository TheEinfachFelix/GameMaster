#include "main.hpp"


Buzzer BuzzerList[] = {Buzzer(48,47,1)};

void setup() {

  Serial.begin(9600);
  BuzzerList[0].SetLED(true);

  if (true){
    BuzzerMngr mngr = BuzzerMngr();
    mngr.AddBuzzer(48,47);
  }
  if (true)
  {
    LEDController controller = LEDController(12,21);
    RGBColor color = RGBColor();
    color.R = 50;
    controller.SetLED(1, color);
  }
}

void loop() 
{
  BuzzerList[0].PrintRead();
  //Serial.write("x");
  delay(10);
}

