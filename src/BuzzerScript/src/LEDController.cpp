#include "LEDController.hpp"

LEDController::LEDController()
{
}

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

LEDController::~LEDController()
{
}

String LEDController::JsonHandler(JsonDocument pJson)
{
    String Request = pJson[String(JsonRequest)];
    // validate request
    if (Request != JsonRequestAmount && Request != JsonRequestPin && Request != JsonRequestLedCollor) 
    {
        return ErrorBuilder("\"" + String(JsonRequest) + "\" not found! Only requests are \""  + String(JsonRequestAmount) + "\" or \"" + String(JsonRequestLedCollor) + "\" or \"" + String(JsonRequestPin) + "\"",true);
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
String LEDController::JsonSetHandler(JsonDocument pJson)
{
    String Request = pJson[String(JsonRequest)];

    // handle set state
    if (Request == JsonRequestLedCollor)
    {
        return JsonSetLedCollor(pJson);
    }

    return ErrorBuilder("Only requests for Get are: \""+String(JsonRequestLedCollor)+"\"",true);
}
String LEDController::JsonSetLedCollor(JsonDocument pJson)
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
        return ErrorBuilder("The ID is wrong",true);
    }
}
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
        return GetLedCollorAsJson(pJson);
    }

    return ErrorBuilder("Only requests for Get are: \""+String(JsonRequestAmount)+"\", \""+String(JsonRequestPin)+"\", \""+String(JsonRequestLedCollor)+"\"",true);
}
String LEDController::GetLedCollorAsJson(JsonDocument pJson)
{
    // convert ID to int
    int index = pJson[String(JsonRequestID)];

    try
    {
        uint32_t rgbcolor = strip.getPixelColor(index);

        int r = (rgbcolor>>16)&0xFF;
        int g = (rgbcolor>>8)&0xFF;
        int b= rgbcolor&0xFF;

        String out = "";
        JsonDocument doc;

        doc[JsonRequestValueR] = r;
        doc[JsonRequestValueG] = g;
        doc[JsonRequestValueB] = b;

        serializeJsonPretty(doc,out);
        return ResponseBuilder(out);
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder("The ID is wrong",true);
    }
}