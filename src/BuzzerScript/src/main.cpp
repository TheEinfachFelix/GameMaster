#include "main.hpp"
#include <vector>
#include <list>
#include<bits/stdc++.h>



BuzzerMngr BuzzMngr = BuzzerMngr();
TasterMngr tastMngr = TasterMngr();
LEDController ledCntrl = LEDController();
String Input;
String OldInput;
int64_t TimerOfDeletion;

std::list<String> split (std::string toSplit)
{
  
  std::list<String> OutputList = {};
  auto foundOpen = toSplit.find("{");
  auto foundClosed = toSplit.find("}");

  while (foundOpen != std::string::npos && foundClosed != std::string::npos)
  {
    OutputList.push_back(String(toSplit.substr(foundOpen , foundOpen-foundClosed).c_str())); 
    toSplit = toSplit.substr(foundClosed+1);

    foundOpen = toSplit.find("{");
    foundClosed = toSplit.find("}");
  }
  Input = String(toSplit.c_str());
  return OutputList;
}

void setup() {
  Serial.begin(SerialSpeed);
  Serial.setTimeout(SerialTimeout);
  Serial.setDebugOutput(true);

  tastMngr.Setup();
  BuzzMngr.Setup();
  ledCntrl.Setup();
  TimerOfDeletion = millis();
}

void loop() 
{
  BuzzMngr.ChecknPrintPinstate();
  tastMngr.ChecknPrintPinstate();

  Input = Input + Serial.readString();

  //Serial.print(Input);
  checkInputComplete();

  if (millis() > TimerOfDeletion + JsonDeleteInputBufferAfter)
  {
    if (OldInput == Input)
    {
      Input = "";
    }
    TimerOfDeletion = millis();
    OldInput = Input;
  }
}

void checkInputComplete()
{
  for(auto i: split(Input.c_str()))
  {
    inputToJson(i);
    Serial.print(Input); 
  }
}
void inputToJson(String strJson)
{
  // fix missing last } on nested json string
  int countOpen = 0;
  int countClose = 0;
  for (size_t i = 0; i < strJson.length(); i++)
  {
    char in = strJson[i];
    if (in  == '{')
      countOpen ++;
    if (in  == '}')
      countClose ++;
  }
  if (countClose + 1 == countOpen)
  {
    strJson = strJson + "}";
  }

  // deserialise
  JsonDocument Json = JsonDocument();
  DeserializationError error = deserializeJson(Json, strJson);
  
  Serial.println(HandleJson(error, Json));
}

String HandleJson(DeserializationError pError, JsonDocument pJson)
{
  Serial.println(DebugBuilder("main","something new", false,""));
  // catch deserilaising error
  if (pError)
  {
    return ErrorBuilder("During Deserialization this error ocured: " + (String)pError.f_str(),false);
  }

  //catch empty json
  if (pJson[String(JsonType)] == "null")
  {
    return ErrorBuilder("The Json is Empty or key \"" + String(JsonType) + "\" ist missing",false);
  }

  //validate type
  if (pJson[String(JsonType)] != JsonRequest) 
  {
    return ErrorBuilder("we only handle \"" + String(JsonType) + "\" json \"" + String(JsonRequest) + "\"",true);
  }

  // validate IOType
  String IOType = pJson[String(JsonIOType)];
  if (IOType != LEDType && IOType != BuzzerType && IOType != TasterType)
  {
    return ErrorBuilder("the value from key \"" + String(JsonIOType) + "\" is not valide. Must be \""  + String(LEDType) + "\" or \"" + String(BuzzerType) + "\" or \"" + String(TasterType) + "\"",true);
  }

  // validate get set
  if (pJson[String(JsonRequestType)] != JsonGet && pJson[String(JsonRequestType)] != JsonSet) 
  {
    return ErrorBuilder("error with key \"" + String(JsonRequestType) + "\" must be \""  + String(JsonSet) + "\" or \""  + String(JsonGet) + "\"",true);
  }

  // give handling to mngr
  if (IOType == LEDType)
  {
    return ledCntrl.JsonHandler(pJson);
  }
    if (IOType == BuzzerType)
  {
    return BuzzMngr.JsonHandler(pJson);

  }
    if (IOType == TasterType)
  {
    return tastMngr.JsonHandler(pJson);
  }

  return ErrorBuilder("The Json Request left me with nothing to do sus",true); 
}

