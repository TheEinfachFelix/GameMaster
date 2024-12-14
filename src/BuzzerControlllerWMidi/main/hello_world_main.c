#include <stdio.h>
#include <inttypes.h>
#include "sdkconfig.h"
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "esp_system.h"
#include "tinyusb.h"
#include "esp_log.h"
#include <device/usbd.h>


static const char *TAG_USB = "USB";

// Interface counter
enum interface_count {
    ITF_NUM_MIDI = 0,
    ITF_NUM_MIDI_STREAMING,
    ITF_NUM_CDC,
    ITF_COUNT
};

// USB Endpoint numbers
enum usb_endpoints {
    // Available USB Endpoints: 5 IN/OUT EPs and 1 IN EP
    EP_EMPTY = 0,
    EPNUM_MIDI,
    EPNUM_CDC,
};

#define TUSB_DESCRIPTOR_TOTAL_LEN (TUD_CONFIG_DESC_LEN + CFG_TUD_MIDI * TUD_MIDI_DESC_LEN)

static const uint8_t s_midi_cfg_desc[] = {
    // Configuration number, interface count, string index, total length, attribute, power in mA
    TUD_CONFIG_DESCRIPTOR(1, ITF_COUNT, 0, TUSB_DESCRIPTOR_TOTAL_LEN, 0, 100),

    TUD_CDC_DESCRIPTOR(ITF_NUM_CDC, 2, 0x81, 0x02, EPNUM_CDC, (0x80 | EPNUM_CDC),64), // Hier min ich mir echt unsicher

    // Interface number, string index, EP Out & EP In address, EP size
    TUD_MIDI_DESCRIPTOR(ITF_NUM_MIDI, 4, EPNUM_MIDI, (0x80 | EPNUM_MIDI), 64),
};


void app_main(void)
{
    ESP_LOGI(TAG_USB, "USB initialization");
    tinyusb_config_t const tusb_cfg = {
        .external_phy = false,
        .configuration_descriptor = s_midi_cfg_desc,
    };
    ESP_ERROR_CHECK(tinyusb_driver_install(&tusb_cfg));

    ESP_LOGI(TAG_USB, "USB initialization DONE");

    while (true)
    {
        ESP_LOGI(TAG_USB, "Test");
        printf("Hello Word\n" );
        vTaskDelay(1000 / portTICK_PERIOD_MS);
        
    }
}
