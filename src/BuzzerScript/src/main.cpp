#include "JsonHandler.hpp"
#include <Arduino.h>

void setup() 
{
  SetupJson();
  SetupAllHardware();
}

void loop() 
{
  LoopAllHardware();
  LoopJson();
}