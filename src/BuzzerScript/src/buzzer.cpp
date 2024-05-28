#include "buzzer.hpp"

Buzzer::Buzzer(int pPressPin, int pLEDPin, int pID)
{
    PressPin = pPressPin;
    LedPin = pLEDPin;
    ID = pID;

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
        Serial.print(ID);
        Serial.println(" press --");
    }else if (buttonState == "release")
    {
        Serial.print(ID);
        Serial.println(" release --");
    }
}

void Buzzer::SetLED(bool value)
{
    digitalWrite(LedPin, !value);
}


Buzzer::~Buzzer()
{
}
