#include <Arduino.h>
#include <ArduinoJson.h>
#include<bits/stdc++.h>



void setup() {


}

void loop() {
  // put your main code here, to run repeatedly:
  delay(10000);

  std::string toSplit = "{\"Type\":\"Request\",\"IO-Type\":\"LED\",\"RequestType\":\"Set\", \"Request\":\"Collor\"}";
  std::list<String> OutputList = {};
  std::smatch smatch;
  std::regex regex ("\\{.*?\\}");
  
  Serial.println(std::regex_search (toSplit,smatch,regex));
}