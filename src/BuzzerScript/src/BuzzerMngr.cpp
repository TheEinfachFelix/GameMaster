#include "BuzzerMngr.hpp"
#include <list>

BuzzerMngr::BuzzerMngr(/* args */)
{
}

BuzzerMngr::~BuzzerMngr()
{
}

void BuzzerMngr::AddBuzzer(int TasterPin, int LEDPin)
{
    int id = 2;
    Buzzer newBuzz = Buzzer(TasterPin, LEDPin, id);
    // add buzzer to buzzer list
}

void BuzzerMngr::ChecknPrintPinstate()
{

}

void BuzzerMngr::SetLED(int BuzzerID, bool Value)
{

}