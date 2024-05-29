#pragma once
#include <Arduino.h>

#include "Taster.hpp"
#include "config.h"
#include "Config.hpp"



class TasterMngr
{
private:
    //Taster TasterList[] = CTasterList;
public:
    TasterMngr();
    void ChecknPrintPinstate();
    ~TasterMngr();
};


