#include "Taster.hpp"

Taster::Taster(int pPressPin)
{
    PressPin = pPressPin;

    pinMode(PressPin, INPUT_PULLUP);

    PressState = digitalRead(PressPin);
}

void Taster::PrintRead()
{
    bool now = digitalRead(PressPin);

    if (PressState != now)
    {
        EventSender(TasterType, ID , now, PressState);
        PressState = now;
    }
}

int Taster::GetPin()
{
    return PressPin;
}

Taster::~Taster()
{
}
