#pragma once
#include <Arduino.h>

#include "buzzer.hpp"

class BuzzerMngr
{
private:
    // buzzer list here
public:
    BuzzerMngr();
    void AddBuzzer(int TasterPin, int LEDPin);
    void ChecknPrintPinstate();
    void SetLED(int BuzzerID, bool Value);
    ~BuzzerMngr();
};


