#include <stdio.h>
#include <inttypes.h>
#include "sdkconfig.h"
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "esp_system.h"
#include "tinyusb.h"
#include "esp_log.h"
#include <device/usbd.h>
#include <tusb_cdc_acm.h>


static const char *TAG_USB = "USB";

// Interface counter
enum interface_count {
    ITF_NUM_MIDI = 0,
    ITF_NUM_MIDI_STREAMING,
    ITF_NUM_CDC,
    ITF_NUM_CDC_DATA,
    ITF_COUNT
};

// USB Endpoint numbers
enum usb_endpoints {
    // Available USB Endpoints: 5 IN/OUT EPs and 1 IN EP
    EP_EMPTY = 0,
    EPNUM_MIDI_OUT,
    EPNUM_MIDI_IN,
    EPNUM_CDC_NOTIF,
    EPNUM_CDC_OUT,
    EPNUM_CDC_IN
};

#define TUSB_DESCRIPTOR_TOTAL_LEN (TUD_CONFIG_DESC_LEN + CFG_TUD_MIDI * TUD_MIDI_DESC_LEN + CFG_TUD_CDC * TUD_CDC_DESC_LEN)

static const uint8_t s_midi_cfg_desc[] = {
    // Configuration number, interface count, string index, total length, attribute, power in mA
    TUD_CONFIG_DESCRIPTOR(1, ITF_COUNT, 0, TUSB_DESCRIPTOR_TOTAL_LEN, 0, 100),

    TUD_CDC_DESCRIPTOR(ITF_NUM_CDC, 2, EPNUM_CDC_NOTIF, 8, EPNUM_CDC_OUT, EPNUM_CDC_IN,64), // Hier min ich mir echt unsicher

    // Interface number, string index, EP Out & EP In address, EP size
    TUD_MIDI_DESCRIPTOR(ITF_NUM_MIDI, 4, EPNUM_MIDI_OUT, EPNUM_MIDI_IN, 64), // OLD in: (0x80 | EPNUM_MIDI)
};

/**
 * @brief CDC device RX callback
 *
 * CDC device signals, that new data were received
 *
 * @param[in] itf   CDC device index
 * @param[in] event CDC event type
 */
void tinyusb_cdc_rx_callback(int itf, cdcacm_event_t *event)
{
    ESP_LOGI(TAG_USB, "Recieved some Data over USB");
    /* initialization */
    //size_t rx_size = 0;

    /* read */
    //esp_err_t ret = tinyusb_cdcacm_read(itf, rx_buf, CONFIG_TINYUSB_CDC_RX_BUFSIZE, &rx_size);
    //if (ret == ESP_OK) {

    //    app_message_t tx_msg = {
    //        .buf_len = rx_size,
    //        .itf = itf,
    //    };

        /* Copy received message to application queue buffer */
    //    memcpy(tx_msg.buf, rx_buf, rx_size);
    //    xQueueSend(app_queue, &tx_msg, 0);
    //} else {
    //    ESP_LOGE(TAG, "Read Error");
    //}
}

/**
 * @brief CDC device line change callback
 *
 * CDC device signals, that the DTR, RTS states changed
 *
 * @param[in] itf   CDC device index
 * @param[in] event CDC event type
 */
void tinyusb_cdc_line_state_changed_callback(int itf, cdcacm_event_t *event)
{
    int dtr = event->line_state_changed_data.dtr;
    int rts = event->line_state_changed_data.rts;
    ESP_LOGI(TAG_USB, "Line state changed on channel %d: DTR:%d, RTS:%d", itf, dtr, rts);
}



void app_main(void)
{
    ESP_LOGI(TAG_USB, "USB initialization");

    tinyusb_config_t const tusb_cfg = {
        .external_phy = false,
        .configuration_descriptor = s_midi_cfg_desc,
    };
    ESP_ERROR_CHECK(tinyusb_driver_install(&tusb_cfg));

    ESP_LOGI(TAG_USB, "USB CDC initialization");
    const tinyusb_config_cdcacm_t acm_cfg = {
        .usb_dev = TINYUSB_USBDEV_0,
        .cdc_port = TINYUSB_CDC_ACM_0,
        .rx_unread_buf_sz = 64,
        .callback_rx = &tinyusb_cdc_rx_callback,
        .callback_rx_wanted_char = NULL,
        .callback_line_state_changed = NULL,
        .callback_line_coding_changed = NULL
    };
    tusb_cdc_acm_init(&acm_cfg);

    tusb_init();

    ESP_ERROR_CHECK(tinyusb_cdcacm_register_callback(
                    TINYUSB_CDC_ACM_0,
                    CDC_EVENT_LINE_STATE_CHANGED,
                    &tinyusb_cdc_line_state_changed_callback));

    ESP_LOGI(TAG_USB, "USB initialization DONE");

    while (true)
    {
        ESP_LOGI(TAG_USB, "Test");
        printf("Hello Word\n" );
        vTaskDelay(1000 / portTICK_PERIOD_MS);
        
    }
}
// ESP System Settings is changed 