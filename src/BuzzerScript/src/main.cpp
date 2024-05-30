#include "main.hpp"

BuzzerMngr BuzzMngr = BuzzerMngr();
TasterMngr tastMngr = TasterMngr();
LEDController ledCntrl = LEDController();

String pinput;
JsonDocument readJson = JsonDocument();

void setup() {
  Serial.begin(9600);

  tastMngr.Setup();
  BuzzMngr.Setup();
  ledCntrl.Setup();
}

void loop() 
{
  BuzzMngr.ChecknPrintPinstate();
  tastMngr.ChecknPrintPinstate();

  pinput = pinput + Serial.readString();
  checkInputComplete();

  delay(5);
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
  if (countClose == countClose && countOpen != 0) // the msg is copletly recived
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
  
  String OutpT = readJson[String("Output_Type")];
  if (OutpT == "null") //catch empty json
  {
    PrintError("The Json is Empty");
    readJson = JsonDocument();
    return;
  }

  HandleJson();
}

void HandleJson()
{
  String OutpT = readJson[String("Output_Type")];
  if (OutpT == BuzzerType) 
  {
    BuzzMngr.SetLED(readJson[String("ID")],readJson[String("Value")]);
  }
  if (OutpT == LEDType) 
  {
    ledCntrl.SetLED(readJson[String("ID")], readJson[String("Value")][String("R")],
                                            readJson[String("Value")][String("G")],
                                            readJson[String("Value")][String("B")]);
  }
}


