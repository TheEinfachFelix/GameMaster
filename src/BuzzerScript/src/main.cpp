#include "main.hpp"

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