#include "Taster.hpp"

Taster::Taster(int pPressPin)
{
    PressPin = pPressPin;

    pinMode(PressPin, INPUT_PULLUP);

    PressState = digitalRead(PressPin);
}

int Taster::Read()
{
    bool now = digitalRead(PressPin);

    if (PressState != now)
    {
        PressState = now;

        if(!now)
        {
          return 1;
        } else 
        {
          return 0;
        }
    }
    return -1;
}

void Taster::PrintRead()
{
    int buttonState = Read();

    if (buttonState == 1)
    {
        PrintButtonValueChanged(TasterType, ID , true);
    }else if (buttonState == 0)
    {
        PrintButtonValueChanged(TasterType, ID , false);
    }
}



Taster::~Taster()
{
}
