#pragma once
#include "BuzzerMngr.hpp"
#include "LEDController.hpp"
#include "TasterMngr.hpp"
#include "SerialHandler.hpp"

#include <Arduino.h>
#include <ArduinoJson.h>
#include <vector>
#include <list>
#include<bits/stdc++.h>

std::list<String> SplitInputToJson (std::string toSplit);
void SplitInput();
void inputToJson(String strJson);
String HandleJson(DeserializationError pError, JsonDocument pJson);