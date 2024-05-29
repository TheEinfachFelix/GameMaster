#include "TasterMngr.hpp"
#include "Config.hpp"

Taster TasterList[] = CTasterList;

TasterMngr::TasterMngr()
{

}

void TasterMngr::Setup()
{
    // Set the ID of the Taster
    int IDCounter = 0;
    for(auto &i: TasterList)
    {
        IDCounter ++;
        i.ID = IDCounter;
    }
}

TasterMngr::~TasterMngr()
{
    
}

void TasterMngr::ChecknPrintPinstate()
{
    for(auto &i: TasterList)
    {
        i.PrintRead();
    }
}