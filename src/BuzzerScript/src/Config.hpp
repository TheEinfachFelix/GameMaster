#pragma once
//////////////////// NeoPixel ////////////////////

#define LED_PIN 21 // NeoPixel LED strip
#define NUM_LEDS 12 // Number of LEDs
#define LEDType "LED"

//////////////////// Input ////////////////////
// Taster
#define CTasterList {Taster(40),Taster(39), Taster(38)}
#define TasterType "Taster"

// Buzzer
#define CBuzzerList {Buzzer(47, 48),Buzzer(13, 14),Buzzer(11, 12),Buzzer(9, 10),Buzzer(17, 18),Buzzer(15, 16),Buzzer(6,  7),Buzzer(4, 5)}
#define BuzzerType "Buzzer"

//////////////////// Serial ////////////////////
#define SerialTimeout 5
#define SerialSpeed 115200

// Json Keys
#define JsonError "Error"
#define JsonID "ID"
#define JsonValue "Value"
#define JsonRequest "Request"
#define JsonGetVal "Get"
#define JsonBufferLength 1000
#define JsonErrorPanic "ErrorPanic"

// General
#define JsonType "Type"
#define JsonIOType "IO-Type"
#define JsonGet "Get"
#define JsonSet "Set"

//Request
#define JsonRequest "Request"
#define JsonRequestType "RequestType"
#define JsonRequestAmount "Amount"
#define JsonRequestPin "Pin"
#define JsonRequestID "ID"

//Response
#define JsonResponse "Response"
#define JsonResponseSuccess "Success"
#define JsonResponseCritical JsonDebugCritical
#define JsonResponseError "Error"
#define JsonResponseValue "Value"

// Debug
#define JsonDebug "Debug"
#define JsonDebugMSG "MSG"
#define JsonDebugCritical "Critical"
#define JsonDebugValue "Value"