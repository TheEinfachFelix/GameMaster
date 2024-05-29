#include "Taster.hpp"

Taster::Taster(int pPressPin)
{
    PressPin = pPressPin;

    pinMode(PressPin, INPUT_PULLUP);

    PressState = digitalRead(PressPin);
    //PressState = 0;
}

String Taster::Read()
{
    bool now = digitalRead(PressPin);

Serial.print("now: ");
Serial.print(now);
Serial.print(" PressState: ");
Serial.print(PressState);

    if (PressState != now)
    {
        PressState = now;

        if(!now)
        {
          return "on";
        } else 
        {
          return "off";
        }
    }
    return "";
}

void Taster::PrintRead()
{
    String buttonState = Read();

    if (buttonState == "on")
    {

        Serial.print("{\"Input_Type\" = \"Taster\" ,\"ID\" = ");
        Serial.print(PressPin);
        Serial.println(", \"value\" = true }");
    }else if (buttonState == "off")
    {
        Serial.print("{\"Input_Type\" = \"Taster\" ,\"ID\" = ");
        Serial.print(PressPin);
        Serial.println(", \"value\" = false }");
    }
}




Taster::~Taster()
{
}
