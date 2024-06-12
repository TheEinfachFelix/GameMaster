#include "LEDController.hpp"

LEDController::LEDController()
{
}

void LEDController::Setup()
{
    strip.begin();
    strip.show();
}

void LEDController::SetLED(int id, int R = 0, int G = 0, int B = 0)
{
    strip.setPixelColor(id, R, G, B);
    strip.show();
}

LEDController::~LEDController()
{
}