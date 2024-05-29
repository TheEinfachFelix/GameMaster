#include "Taster.hpp"

Taster::Taster(int pPressPin)
{
    PressPin = pPressPin;

    pinMode(PressPin, INPUT_PULLDOWN);

    PressState = digitalRead(PressPin);
}

String Taster::Read()
{
    bool now = digitalRead(PressPin);
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
    return "";
}

void Taster::PrintRead()
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




Taster::~Taster()
{
}
