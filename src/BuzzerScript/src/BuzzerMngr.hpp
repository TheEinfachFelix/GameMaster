#pragma once
#include <Arduino.h>
#include <ArduinoJson.h>

#include "buzzer.hpp"
#include "Config.hpp"
#include "SerialHandler.hpp"

class BuzzerMngr
{
private:
    String JsonSetHandler(JsonDocument pJson);
    String JsonGetHandler(JsonDocument pJson);
    String JsonSetLedState(JsonDocument pJson);
    String JsonGetPin(JsonDocument pJson);
    void SetLED(int BuzzerID, bool Value);
public:
    BuzzerMngr();
    void ChecknPrintPinstate();
    
    ~BuzzerMngr();
    void Setup();
    String JsonHandler(JsonDocument pJson);
};
