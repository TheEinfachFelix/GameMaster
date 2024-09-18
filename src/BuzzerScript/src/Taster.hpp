#pragma once

#include <Arduino.h>

#include "SerialHandler.hpp"
#include "Config.hpp"

class Taster
{
private:
    int PressPin;    
    bool PressState;
public:
    int ID = 0;

    Taster(int pPressPin);
    ~Taster();

    void CheckStateChange();
    void PrintEvent(bool old, bool now);

    int GetPin();
    bool GetInputState();
};