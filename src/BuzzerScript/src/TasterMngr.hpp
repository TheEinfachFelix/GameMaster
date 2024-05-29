#pragma once
#include <Arduino.h>

#include "Taster.hpp"

class TasterMngr
{
private:
public:
    TasterMngr();
    void ChecknPrintPinstate();
    ~TasterMngr();
};


