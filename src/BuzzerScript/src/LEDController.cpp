#include "LEDController.hpp"

LEDController::LEDController(int numLeds, int LEDPin)
{
    strip = Adafruit_NeoPixel(numLeds, LEDPin, NEO_GRB + NEO_KHZ800);
    strip.begin();
    strip.show();
}

void LEDController::SetLED(int id, RGBColor pColor)
{
    strip.setPixelColor(id, pColor.R, pColor.G, pColor.B);
    strip.show();
}

LEDController::~LEDController()
{
}