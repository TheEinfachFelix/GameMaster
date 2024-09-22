#pragma once
#include <Arduino.h>
#include <ArduinoJson.h>

#include "buzzer.hpp"
#include "Config.hpp"
#include "SerialHandler.hpp"

class BuzzerMngr
{
private:
public:
    Buzzer BuzzerList[CBuzzerListLength] = CBuzzerList;
    String Mode = JsonRequestLEDModeAuto;
    bool isDisabeled;

    BuzzerMngr();
    ~BuzzerMngr();

    void Setup();
    void CheckAllInputChanges();
};
