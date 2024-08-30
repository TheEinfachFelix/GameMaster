#include "buzzer.hpp"

Buzzer::Buzzer(int pPressPin, int pLEDPin)
{
    PressPin = pPressPin;
    LedPin = pLEDPin;
    LedState = false;

    pinMode(PressPin, INPUT_PULLDOWN);
    pinMode(LedPin, OUTPUT);

    PressState = digitalRead(PressPin);
}

void Buzzer::PrintRead()
{
    bool now = digitalRead(PressPin);
    bool old = PressState;
    if (old != now)
    {
        EventSender(BuzzerType, ID , old, now);
        PressState = now;
    }
}

void Buzzer::SetLED(bool value)
{
    digitalWrite(LedPin, value);
    LedState = value;
}

Buzzer::~Buzzer()
{
}

int Buzzer::GetLedPin()
{
    return LedPin;
}
int Buzzer::GetBuzPin()
{
    return PressPin;
}

bool Buzzer::GetLedState()
{
    return LedState;
}