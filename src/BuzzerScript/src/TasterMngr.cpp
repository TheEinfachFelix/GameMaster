#include "TasterMngr.hpp"
#include "Config.hpp"

TasterMngr::TasterMngr(){}
TasterMngr::~TasterMngr(){}

void TasterMngr::Setup()
{
    // Set the ID of the Taster
    int IDCounter = 0;
    for(auto &i: TasterList)
    {
        i.ID = IDCounter;
        IDCounter ++;
    }
}

void TasterMngr::CheckAllInputChanges()
{
    for(auto &i: TasterList)
    {
        i.CheckStateChange();
    }
}

String TasterMngr::JsonHandlerGetSet(JsonDocument pJson)
{
    String Request = pJson[String(JsonRequest)];
    int index= pJson[String(JsonRequestID)];
    Taster Tas = TasterList[index];
    bool val  = pJson[String(JsonRequestValue)];

    // catch Set
    if (pJson[String(JsonRequestType)] == JsonSet) 
    {
        if (Request != JsonRequestInputState)
        {
            return ErrorBuilder("Taster only has \"" + String(JsonRequestInputState) + "\" to SET",true);
        }
        // SetInputState
        try
        {
            Tas.PrintEvent(Tas.GetInputState(),val);
            return ResponseBuilder("Done");
        }
            catch(const std::exception& e)
        {
            return ErrorBuilder(JsonErrorGeneric,true);
        }
    }

    // handle amount
    if (Request == JsonRequestAmount)
    {
        int len = sizeof(TasterList)/sizeof(TasterList[0]);
        return ResponseBuilder(String(len));
    }

    // handle pin
    if (Request == JsonRequestPin)
    {
        try
        {
            int pin = Tas.GetPin();
            return ResponseBuilder(String(pin));
        }
        catch(const std::exception& e)
        {
            return ErrorBuilder(JsonErrorGeneric,true);
        }
    }

    // handle state
    if (Request == JsonRequestInputState)
    {
        // get data
        try
        {
            bool pin = Tas.GetInputState();
            return ResponseBuilder(BoolToString(pin));
        }
        catch(const std::exception& e)
        {
            return ErrorBuilder(JsonErrorGeneric,true);
        }
    }
    
    return ErrorBuilder("Request not found! Only requests are \""  + String(JsonRequestAmount) + "\" or \""  + String(JsonRequestPin) + "\"",true);
}