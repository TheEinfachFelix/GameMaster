#pragma once
#include <Arduino.h>
#include <ArduinoJson.h>

#include "Taster.hpp"
#include "SerialHandler.hpp"

class TasterMngr
{
private:
public:
    Taster TasterList[CTasterListLength] = CTasterList;

    TasterMngr();
    ~TasterMngr();
    void Setup();

    void CheckAllInputChanges();

};


