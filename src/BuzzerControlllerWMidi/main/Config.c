# include "Config.h"

//////////////////// NeoPixel ////////////////////
int NeoPixel_PIN = UCNeoPixel_PIN; // NeoPixel LED strip
int NeoPixel_LED_Count = UCNeoPixel_LED_Count; // Number of LEDs
char* LEDType = UCLEDType;
// mode

int NeoPixel_Collor_Default[] = UCNeoPixel_Collor_Default;
int NeoPixel_Collor_onPress[] = UCNeoPixel_Collor_onPress;
int NeoPixel_Collor_onBlock[] = UCNeoPixel_Collor_onBlock;

//////////////////// Input ////////////////////
// Taster
bool Taster_in_state[Buzzer_Count] = {};
int Taster_Pins[] = UCTaster_Pins;
char* Taster_Name = UCTaster_Name;
// midi note


// Buzzer
bool Buzzer_out_state[Buzzer_Count] = {true,true,true};
bool Buzzer_in_state[Buzzer_Count] = {};
int Buzzer_Pins_in[]  = UCBuzzer_Pins_in;
int Buzzer_Pins_out[] = UCBuzzer_Pins_out;
char* Buzzer_Name = UCBuzzer_Name;
// midinote
// ledmode
// disabeled

void PrintlnToCDC(char* msg)
{
    tud_cdc_write_str(msg);
    tud_cdc_write_char('\n');
    tud_cdc_write_flush();
}


/*
//////////////////// Serial ////////////////////
#define SerialTimeout 5
#define SerialSpeed 115200
*/
