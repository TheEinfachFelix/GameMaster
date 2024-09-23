#pragma once

#include <Arduino.h>

#include "SerialHandler.hpp"
#include "Config.hpp"

class Buzzer
{
private:
    int PressPin;
    int LedPin;
    bool LedState;
    
    bool PressState;
public:
    int ID = 0;
    Buzzer(int pPressPin, int pLEDPin);
    ~Buzzer();
    // 
    void CheckStateChange();
    void PrintEvent(bool old, bool now);

    // Set -> SetInputState is done by only Calling a event
    void SetLedValue(bool value);
    // get Pin
    int GetLedPin();
    int GetInputPin();
    // get State
    bool GetLedState();
    bool GetInputState();
    
    
};