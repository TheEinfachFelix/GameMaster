
#pragma once
// Libs
#include <Arduino.h>


class Buzzer
{
private:
    int PressPin;
    int LedPin;
    int ID;
    bool PressState;
public:
    Buzzer(int pPressPin, int pLEDPin, int pID);
    void PrintRead();
    String Read();
    void SetLED(bool value);
    ~Buzzer();
};