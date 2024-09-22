#include "main.hpp"

BuzzerMngr BuzzMngr = BuzzerMngr();
TasterMngr tastMngr = TasterMngr();
LEDController ledCntrl = LEDController();
String Input;
String OldInput;
int64_t TimerOfDeletion;
bool ProcessingRequestHold = false;

std::list<String> SplitInputToJson (std::string toSplit)
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
  if (!ProcessingRequestHold)
  {
    BuzzMngr.CheckAllInputChanges();
    tastMngr.CheckAllInputChanges();
  }

  Input = Input + Serial.readString();

  SplitInput();

  // Delete the Input if it is not used
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

void SplitInput()
{
  for(auto i: SplitInputToJson(Input.c_str()))
  {
    inputToJson(i);
  }
}

void inputToJson(String strJson)
{
  // fix missing last } on nested json string
  int countOpen = 0;
  int countClose = 0;
  // count
  for (size_t i = 0; i < strJson.length(); i++)
  {
    char in = strJson[i];
    if (in  == '{')
      countOpen ++;
    if (in  == '}')
      countClose ++;
  }
  // act
  if (countClose + 1 == countOpen)
  {
    strJson = strJson + "}";
  }

  // deserialise
  JsonDocument Json = JsonDocument();
  DeserializationError error = deserializeJson(Json, strJson);
  
  // output the result
  ProcessingRequestHold = true;
  Serial.println(HandleJson(error, Json));
  ProcessingRequestHold = false;
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

  // check if Request is Request
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
    return ledCntrl.JsonHandlerGetSet(pJson);
  }
    if (IOType == BuzzerType)
  {
    return BuzzMngr.JsonHandlerGetSet(pJson);

  }
    if (IOType == TasterType)
  {
    return tastMngr.JsonHandlerGetSet(pJson);
  }

  return ErrorBuilder("The Json Request left me with nothing to do sus",true); 
}

