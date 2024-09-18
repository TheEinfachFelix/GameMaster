#include "LEDController.hpp"

LEDController::LEDController(){}
LEDController::~LEDController(){}

void LEDController::Setup()
{
    strip.begin();
    strip.show();
}

void LEDController::SetLED(int id, int R = 0, int G = 0, int B = 0)
{
    strip.setPixelColor(id, R, G, B);
    strip.show();
}

String LEDController::JsonHandlerGetSet(JsonDocument pJson)
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
// Set
String LEDController::JsonSetHandler(JsonDocument pJson)
{
    String Request = pJson[String(JsonRequest)];

    // handle set state
    if (Request == JsonRequestLedCollor)
    {
        return JsonSetCollor(pJson);
    }

    return ErrorBuilder("Only requests for Get are: \""+String(JsonRequestLedCollor)+"\"",true);
}
String LEDController::JsonSetCollor(JsonDocument pJson)
{
    // convert ID to int
    int index  = pJson[String(JsonRequestID)];

    //convert value to bool
    int R;
    int G;
    int B;

    try
    {
        R = pJson[String(JsonRequestValueR)];
        G = pJson[String(JsonRequestValueG)];
        B = pJson[String(JsonRequestValueB)];
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder("cant convert Value to RGB",true);
    }

    // set state
    try
    {
        SetLED(index,R,G,B);
        return ResponseBuilder("Done - Be aware that this method has defauld values of 0 for \""+String(JsonRequestID)+"\", \""+String(JsonRequestValueR)+"\", \""+String(JsonRequestValueG)+"\" and \""+String(JsonRequestValueB)+"\"! And be aware that if the index is out of range no error is thrown!");
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder(JsonErrorGeneric,true);
    }
}
// Get
String LEDController::JsonGetHandler(JsonDocument pJson)
{
    String Request = pJson[String(JsonRequest)];

    // handle amount
    if (Request == JsonRequestAmount)
    {
        return ResponseBuilder(String(NUM_LEDS));
    }

    // handle pin
    if (Request == JsonRequestPin)
    {
        return ResponseBuilder(String(LED_PIN));
    }

    if (Request == JsonRequestLedCollor){
        return JsonGetCollor(pJson);
    }

    return ErrorBuilder("Only requests for Get are: \""+String(JsonRequestAmount)+"\", \""+String(JsonRequestPin)+"\", \""+String(JsonRequestLedCollor)+"\"",true);
}
String LEDController::JsonGetCollor(JsonDocument pJson)
{
    // convert ID to int
    int index = pJson[String(JsonRequestID)];
    String key = pJson[String(JsonRequestKey)];

    try
    {
        uint32_t rgbcolor = strip.getPixelColor(index);

        int r = (rgbcolor>>16)&0xFF;
        int g = (rgbcolor>>8)&0xFF;
        int b= rgbcolor&0xFF;

        String out = "";

        if (key == JsonRequestValueR)
        {
            return ResponseBuilder(String(r));
        }
        if (key == JsonRequestValueG)
        {
            return ResponseBuilder(String(g));
        }
        if (key == JsonRequestValueB)
        {
            return ResponseBuilder(String(b));
        }

        return ErrorBuilder("the key \"" + String(JsonRequestKey)+"\" does not have a valide Key. mus be :" + String(JsonRequestValueR) + String(JsonRequestValueG) + String(JsonRequestValueB), true);
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder(JsonErrorGeneric,true);
    }
}