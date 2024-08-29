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
    // catch if value is missing
    if (pJson[String(JsonRequestValue)] == NULL) 
    {
        return ErrorBuilder("the key \"" + String(JsonRequestValue) + "\" is missing",true);
    }

    // handle set state
    if (Request == JsonRequestLedCollor)
    {
        return "adsf";//JsonSetLedCollor(pJson);
    }

    return ErrorBuilder("oops somthing went wrond in the BuzzerMngr-Set",true);
}
String LEDController::JsonSetLedCollor(JsonDocument pJson)
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
    int R;
    int G;
    int B;

    try
    {
        R = pJson[String(JsonRequestValue)][String("R")];
        G = pJson[String(JsonRequestValue)][String("G")];
        B = pJson[String(JsonRequestValue)][String("B")];
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder("cant convert Value to RGB",true);
    }
    Serial.println("4");
    // set state
    try
    {
        SetLED(index,R,G,B);
        return ResponseBuilder("Done");
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

    return ErrorBuilder("oops somthing went wrond in the BuzzerMngr-Get",true);
}
String LEDController::GetLedCollorAsJson(JsonDocument pJson)
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

    try
    {
        uint32_t rgbcolor = strip.getPixelColor(index);

        int r = (rgbcolor>>16)&0xFF;
        int g = (rgbcolor>>8)&0xFF;
        int b= rgbcolor&0xFF;

        String out = "";
        JsonDocument doc;

        doc["R"] = r;
        doc["G"] = g;
        doc["B"] = b;

        serializeJsonPretty(doc,out);
        return ResponseBuilder(out);
    }
    catch(const std::exception& e)
    {
        return ErrorBuilder("The ID is wrong",true);
    }
}