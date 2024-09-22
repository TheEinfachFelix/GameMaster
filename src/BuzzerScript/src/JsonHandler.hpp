#pragma once

#include "hardware.hpp"

#include "SerialHandler.hpp"

#include <Arduino.h>
#include <ArduinoJson.h>
#include <vector>
#include <list>
#include <bits/stdc++.h>

extern String Input;
extern String OldInput;
extern int64_t TimerOfDeletion;


void SetupJson();
void LoopJson();

std::list<String> SplitInputToJson (std::string toSplit);
void inputToJson(String strJson);
String HandleJson(DeserializationError pError, JsonDocument pJson);