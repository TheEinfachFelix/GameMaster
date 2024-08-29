#pragma once
#include <Arduino.h>
#include <ArduinoJson.h>

#include "Taster.hpp"
#include "SerialHandler.hpp"

extern Taster TasterList[];

class TasterMngr
{
private:
public:
    TasterMngr();
    void Setup();
    void ChecknPrintPinstate();
    ~TasterMngr();
    String JsonHandler(JsonDocument pJson);
};


