#include "buzzer.hpp"


Buzzer::Buzzer(int pPressPin, int pLEDPin)
{
    PressPin = pPressPin;
    LedPin = pLEDPin;

    pinMode(PressPin, INPUT_PULLDOWN);
    pinMode(LedPin, OUTPUT);

    PressState = digitalRead(PressPin);
}

int Buzzer::Read()
{
    bool now = digitalRead(PressPin);
    bool old = PressState;
    if (old != now)
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

void Buzzer::PrintRead()
{
    int buttonState = Read();

    if (buttonState == 1)
    {
        PrintButtonValueChanged(BuzzerType, ID , true);
    }else if (buttonState == 0)
    {
        PrintButtonValueChanged(BuzzerType, ID , false);
    }
}

void Buzzer::SetLED(bool value)
{
    digitalWrite(LedPin, value);
}


Buzzer::~Buzzer()
{
}
