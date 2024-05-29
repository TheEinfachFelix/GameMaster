#pragma once
#include <Arduino.h>

#include "buzzer.hpp"
#include "Config.hpp"

class BuzzerMngr
{
private:
public:
    BuzzerMngr();
    void ChecknPrintPinstate();
    void SetLED(int BuzzerID, bool Value);
    ~BuzzerMngr();
};


