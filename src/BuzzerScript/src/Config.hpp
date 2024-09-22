#pragma once
//////////////////// NeoPixel ////////////////////

#define LED_PIN 21 // NeoPixel LED strip
#define NUM_LEDS 12 // Number of LEDs
#define LEDType "LED"

#define CCollorDef {0,0,7}
#define CCollorPress {0,10,0}
#define CCollorBlock {10,0,0}
#define BlockTasterIndex 2 // welcher der Taster sperren kann
#define BlockTasterIsAdvanced true

//////////////////// Input ////////////////////
// Taster
#define CTasterListLength 3
#define CTasterList {Taster(40),Taster(39), Taster(38)}
#define TasterType "Taster"

// Buzzer
#define CBuzzerListLength 8
#define CBuzzerList {Buzzer(47, 48),Buzzer(13, 14),Buzzer(11, 12),Buzzer(9, 10),Buzzer(17, 18),Buzzer(15, 16),Buzzer(6,  7),Buzzer(4, 5)}
#define BuzzerType "Buzzer"

//////////////////// Serial ////////////////////
#define SerialTimeout 5
#define SerialSpeed 115200

// Json
#define JsonDeleteInputBufferAfter 100

// General
#define JsonType "Type"
#define JsonGet "Get"
#define JsonSet "Set"

//Request
#define JsonRequest "Request"
#define JsonRequestType "RequestType"
#define JsonRequestAmount "Amount"
#define JsonRequestPin "Pin"
#define JsonRequestID "ID"
#define JsonRequestState "State"
#define JsonRequestInputState "InputState"
#define JsonRequestValue "Value"
#define JsonRequestLEDPin "LedPin"
#define JsonRequestLedMode "LedMode"
#define JsonRequestLEDModeOFF "OFF"
#define JsonRequestLEDModeON "ON"
#define JsonRequestLEDModeAuto "Auto"
#define JsonRequestIsDisabeled "isDisabeled"

//Response
#define JsonResponse "Response"
#define JsonResponseSuccess "Success"
#define JsonResponseCritical "Critical"
#define JsonResponseError "Error"
#define JsonResponseValue JsonRequestValue

// Debug
#define JsonDebug "Debug"
#define JsonDebugMSG "MSG"
#define JsonDebugCritical JsonResponseCritical
#define JsonDebugValue JsonRequestValue

// Event
#define JsonEvent "Event"
#define JsonIOType "IOType"
#define JsonEventID JsonRequestID
#define JsonEventNewValue "NewValue"
#define JsonEventOldValue "OldValue"

// Error msg
#define JsonErrorGeneric "Something went wrong"
/*
{"Type":"Request","IO-Type":"Buzzer","RequestType":"Set", "Request":"State","ID":7,"Value": 1}
*/