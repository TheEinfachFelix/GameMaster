#pragma once
#include <Arduino.h>
#include <Adafruit_NeoPixel.h>
#include "Config.hpp"



class LEDController
{
private:
    //Adafruit_NeoPixel strip;
    Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUM_LEDS, LED_PIN, NEO_GRB + NEO_KHZ800);
public:
    LEDController();
    void SetLED(int id, int R, int G, int B);
    ~LEDController();
};