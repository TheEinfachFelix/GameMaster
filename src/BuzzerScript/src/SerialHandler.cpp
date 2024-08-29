#include "SerialHandler.hpp"

void EventSender(String type, int ID, bool oldVal, bool newVal)
{
    //String strValue = "false";
    //if (Value)
    //    strValue = "true";
    
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
    out += "{\"" + String(JsonType)             + "\" = \"" + String(JsonResponse) + "\",";
    out += " \"" + String(JsonResponseSuccess)  + "\" = \"" + String(false)        + "\",";
    out += " \"" + String(JsonResponseCritical) + "\" = \"" + String(pCritical)    + "\",";
    out += " \"" + String(JsonResponseError)    + "\" = \"" + pError               + "\"}";
    return out;
}

String DebugBuilder(String pSorce, String pMSG, bool pCritical, String pValue = "")
{
    String out = "";
    out += "{\"" + String(JsonType)          + "\" = \"" + String(JsonDebug) + "\",";
    out += " \"" + String(JsonDebugMSG)      + "\" = \"" + pMSG              + "\",";
    out += " \"" + String(JsonDebugValue)    + "\" = \"" + pValue            + "\",";
    out += " \"" + String(JsonDebugCritical) + "\" = \"" + String(pCritical) + "\"}";
    return out;
}
String ResponseBuilder (String pValue)
{
    String out = "";
    out += "{\"" + String(JsonType)             + "\" = \"" + String(JsonResponse) + "\",";
    out += " \"" + String(JsonResponseSuccess)  + "\" = \"" + String(true)        + "\",";
    out += " \"" + String(JsonResponseValue)    + "\" = \"" + pValue               + "\"}";
    return out;
}