#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>

#include "Config.hpp"

void EventSender(String type, int ID, bool oldVal, bool newVal);
String ErrorBuilder(String pError, bool pCritical);
String DebugBuilder(String pSorce, String pMSG, bool pCritical, String pValue);
String ResponseBuilder (String pValue);
String BoolToString (bool input);