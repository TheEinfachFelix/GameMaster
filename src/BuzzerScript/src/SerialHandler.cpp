#include "SerialHandler.hpp"

void EventSender(String type, int ID, bool oldVal, bool newVal)
{
    String out = "";
    JsonDocument doc;

    doc[JsonType] = JsonEvent;
    doc[JsonIOType] = type;
    doc[JsonEventID] = ID;
    doc[JsonEventNewValue] = newVal;
    doc[JsonEventOldValue] = oldVal;

    serializeJsonPretty(doc,out);
    Serial.println(out);
}

String ErrorBuilder(String pError, bool pCritical = false)
{
    String out = "";
    JsonDocument doc;

    doc[JsonType] = JsonResponse;
    doc[JsonResponseSuccess] = false;
    doc[JsonResponseCritical] = pCritical;
    doc[JsonResponseError] = pError;

    serializeJsonPretty(doc,out);
    return out;
}

String DebugBuilder(String pSorce, String pMSG, bool pCritical, String pValue = "")
{
    String out = "";
    JsonDocument doc;

    doc[JsonType] = JsonDebug;
    doc[JsonDebugMSG] = pMSG;
    doc[JsonDebugValue] = pValue;
    doc[JsonDebugCritical] = pCritical;

    serializeJsonPretty(doc,out);
    return out;
}

String ResponseBuilder (String pValue)
{
    String out = "";
    JsonDocument doc;

    doc[JsonType] = JsonResponse;
    doc[JsonResponseSuccess] = true;
    doc[JsonResponseValue] = pValue;

    serializeJsonPretty(doc,out);
    return out; 
}