#include "buzzer.hpp"

Buzzer::Buzzer(int pPressPin, int pLEDPin)
{
    PressPin = pPressPin;
    LedPin = pLEDPin;

    pinMode(PressPin, INPUT);
    pinMode(LedPin, OUTPUT);

    PressState = digitalRead(PressPin);
}

String Buzzer::Read()
{
    bool now = digitalRead(LedPin);
    bool old = PressState;
    if (old != now)
    {
        PressState = now;

        if(!now)
        {
          return "press";
        } else 
        {
          return "release";
        }
    }
}

void Buzzer::PrintRead()
{
    String buttonState = Read();

    if (buttonState == "press")
    {
        Serial.println(" press --");
    }else if (buttonState == "release")
    {
        Serial.println(" release --");
    }
}

void Buzzer::SetLED(bool value)
{
    digitalWrite(LedPin, value);
}


Buzzer::~Buzzer()
{
}
