#pragma once
#include <Arduino.h>

#include <Adafruit_NeoPixel.h>

struct RGBColor {
  int R;
  int G;
  int B;
};

class LEDController
{
private:
    Adafruit_NeoPixel strip;
public:
    LEDController(int numLeds, int LEDPin);
    void SetLED(int id, RGBColor pColor);
    ~LEDController();
};