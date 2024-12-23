# include "Config.h"

#define NOTE_OFF 0x80
#define NOTE_ON  0x90
//////////////////// NeoPixel ////////////////////
char* LEDType = UCLEDType;
// mode

uint8_t NeoPixel_Collor_Default[] = UCNeoPixel_Collor_Default;
uint8_t NeoPixel_Collor_onPress[] = UCNeoPixel_Collor_onPress;
uint8_t NeoPixel_Collor_onBlock[] = UCNeoPixel_Collor_onBlock;

//////////////////// Input ////////////////////
// Taster
int Taster_Midi_Notes[] = UCTaster_Midi_Notes;
bool Taster_in_state[Taster_Count] = {};
int Taster_Pins[] = UCTaster_Pins;
char* Taster_Name = UCTaster_Name;
// etwas um ffestzulegen ob es ein taster ist


// Buzzer
int Buzzer_Midi_Notes[] = UCBuzzer_Midi_Notes;
bool Buzzer_out_state[Buzzer_Count] = {};
bool Buzzer_in_state[Buzzer_Count] = {};
int Buzzer_Pins_in[]  = UCBuzzer_Pins_in;
int Buzzer_Pins_out[] = UCBuzzer_Pins_out;
char* Buzzer_Name = UCBuzzer_Name;
bool Buzzer_isBlocked;
// ledmode


void PrintlnToCDC(char* msg)
{
    tud_cdc_write_str(msg);
    tud_cdc_write_char('\n');
    tud_cdc_write_flush();
}

void SendMidiNoteOn(int Note)
{
    if (tud_midi_mounted()) {
        static uint8_t const cable_num = 0;
        static uint8_t const channel = 0;

        uint8_t note_on[3] = {NOTE_ON | channel, 98, 127};
        tud_midi_stream_write(cable_num, note_on, 3);
    } else {
        ESP_LOGE("MIDI", "Midi is not Mounted");
    }
}
void SendMidiNoteOff(int Note)
{
    if (tud_midi_mounted()) {
        static uint8_t const cable_num = 0;
        static uint8_t const channel = 0;

        uint8_t note_off[3] = {NOTE_OFF | channel, 98, 0};
        tud_midi_stream_write(cable_num, note_off, 3);
    } else {
        ESP_LOGE("MIDI", "Midi is not Mounted");
    }
}


/*
//////////////////// Serial ////////////////////
#define SerialTimeout 5
#define SerialSpeed 115200
*/
