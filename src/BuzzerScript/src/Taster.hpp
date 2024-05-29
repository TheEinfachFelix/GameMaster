
#pragma once
// Libs
#include <Arduino.h>


class Taster
{
private:
    int PressPin;    
    bool PressState;
public:
    int ID = 0;
    Taster(int pPressPin);
    void PrintRead();
    String Read();
    ~Taster();
};