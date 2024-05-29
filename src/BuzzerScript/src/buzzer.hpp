#pragma once

#include <Arduino.h>

#include "SerialHandler.hpp"
#include "Config.hpp"

class Buzzer
{
private:
    int PressPin;
    int LedPin;
    
    bool PressState;
public:
    int ID = 0;
    Buzzer(int pPressPin, int pLEDPin);
    void PrintRead();
    int Read();
    void SetLED(bool value);
    ~Buzzer();
};