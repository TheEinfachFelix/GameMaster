#pragma once
#include <Arduino.h>
#include <Adafruit_NeoPixel.h>
#include <ArduinoJson.h>

#include "SerialHandler.hpp"
#include "Config.hpp"



class LEDController
{
private:
    Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUM_LEDS, LED_PIN, NEO_GRB + NEO_KHZ800);

public:
    String Mode = JsonRequestLEDModeAuto;

    LEDController();
    ~LEDController();

    void Setup();
    void SetLED(int id, int R, int G, int B);
};

