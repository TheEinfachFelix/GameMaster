#pragma once

#include <Arduino.h>

#include "Config.hpp"

void PrintButtonValueChanged(String type, int ID, bool Value);
String ErrorBuilder(String pError, bool pCritical = false);
String DebugBuilder(String pSorce, String pMSG, bool pCritical, String pValue = "");
String ResponseBuilder (String pValue);