#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>
#include <vector>
#include <list>
#include <bits/stdc++.h>

#include "BuzzerMngr.hpp"
extern BuzzerMngr BuzzMngr;

#include "LEDController.hpp"
extern LEDController ledCntrl;

#include "TasterMngr.hpp"
extern TasterMngr tastMngr;

extern bool BlockBuzzer;
extern bool ProcessingRequestHold;


void SetupAllHardware();
void LoopAllHardware();
void SetLEDs();