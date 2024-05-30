#include "SerialHandler.hpp"

void PrintButtonValueChanged(String type, int ID, bool Value)
{
    String strValue = "false";
    if (Value)
        strValue = "true";
    
    Serial.print("{\"Input_Type\" = \"" + type + "\" ,\"ID\" = ");
    Serial.print(ID);
    Serial.println(", \"value\" = " + strValue + " }");
}

void PrintError(String errorMSG)
{
    Serial.print("\"Error\":\"");
    Serial.print(errorMSG);
    Serial.println("\"}");
}