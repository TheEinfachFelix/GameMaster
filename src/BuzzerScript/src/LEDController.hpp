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

    // Set
    String JsonSetHandler(JsonDocument pJson);
    String JsonSetCollor(JsonDocument pJson);
    // Get
    String JsonGetHandler(JsonDocument pJson);
    String JsonGetCollor(JsonDocument pJson);

    void SetLED(int id, int R, int G, int B);
public:
    LEDController();
    ~LEDController();
    void Setup();
    
    String JsonHandlerGetSet(JsonDocument pJson);
};