#include "main.hpp"

BuzzerMngr BuzzMngr = BuzzerMngr();
TasterMngr tastMngr = TasterMngr();
LEDController ledCntrl = LEDController();
String pinput;
JsonDocument readJson = JsonDocument();

void setup() {
  Serial.begin(SerialSpeed);
  Serial.setTimeout(SerialTimeout);
  Serial.setDebugOutput(true);

  tastMngr.Setup();
  BuzzMngr.Setup();
  ledCntrl.Setup();
}

void loop() 
{
  BuzzMngr.ChecknPrintPinstate();
  tastMngr.ChecknPrintPinstate();

  pinput = pinput + Serial.readString();
  //Serial.println(pinput);
  checkInputComplete();
}


void checkInputComplete()
{
  // Analise the Input
  int countOpen = 0;
  int countClose = 0;
  for (size_t i = 0; i < pinput.length(); i++)
  {
    char in = pinput[i];
    if (in  == '{')
      countOpen ++;
    if (in  == '}')
      countClose ++;
  }

  // Evalueate the analysis
  if (countClose > countOpen) // catch more close than open
  {
    PrintError("there where to many } send");
    pinput = "";
    return;
  }
  if (countClose == countOpen && countOpen != 0) // the msg is copletly recived
  {
    inputToJson();
  }
}
void inputToJson()
{
  DeserializationError error = deserializeJson(readJson, pinput);
  pinput = "";

  if (error)
  {
    PrintError("While Deserialization this happend: " + (String)error.f_str());
    return;
  }
  
  String OutputType = readJson[String(JsonType)];
  if (OutputType == "null") //catch empty json
  {
    PrintError("The Json is Empty");
    return;
  }

  HandleJson();
}
void HandleJson()
{
  String OutputType = readJson[String(JsonType)];
  if (OutputType == BuzzerType) 
  {
    BuzzMngr.SetLED(readJson[String(JsonID)],readJson[String(JsonValue)]);
  }
  if (OutputType == LEDType) 
  {
    ledCntrl.SetLED(readJson[String(JsonID)], readJson[String(JsonValue)][String("R")],
                                              readJson[String(JsonValue)][String("G")],
                                              readJson[String(JsonValue)][String("B")]);
  }
}

