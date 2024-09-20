#include "Taster.hpp"

Taster::Taster(int pPressPin)
{
    PressPin = pPressPin;

    pinMode(PressPin, INPUT_PULLUP);

    PressState = GetInputState();
}
Taster::~Taster(){}

void Taster::CheckStateChange()
{
    bool now = GetInputState();

    if (PressState != now)
    {
        PrintEvent(PressState, now);
        PressState = now;
    }
}
void Taster::PrintEvent(bool old, bool now)
{
    EventSender(TasterType, ID , old, now);
}

// Get
int Taster::GetPin()
{
    return PressPin;
}
bool Taster::GetInputState()
{
    return digitalRead(PressPin);
}