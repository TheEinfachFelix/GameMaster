#pragma once
#include <Arduino.h>
#include <ArduinoJson.h>

#include "buzzer.hpp"
#include "Config.hpp"
#include "SerialHandler.hpp"

class BuzzerMngr
{
private:
    // SET
    void SetLedState(int BuzzerID, bool Value);
    void SetInputState(int BuzzerID, bool Value);

    String JsonSetHandler(JsonDocument pJson);

    // GET
    String JsonGetHandler(JsonDocument pJson);

public:
    Buzzer BuzzerList[CBuzzerListLength] = CBuzzerList;

    BuzzerMngr();
    ~BuzzerMngr();
    void Setup();

    void CheckAllInputChanges();

    String JsonHandlerGetSet(JsonDocument pJson);
};
