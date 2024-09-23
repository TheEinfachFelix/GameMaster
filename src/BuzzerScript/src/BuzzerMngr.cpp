#include "BuzzerMngr.hpp"
#include "Config.hpp"

BuzzerMngr::BuzzerMngr(){}
BuzzerMngr::~BuzzerMngr(){}

void BuzzerMngr::Setup()
{
    // Set the ID of the Buzzer
    int IDCounter = 0;
    for(auto &i: BuzzerList)
    {
        i.ID = IDCounter;
        IDCounter ++;
    }
}

void BuzzerMngr::CheckAllInputChanges()
{
    for(auto &i: BuzzerList)
    {
        i.CheckStateChange();
    }
}
