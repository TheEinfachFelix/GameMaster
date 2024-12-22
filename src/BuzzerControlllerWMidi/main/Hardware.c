#include "Hardware.h"

const char* Tag_Hardware = "Hardware";

void EventSender(char* type, int ID, bool oldVal, bool newVal);

void SetupHardware()
{
    ESP_LOGI(Tag_Hardware, "Setup");

    // Setup Taster
    for (int i = 0; i < Taster_Count; i++)
    {
        int INpin = Taster_Pins[i];
        gpio_set_direction(INpin, GPIO_MODE_INPUT);
        gpio_set_pull_mode(INpin, GPIO_PULLUP_ONLY);
    }
    

    // Setup Buzzer
    for (int i = 0; i < Buzzer_Count; i++)
    {
        int INpin = Buzzer_Pins_in[i];
        int OUTpin = Buzzer_Pins_out[i];
        gpio_set_direction(INpin, GPIO_MODE_INPUT);
        gpio_set_pull_mode(INpin, GPIO_PULLDOWN_ONLY);

        gpio_set_direction(OUTpin, GPIO_MODE_OUTPUT);
    }

    // Setup Neopixel

    ESP_LOGI(Tag_Hardware, "Setup DONE");
}

void LoopHardware()
{
    for (int i = 0; i < Taster_Count; i++) // pulllupp
    {
        if (Taster_in_state[i] != gpio_get_level(Taster_Pins[i]))
        {
            EventSender(Taster_Name,i,Taster_in_state[i], gpio_get_level(Taster_Pins[i]));
            Taster_in_state[i] = gpio_get_level(Taster_Pins[i]);
            if (Taster_in_state[i] == 1)
            {
                SendMidiNoteOn(Taster_Midi_Notes[i]);
            }
            else
            {
                SendMidiNoteOff(Taster_Midi_Notes[i]);
            }
        }
    }
    for (int i = 0; i < Buzzer_Count; i++) // pulllupp
    {
        if (Buzzer_in_state[i] != gpio_get_level(Buzzer_Pins_in[i]))
        {
            EventSender(Buzzer_Name,i,Buzzer_in_state[i], gpio_get_level(Buzzer_Pins_in[i]));
            Buzzer_in_state[i] = gpio_get_level(Buzzer_Pins_in[i]);
            if (Buzzer_in_state[i] == 1)
            {
                SendMidiNoteOn(Buzzer_Midi_Notes[i]);
            }
            else
            {
                SendMidiNoteOff(Buzzer_Midi_Notes[i]);
            }
        }
        gpio_set_level(Buzzer_Pins_out[i],Buzzer_out_state[i]);
    }

}

void EventSender(char* type, int ID, bool oldVal, bool newVal)
{
    // create json
    cJSON *root = cJSON_CreateObject();
    cJSON_AddStringToObject(root, JsonType, JsonEvent);
    cJSON_AddStringToObject(root, JsonIOType, type);
    cJSON_AddNumberToObject(root, JsonEventID, ID);
    cJSON_AddBoolToObject(root, JsonEventOldValue, oldVal);
    cJSON_AddBoolToObject(root, JsonEventNewValue, newVal);
    // output
    char *my_json_string = cJSON_Print(root);
    PrintlnToCDC(my_json_string);
    // cleanup
    cJSON_Delete(root);
    free(my_json_string);
}

