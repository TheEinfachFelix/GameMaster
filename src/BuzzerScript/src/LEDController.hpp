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
    String JsonSetHandler(JsonDocument pJson);
    String JsonSetLedCollor(JsonDocument pJson);
    String JsonGetHandler(JsonDocument pJson);
    String GetLedCollorAsJson(JsonDocument pJson);

    void SetLED(int id, int R, int G, int B);
public:
    LEDController();
    void Setup();
    ~LEDController();
    String JsonHandler(JsonDocument pJson);
};