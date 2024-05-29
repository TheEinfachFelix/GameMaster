#pragma once
#include <Arduino.h>

#include "Taster.hpp"

extern Taster TasterList[];

class TasterMngr
{
private:
public:
    TasterMngr();
    void Setup();
    void ChecknPrintPinstate();
    ~TasterMngr();
};


