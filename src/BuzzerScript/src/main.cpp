#include "main.hpp"
#include <vector>
#include <list>
#include<bits/stdc++.h>



BuzzerMngr BuzzMngr = BuzzerMngr();
TasterMngr tastMngr = TasterMngr();
LEDController ledCntrl = LEDController();
String pinput;
JsonDocument readJson = JsonDocument();

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
    PrintError("Input Buffer size exceded! Dumping Buffer! The Buffer length is:" + String(pinput.length()),false);
    pinput = "";
    return;
  }
  // Evalueate the analysis
  if (countClose > countOpen) // catch more close than open
  {
    PrintError("there where to many } send",false);
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
  DeserializationError error = deserializeJson(readJson, strJson);
  
  if (error)
  {
    PrintError("While Deserialization this happend: " + (String)error.f_str(),false);
    return;
  }

  if (readJson[String(JsonType)] == "null") //catch empty json
  {
    PrintError("The Json is Empty",false);
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

