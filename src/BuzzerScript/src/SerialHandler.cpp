#include "SerialHandler.hpp"

void PrintButtonValueChanged(String type, int ID, bool Value)
{
    String strValue = "false";
    if (Value)
        strValue = "true";
    
    Serial.print("{\""+String(JsonType)+"\" : \"" + type + "\" ,\""+String(JsonID)+"\" : ");
    Serial.print(ID);
    Serial.println(", \""+String(JsonValue)+"\" : " + strValue + " }");
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