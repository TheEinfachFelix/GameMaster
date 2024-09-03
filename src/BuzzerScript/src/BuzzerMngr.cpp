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

    // handle set state
    if (Request == JsonRequestState)
    {
        return JsonSetLedState(pJson);
    }

    return ErrorBuilder("Only requests for Get are: \""+String(JsonRequestState)+"\"",true);

}
String BuzzerMngr::JsonSetLedState(JsonDocument pJson)
{
    // convert ID to int
    int index = pJson[String(JsonRequestID)];

    //convert value to bool
    bool val  = pJson[String(JsonRequestValue)];

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

    if (Request == JsonRequestState)
    {
        // convert ID to int
        int index= pJson[String(JsonRequestID)];

        // get data
        try
        {
            bool pin = BuzzerList[index].GetLedState();
            return ResponseBuilder(String(pin));
        }
        catch(const std::exception& e)
        {
            return ErrorBuilder("The ID is wrong",true);
        }
    }

    return ErrorBuilder("Only requests for Get are: \""+String(JsonRequestLEDPin)+"\", \""+String(JsonRequestTasterPin)+"\", \""+String(JsonRequestState)+"\" and \""+String(JsonRequestAmount)+"\"",true);
}
String BuzzerMngr::JsonGetPin(JsonDocument pJson)
{

    // convert ID to int
    int index= pJson[String(JsonRequestID)];

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
