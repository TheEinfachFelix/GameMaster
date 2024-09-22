#include "BuzzerMngr.hpp"
#include "Config.hpp"




BuzzerMngr::BuzzerMngr()
{
    
}
BuzzerMngr::~BuzzerMngr(){}

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

void BuzzerMngr::CheckAllInputChanges()
{
    for(auto &i: BuzzerList)
    {
        i.CheckStateChange();
    }
}

// Set States
void BuzzerMngr::SetLedState(int BuzzerID, bool Value)
{
    BuzzerList[BuzzerID].SetLedValue(Value);
}
void BuzzerMngr::SetInputState(int BuzzerID, bool Value)
{
    Buzzer buz = BuzzerList[BuzzerID];
    buz.PrintEvent(buz.GetInputState(),Value);
}

// Handle json
String BuzzerMngr::JsonHandlerGetSet(JsonDocument pJson)
{
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
// Json SET
String BuzzerMngr::JsonSetHandler(JsonDocument pJson)
{
    String Request = pJson[String(JsonRequest)];
    int index = pJson[String(JsonRequestID)];
    bool val  = pJson[String(JsonRequestValue)];

    try
    {
        if (Request == JsonRequestState)
        {
            SetLedState(index,val);
            return ResponseBuilder("Done");
        }
        if (Request == JsonRequestInputState)
        {
            SetInputState(index,val);
            return ResponseBuilder("Done");
        } 
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder(JsonErrorGeneric,true);
    }


    return ErrorBuilder("Only requests for Get are: \""+String(JsonRequestState)+"\", \""+String(JsonRequestInputState)+"\"",true);
}
// Json GET
String BuzzerMngr::JsonGetHandler(JsonDocument pJson)
{
    String Request = pJson[String(JsonRequest)];
    int index= pJson[String(JsonRequestID)];
    Buzzer Buz = BuzzerList[index];

    if (Request == JsonRequestAmount)
    {
        int len = sizeof(BuzzerList)/sizeof(BuzzerList[0]);
        return ResponseBuilder(String(len));
    }

    if (Request == JsonRequestLEDPin)
    {
        try
        {
            int pin = Buz.GetLedPin();
            return ResponseBuilder(String(pin));
        }
        catch(const std::exception& e)
        {
            return ErrorBuilder(JsonErrorGeneric,true);
        }
    } 

    if (Request == JsonRequestTasterPin)
    {
        try
        {
            int pin = Buz.GetInputPin();
            return ResponseBuilder(String(pin));
        }
        catch(const std::exception& e)
        {
            return ErrorBuilder(JsonErrorGeneric,true);
        }
    } 

    if (Request == JsonRequestState)
    {
        try
        {
            bool pin = Buz.GetLedState();
            return ResponseBuilder(BoolToString(pin));
        }
        catch(const std::exception& e)
        {
            return ErrorBuilder(JsonErrorGeneric,true);
        }
    }

    if (Request == JsonRequestInputState)
    {
        try
        {
            bool pin= Buz.GetInputState();
            return ResponseBuilder(BoolToString(pin));
        }
        catch(const std::exception& e)
        {
            return ErrorBuilder(JsonErrorGeneric,true);
        }
    }

    return ErrorBuilder("Only requests for Get are: \""+String(JsonRequestLEDPin)+"\", \""+String(JsonRequestTasterPin)+"\", \""+String(JsonRequestState)+"\" and \""+String(JsonRequestAmount)+"\"and \""+String(JsonRequestInputState)+"\"",true);
}
