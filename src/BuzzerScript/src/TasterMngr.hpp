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
    ~TasterMngr();
    void Setup();

    void CheckAllInputChanges();
    
    String JsonHandlerGetSet(JsonDocument pJson);
};


