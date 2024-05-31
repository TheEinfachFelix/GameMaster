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
#define SerialTimeout 20
#define SerialSpeed 115200

// Json Keys
#define JsonError "Error"
#define JsonID "ID"
#define JsonValue "Value"
#define JsonType "Output_Type"
// {"Output_Type" : "Buzzer", "ID" : 2, "Value" : true}
// {"Output_Type" : "LED", "ID" : 2, "Value" : {"R":50, "G":10, "B":0}}
