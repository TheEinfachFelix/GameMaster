#include "main.hpp"
#include <vector>
#include <list>
#include<bits/stdc++.h>



BuzzerMngr BuzzMngr = BuzzerMngr();
TasterMngr tastMngr = TasterMngr();
LEDController ledCntrl = LEDController();
String pinput;


std::list<String> readjsonToDos = {};

void split (std::string toSplit)
{
  int pos = 0;
  std::string Helper;
  std::string SplitON = "}{";

  while ((pos = toSplit.find(SplitON)) != std::string::npos)
  {
    Helper = toSplit.substr(0, pos);
    readjsonToDos.push_back(String(Helper.c_str())+"}");
    toSplit.erase(0, pos + SplitON.length());
  }
  readjsonToDos.push_back(String(toSplit.c_str()));
}

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
  
  //Serial.print(pinput);
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
  if (pinput.length() >= JsonBufferLength)
  {
    Serial.println(ErrorBuilder("Input Buffer size exceded! Dumping Buffer! The Buffer length is:" + String(pinput.length()),false));
    pinput = "";
    return;
  }
  // Evalueate the analysis
  if (countClose > countOpen) // catch more close than open
  {
    Serial.println(ErrorBuilder("there where to many } send",false));
    pinput = "";
    return;
  }
  if (countClose == countOpen && countOpen != 0) // the msg is copletly recived
  {
    split(pinput.c_str());
    pinput = "";

    for(auto i: readjsonToDos)
    {
      if (i != "")
      {
      if (i[0] != '{')
      {
        i="{"+i;
      }
      Serial.println(i);
      inputToJson(i);

      }
    }
    readjsonToDos = {};
  }
}
void inputToJson(String strJson)
{
  JsonDocument Json = JsonDocument();
  DeserializationError error = deserializeJson(Json, strJson);
  
  Serial.println(HandleJson(error, Json));
}

String HandleJson(DeserializationError pError, JsonDocument pJson)
{
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

