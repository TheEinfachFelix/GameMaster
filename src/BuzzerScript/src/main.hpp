#pragma once
#include "BuzzerMngr.hpp"
#include "LEDController.hpp"
#include "TasterMngr.hpp"
#include "SerialHandler.hpp"

#include <Arduino.h>
#include <ArduinoJson.h>

void checkInputComplete();
void inputToJson();
void HandleJson();