#include "buzzer.hpp"

Buzzer::Buzzer(int pPressPin, int pLEDPin)
{
    PressPin = pPressPin;
    LedPin = pLEDPin;
    LedState = false;

    pinMode(PressPin, INPUT_PULLDOWN);
    pinMode(LedPin, OUTPUT);

    PressState = GetInputState();
}
Buzzer::~Buzzer(){}

void Buzzer::CheckStateChange()
{
    bool now = GetInputState();
    bool old = PressState;
    if (old != now)
    {
        PrintEvent(old,now);
        PressState = now;
    }
}

void Buzzer::PrintEvent(bool old, bool now)
{
    EventSender(BuzzerType, ID , old, now);
}

void Buzzer::SetLedValue(bool value)
{
    digitalWrite(LedPin, value);
    LedState = value;
}

// States
bool Buzzer::GetInputState()
{
    return digitalRead(PressPin);
}
bool Buzzer::GetLedState()
{
    return LedState;
}

// Pin
int Buzzer::GetLedPin()
{
    return LedPin;
}
int Buzzer::GetInputPin()
{
    return PressPin;
}