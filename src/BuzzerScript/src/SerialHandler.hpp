#pragma once

#include <Arduino.h>

#include "Config.hpp"

void PrintButtonValueChanged(String type, int ID, bool Value);
void PrintError(String errorMSG, bool panic);