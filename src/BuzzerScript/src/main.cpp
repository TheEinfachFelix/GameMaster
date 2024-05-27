#include "main.hpp"


//Buzzer BuzzerList[] = {Buzzer(48,47)};

void setup() {

  Serial.begin(9600);
  //BuzzerList[0].SetLED(true);
}

void loop() 
{
  //BuzzerList[0].PrintRead();
  Serial.write("x");
  delay(10);
}

