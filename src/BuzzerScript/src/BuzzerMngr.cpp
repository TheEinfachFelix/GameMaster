#include "BuzzerMngr.hpp"
#include "Config.hpp"

Buzzer BuzzerList[] = CBuzzerList;

BuzzerMngr::BuzzerMngr()
{
}

BuzzerMngr::~BuzzerMngr()
{
}

void BuzzerMngr::Setup()
{
    // Set the ID of the Buzzer
    int IDCounter = 0;
    for(auto &i: BuzzerList)
    {
        IDCounter ++;
        i.ID = IDCounter;
    }
}

void BuzzerMngr::ChecknPrintPinstate()
{
    for(auto &i: BuzzerList)
    {
        i.PrintRead();
    }
}

void BuzzerMngr::SetLED(int BuzzerID, bool Value)
{
    BuzzerList[BuzzerID-1].SetLED(Value);
}