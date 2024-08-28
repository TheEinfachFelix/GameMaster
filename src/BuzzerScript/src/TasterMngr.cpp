#include "TasterMngr.hpp"
#include "Config.hpp"

Taster TasterList[] = CTasterList;

TasterMngr::TasterMngr()
{
}

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

TasterMngr::~TasterMngr()
{
}

void TasterMngr::ChecknPrintPinstate()
{
    for(auto &i: TasterList)
    {
        i.PrintRead();
    }
}

String TasterMngr::JsonHandler(JsonDocument pJson)
{
    // catch Set
    if (pJson[String(JsonRequestType)] == JsonSet) 
    {
        return ErrorBuilder("Taster has nothing to SET",true);
    }

    String Request = pJson[String(JsonRequest)];
    // validate request
    if (Request != JsonRequestAmount && Request != JsonRequestPin) 
    {
        return ErrorBuilder("Request not found! Only requests are \""  + String(JsonRequestAmount) + "\" or \""  + String(JsonRequestPin) + "\"",true);
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
            int pin = TasterList[index].GetPin();
            String out = ResponseBuilder(String(pin));
            return out;
        }
        catch(const std::exception& e)
        {
            return ErrorBuilder("The ID is wrong",true);
        }

    }

    return ErrorBuilder("Somthing went wrong in TasterMnrg");

}