#include "BuzzerMngr.hpp"
#include "Config.hpp"

Buzzer BuzzerList[] = CBuzzerList;

BuzzerMngr::BuzzerMngr()
{
}

BuzzerMngr::~BuzzerMngr()
{
}

void BuzzerMngr::Setup()
{
    // Set the ID of the Buzzer
    int IDCounter = 0;
    for(auto &i: BuzzerList)
    {
        i.ID = IDCounter;
        IDCounter ++;
    }
}

void BuzzerMngr::ChecknPrintPinstate()
{
    for(auto &i: BuzzerList)
    {
        i.PrintRead();
    }
}

void BuzzerMngr::SetLED(int BuzzerID, bool Value)
{
    BuzzerList[BuzzerID].SetLED(Value);
}

String BuzzerMngr::JsonHandler(JsonDocument pJson)
{
    String Request = pJson[String(JsonRequest)];
    // validate request
    if (Request != JsonRequestAmount && Request != JsonRequestLEDPin && Request != JsonRequestTasterPin && Request != JsonRequestState) 
    {
        return ErrorBuilder("Request not found! Only requests are \""  + String(JsonRequestAmount) + "\" or \"" + String(JsonRequestState) + "\" or \"" + String(JsonRequestTasterPin) + "\" or \"" + String(JsonRequestLEDPin) + "\"",true);
    }

    if (pJson[String(JsonRequestType)] == JsonSet) 
    {
        return JsonSetHandler(pJson);
    }

    if (pJson[String(JsonRequestType)] == JsonGet) 
    {
        return JsonGetHandler(pJson);
    }

    return ErrorBuilder("oops somthing went wrong in BuzzerMngr",true);
}
String BuzzerMngr::JsonSetHandler(JsonDocument pJson)
{
    String Request = pJson[String(JsonRequest)];
    // catch if value is missing
    if (pJson[String(JsonRequestValue)] == NULL) 
    {
        return ErrorBuilder("the key \"" + String(JsonRequestValue) + "\" is missing",true);
    }

    // handle set state
    if (Request == JsonRequestState)
    {
        return JsonSetLedState(pJson);
    }

    return ErrorBuilder("oops somthing went wrond in the BuzzerMngr-Set",true);
}
String BuzzerMngr::JsonSetLedState(JsonDocument pJson)
{
    // check ID
    if (pJson[String(JsonRequestID)] == NULL) 
    {
        return ErrorBuilder("The ID is missig",true);
    }

    // convert ID to int
    int index;
    try
    {
        index = pJson[String(JsonRequestID)];
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder("cant convert ID to int",true);
    }

    //convert value to bool
    bool val;
    try
    {
        val = pJson[String(JsonRequestValue)];
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder("cant convert Value to bool",true);
    }
    
    // set state
    try
    {
        SetLED(index,val);
        return ResponseBuilder("Done");
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder("The ID is wrong",true);
    }
}
String BuzzerMngr::JsonGetHandler(JsonDocument pJson)
{
    String Request = pJson[String(JsonRequest)];

    // handle amount
    if (Request == JsonRequestAmount)
    {
        int len = sizeof(BuzzerList)/sizeof(BuzzerList[0]);
        return ResponseBuilder(String(len));
    }

    // handle pin
    if (Request == JsonRequestLEDPin || Request == JsonRequestTasterPin)
    {
        return JsonGetPin(pJson);
    }

    return ErrorBuilder("oops somthing went wrond in the BuzzerMngr-Get",true);
}
String BuzzerMngr::JsonGetPin(JsonDocument pJson)
{
    // check id key exists
    if (pJson[String(JsonRequestID)] == NULL) 
    {
        return ErrorBuilder("The ID is missig",true);
    }

    // convert ID to int
    int index;
    try
    {
        index = pJson[String(JsonRequestID)];
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder("cant convert ID to int",true);
    }
    
    // get data
    try
    {
        int pin = BuzzerList[index].GetBuzPin();
        if (pJson[String(JsonRequest)] == JsonRequestLEDPin)
        {
            pin = BuzzerList[index].GetLedPin();
        }
        return ResponseBuilder(String(pin));
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder("The ID is wrong",true);
    }
}
