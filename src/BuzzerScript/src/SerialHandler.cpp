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

void PrintError(String errorMSG)
{
    Serial.print("\""+String(JsonError)+"\":\"");
    Serial.print(errorMSG);
    Serial.println("\"}");
}