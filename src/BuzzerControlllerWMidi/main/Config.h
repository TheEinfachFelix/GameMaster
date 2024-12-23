#pragma once

#include <stdbool.h>
#include <tusb_cdc_acm.h>
#include "tinyusb.h"
#include "tusb_console.h"
#include "esp_log.h"


#include "UserConfig.h"
//////////////////// NeoPixel ////////////////////

#define len(arr) sizeof(arr)/sizeof(arr[0])

extern char* LEDType;

extern uint8_t NeoPixel_Collor_Default[];
extern uint8_t NeoPixel_Collor_onPress[];
extern uint8_t NeoPixel_Collor_onBlock[];
//#define BlockTasterIndex 2 // welcher der Taster sperren kann
//#define BlockTasterIsAdvanced true

//////////////////// Input ////////////////////
// Taster
extern int Taster_Midi_Notes[];
extern bool Taster_in_state[];
extern int Taster_Pins[];
extern char* Taster_Name;

// Buzzer
extern int Buzzer_Midi_Notes[];
extern bool Buzzer_in_state[];
extern bool Buzzer_out_state[];
extern int Buzzer_Pins_in[];
extern int Buzzer_Pins_out[];
extern char* Buzzer_Name;

void PrintlnToCDC(char* msg);
void SendMidiNoteOn(int Note);
void SendMidiNoteOff(int Note);
//////////////////// Json ////////////////////
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
