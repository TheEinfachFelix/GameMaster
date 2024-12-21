#pragma once

#define UCNeoPixel_PIN 21 // NeoPixel LED strip
#define UCNeoPixel_LED_Count 12 // Number of LEDs
#define UCLEDType "LED"

#define UCNeoPixel_Collor_Default {0,0,7}
#define UCNeoPixel_Collor_onPress {0,10,0}
#define UCNeoPixel_Collor_onBlock {10,0,0}

//////////////////// Input ////////////////////
// Taster
#define Taster_Count 3
#define UCTaster_Pins {40,39, 38}
#define UCTaster_Name "Taster"

// Buzzer
#define Buzzer_Count 8
#define UCBuzzer_Pins_in  {47, 13, 11, 9, 17, 15, 6, 4}
#define UCBuzzer_Pins_out {48, 14, 12,10, 18, 16, 7, 5}
#define UCBuzzer_Name "Buzzer"
