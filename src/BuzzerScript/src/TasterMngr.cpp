#include "TasterMngr.hpp"
#include "Config.hpp"

TasterMngr::TasterMngr(){}
TasterMngr::~TasterMngr(){}

void TasterMngr::Setup()
{
    // Set the ID of the Taster
    int IDCounter = 0;
    for(auto &i: TasterList)
    {
        i.ID = IDCounter;
        IDCounter ++;
    }
}

void TasterMngr::CheckAllInputChanges()
{
    for(auto &i: TasterList)
    {
        i.CheckStateChange();
    }
}