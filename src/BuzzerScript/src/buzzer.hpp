#pragma once

#include <Arduino.h>

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
    String Read();
    void SetLED(bool value);
    ~Buzzer();
};