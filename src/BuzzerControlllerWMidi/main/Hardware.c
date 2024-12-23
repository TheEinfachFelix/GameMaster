#include "Hardware.h"


const char* Tag_Hardware = "Hardware";
const char* Tag_Neopixel = "Neopixel";

#define NP_RGB(r, g, b)   ( ((uint32_t)(r) & 0xFF) << 16  \
                       | ((uint32_t)(g) & 0xFF) << 8   \
                       | ((uint32_t)(b) & 0xFF) )


#define RMT_LED_STRIP_RESOLUTION_HZ 10000000 // 10MHz resolution, 1 tick = 0.1us (led strip needs a high resolution)

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
void LEDupdater(void *pvParameter) // hat 788 Bytes stack frei
{
    static uint8_t led_strip_pixels[NeoPixel_LED_Count*3];

    ESP_LOGI(Tag_Neopixel, "Create RMT TX channel");
    rmt_channel_handle_t led_chan = NULL;
    rmt_tx_channel_config_t tx_chan_config = {
        .clk_src = RMT_CLK_SRC_DEFAULT, // select source clock
        .gpio_num = NeoPixel_PIN,
        .mem_block_symbols = 64, // increase the block size can make the LED less flickering
        .resolution_hz = RMT_LED_STRIP_RESOLUTION_HZ,
        .trans_queue_depth = 4, // set the number of transactions that can be pending in the background
    };
    ESP_ERROR_CHECK(rmt_new_tx_channel(&tx_chan_config, &led_chan));

    ESP_LOGI(Tag_Neopixel, "Install led strip encoder");
    rmt_encoder_handle_t led_encoder = NULL;
    led_strip_encoder_config_t encoder_config = {
        .resolution = RMT_LED_STRIP_RESOLUTION_HZ,
    };
    ESP_ERROR_CHECK(rmt_new_led_strip_encoder(&encoder_config, &led_encoder));

    ESP_LOGI(Tag_Neopixel, "Enable RMT TX channel");
    ESP_ERROR_CHECK(rmt_enable(led_chan));

    ESP_LOGI(Tag_Neopixel, "Start LED rainbow chase");
    rmt_transmit_config_t tx_config = {
        .loop_count = 0, // no transfer loop
    };

    while (1)
    {    
        for (size_t i = 0; i < Buzzer_Count; i++)
        {
            int id = i * 3;
            led_strip_pixels[id + 0] = NeoPixel_Collor_Default[1];//green
            led_strip_pixels[id + 1] = NeoPixel_Collor_Default[0];//red
            led_strip_pixels[id + 2] = NeoPixel_Collor_Default[2];//blue
            if (Buzzer_in_state[i])
            {
                led_strip_pixels[id + 0] = NeoPixel_Collor_onPress[1];//green
                led_strip_pixels[id + 1] = NeoPixel_Collor_onPress[0];//red
                led_strip_pixels[id + 2] = NeoPixel_Collor_onPress[2];//blue
            }
            if (Buzzer_isBlocked)
            {
                led_strip_pixels[id + 0] = NeoPixel_Collor_onBlock[1];//green
                led_strip_pixels[id + 1] = NeoPixel_Collor_onBlock[0];//red
                led_strip_pixels[id + 2] = NeoPixel_Collor_onBlock[2];//blue
            }
        }
        for (size_t i = 0; i < Taster_Count; i++)
        {
            int id = (NeoPixel_LED_Count - Taster_Count + i) * 3;
            led_strip_pixels[id + 0] = NeoPixel_Collor_Default[1];//green
            led_strip_pixels[id + 1] = NeoPixel_Collor_Default[0];//red
            led_strip_pixels[id + 2] = NeoPixel_Collor_Default[2];//blue
            if (Taster_in_state[i])
            {
                led_strip_pixels[id + 0] = NeoPixel_Collor_onPress[1];//green
                led_strip_pixels[id + 1] = NeoPixel_Collor_onPress[0];//red
                led_strip_pixels[id + 2] = NeoPixel_Collor_onPress[2];//blue
            }
        }
        
        ESP_ERROR_CHECK(rmt_transmit(led_chan, led_encoder, led_strip_pixels, sizeof(led_strip_pixels), &tx_config));
        ESP_ERROR_CHECK(rmt_tx_wait_all_done(led_chan, portMAX_DELAY));

        vTaskDelay(5);
    }
}

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
    ESP_LOGI(Tag_Neopixel, "Start Neopixel Task");
    xTaskCreate(&LEDupdater, "NeopixelUpdater", 3072,NULL,10,NULL );

    ESP_LOGI(Tag_Hardware, "Setup DONE");
}

void LoopHardware()
{
    for (int i = 0; i < Taster_Count; i++)
    {
        if (Taster_in_state[i] != !gpio_get_level(Taster_Pins[i]))// der gpio input ist invertiert, da es hochgepulllt wird
        {
            EventSender(Taster_Name,i,Taster_in_state[i], gpio_get_level(Taster_Pins[i]));
            Taster_in_state[i] = !gpio_get_level(Taster_Pins[i]);
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
    for (int i = 0; i < Buzzer_Count; i++) 
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
        Buzzer_out_state[i] = !gpio_get_level(Buzzer_Pins_in[i]) && !Buzzer_isBlocked;
        gpio_set_level(Buzzer_Pins_out[i],Buzzer_out_state[i]);
    }    
}







