#include "JsonHandler.hpp"

String Input;
String OldInput;
int64_t TimerOfDeletion;

String IOTypeNotexisting = ErrorBuilder("the IOType is wrong.",true);

void SetupJson()
{
    Serial.begin(SerialSpeed);
    Serial.setTimeout(SerialTimeout);
    Serial.setDebugOutput(true);

    TimerOfDeletion = millis();
}
void LoopJson()
{
    Input = Input + Serial.readString();

    for(auto i: SplitInputToJson(Input.c_str()))
    {
        inputToJson(i);
    }

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
    return ErrorBuilder("During Deserialization this error ocured: " + (String)pError.f_str(),false);

  //catch empty json
  if (pJson[String(JsonType)] == "null")
    return ErrorBuilder("The Json is Empty or key \"" + String(JsonType) + "\" ist missing",false);

  // check if Request is Request
  if (pJson[String(JsonType)] != JsonRequest) 
    return ErrorBuilder("we only handle \"" + String(JsonType) + "\" json \"" + String(JsonRequest) + "\"",false);

  // validate get set
  if (pJson[String(JsonRequestType)] != JsonGet && pJson[String(JsonRequestType)] != JsonSet) 
    return ErrorBuilder("error with key \"" + String(JsonRequestType) + "\" must be \""  + String(JsonSet) + "\" or \""  + String(JsonGet) + "\"",true);


    return HandleRequest(pJson);
}

String HandleRequest(JsonDocument pJson)
{
    String Request = pJson[String(JsonRequest)];

    if (Request == JsonRequestPin)
        return JsonPinHandler(pJson);
    if (Request == JsonRequestLEDPin)
        return JsonLEDPinHandler(pJson);
    if (Request == JsonRequestAmount)
        return JsonAmountHandler(pJson);
    if (Request == JsonRequestInputState)
        return JsonInputStateHandler(pJson);
    if (Request == JsonRequestState)
        return JsonStateHandler(pJson);
    if (Request == JsonRequestLedMode)
        return JsonLedModeHandler(pJson);
    if (Request == JsonRequestIsDisabeled)
        return JsonDisableHandler(pJson);
    
    return ErrorBuilder("The Request was not found",true);
}


String JsonPinHandler(JsonDocument pJson)
{
    if (pJson[String(JsonRequestType)] != JsonGet)
        return ErrorBuilder("The Pin can only Get",true);
    
    String IOType = pJson[String(JsonIOType)];
    if (IOType == LEDType)
        return ResponseBuilder(String(LED_PIN));

    int Index = pJson[String(JsonRequestID)];

    if (IOType == BuzzerType)
        return ResponseBuilder(String(BuzzMngr.BuzzerList[Index].GetInputPin()));
    if (IOType == TasterType)
        return ResponseBuilder(String(tastMngr.TasterList[Index].GetPin()));
    
    return IOTypeNotexisting;
}
String JsonLEDPinHandler(JsonDocument pJson)
{
    if (pJson[String(JsonRequestType)] != JsonGet)
        return ErrorBuilder("The Pin can only Get",true);
    
    String error = ErrorBuilder("only suported by the Buzzer",true);
    String IOType = pJson[String(JsonIOType)];
    int Index = pJson[String(JsonRequestID)];
    
    if (IOType == LEDType)
        return error;
    if (IOType == BuzzerType)
        return ResponseBuilder(String(BuzzMngr.BuzzerList[Index].GetLedPin()));
    if (IOType == TasterType)
        return error;
    
    return IOTypeNotexisting;
}
String JsonAmountHandler(JsonDocument pJson)
{
    if (pJson[String(JsonRequestType)] != JsonGet)
        return ErrorBuilder("The Amount can only Get",true);
    
    String IOType = pJson[String(JsonIOType)];
    
    if (IOType == LEDType)
        return ResponseBuilder(String(NUM_LEDS));
    if (IOType == BuzzerType)
        return ResponseBuilder(String(CBuzzerListLength));
    if (IOType == TasterType)
        return ResponseBuilder(String(CTasterListLength));
    
    return IOTypeNotexisting;
}
String JsonInputStateHandler(JsonDocument pJson)
{
    String IOType = pJson[String(JsonIOType)];
    String RType = pJson[String(JsonRequestType)];
    int Index = pJson[String(JsonRequestID)];
    bool Value = pJson[String(JsonRequestValue)];

    if (IOType == LEDType)
        return ErrorBuilder("you can not get the input State of the LED ",true);
    if (IOType == BuzzerType)
    {
        Buzzer buz = BuzzMngr.BuzzerList[Index];
        if (RType == JsonGet)
            return ResponseBuilder(String(buz.GetInputState()));
        //  this is nececary to not cause issues on the other ende
        Serial.println(ResponseBuilder("Done"));
        buz.PrintEvent(buz.GetInputState(),Value);
        return "";
    }
    if (IOType == TasterType)
    {
        Taster tast = tastMngr.TasterList[Index];
        if (RType == JsonGet)
            return ResponseBuilder(String(tast.GetInputState()));
        Serial.println(ResponseBuilder("Done"));
        tast.PrintEvent(tast.GetInputState(),Value);
        return "";
    }
    return IOTypeNotexisting;
}
String JsonStateHandler(JsonDocument pJson)
{
    String IOType = pJson[String(JsonIOType)];
    String RType = pJson[String(JsonRequestType)];
    int Index = pJson[String(JsonRequestID)];
    bool Value = pJson[String(JsonRequestValue)];

    if (IOType == LEDType)
        return ErrorBuilder("you can not get the input State of the LED ",true);
    if (IOType == BuzzerType)
    {
        Buzzer buz = BuzzMngr.BuzzerList[Index];
        if (RType == JsonGet)
            return ResponseBuilder(String(buz.GetLedState()));
        buz.SetLedValue(Value);
        return ResponseBuilder("Done");
    }
    if (IOType == TasterType)
        return ErrorBuilder("you can not get the (LED) State of the LED ",true);

    return IOTypeNotexisting;
}
String JsonLedModeHandler(JsonDocument pJson)
{
    String IOType = pJson[String(JsonIOType)];
    String RType = pJson[String(JsonRequestType)];
    String Value = pJson[String(JsonRequestValue)];

    if (IOType == LEDType)
    {
        if (RType == JsonGet)
            return ResponseBuilder(String(ledCntrl.Mode));
        if (Value != JsonRequestLEDModeAuto && Value != JsonRequestLEDModeOFF)
            return ErrorBuilder("The Value must be: \"" + String(JsonRequestLEDModeAuto)+  "\", \"" +String(JsonRequestLEDModeOFF)+ "\", \"",true);
        ledCntrl.Mode = Value;
        return ResponseBuilder("Done");
    }   
    if (IOType == BuzzerType)
    {
        if (RType == JsonGet)
            return ResponseBuilder(String(BuzzMngr.Mode));
        if (Value != JsonRequestLEDModeAuto && Value != JsonRequestLEDModeOFF && Value != JsonRequestLEDModeON)
            return ErrorBuilder("The Value must be: \"" + String(JsonRequestLEDModeAuto)+ "\", \"" + String(JsonRequestLEDModeON)+ "\", \"" + String(JsonRequestLEDModeOFF)+ "\", \"",true);
        BuzzMngr.Mode = Value;
        return ResponseBuilder("Done");
    }
    if (IOType == TasterType)
        return ErrorBuilder("the Taster does not have a LED",true);

    return IOTypeNotexisting;
}
String JsonDisableHandler(JsonDocument pJson)
{
    String IOType = pJson[String(JsonIOType)];
    String RType = pJson[String(JsonRequestType)];
    bool Value = pJson[String(JsonRequestValue)];

    if (IOType == LEDType)
        return ErrorBuilder("ony for Buzzer",true);
    if (IOType == BuzzerType)
    {
        if (RType == JsonGet)
            return ResponseBuilder(String(BuzzMngr.isDisabeled));
        BuzzMngr.isDisabeled = Value;
        return ResponseBuilder("Done");
    }
    if (IOType == TasterType)
        return ErrorBuilder("only for Buzzer",true);

    return IOTypeNotexisting;
}