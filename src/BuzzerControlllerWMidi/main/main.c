#include <stdio.h>
#include <inttypes.h>
#include "sdkconfig.h"
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "esp_system.h"
#include "tinyusb.h"
#include "tusb_console.h"
#include "esp_log.h"
#include <device/usbd.h>
#include <tusb_cdc_acm.h>
#include "esp_mac.h"


#include "Hardware.h"



static uint8_t rx_buf[CONFIG_TINYUSB_CDC_RX_BUFSIZE + 1];
static const char *TAG_USB = "USB";



static QueueHandle_t app_queue;
typedef struct {
    uint8_t buf[CONFIG_TINYUSB_CDC_RX_BUFSIZE + 1];     // Data buffer
    size_t buf_len;                                     // Number of bytes received
    uint8_t itf;                                        // Index of CDC device interface
} app_message_t;

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
    EPNUM_MIDI,
    EPNUM_CDC_NOTIF,
    EPNUM_CDC
};

#define TUSB_DESCRIPTOR_TOTAL_LEN (TUD_CONFIG_DESC_LEN + CFG_TUD_MIDI * TUD_MIDI_DESC_LEN + CFG_TUD_CDC * TUD_CDC_DESC_LEN)

static const uint8_t s_midi_cfg_desc[] = {
    // Configuration number, interface count, string index, total length, attribute, power in mA
    TUD_CONFIG_DESCRIPTOR(1, ITF_COUNT, 0, TUSB_DESCRIPTOR_TOTAL_LEN, 0, 100),

    
    TUD_CDC_DESCRIPTOR(ITF_NUM_CDC, 4, EPNUM_CDC_NOTIF, 8, EPNUM_CDC, (0x80 | EPNUM_CDC),64), // Hier min ich mir echt unsicher

    // Interface number, string index, EP Out & EP In address, EP size
    TUD_MIDI_DESCRIPTOR(ITF_NUM_MIDI, 4, EPNUM_MIDI, (0x80 | EPNUM_MIDI), 64), // OLD in: (0x80 | EPNUM_MIDI)
};

static const char* s_str_desc[6] = {
    // array of pointer to string descriptors
    (char[]){0x09, 0x04},  // 0: is supported language is English (0x0409)
    "EinfachTechnik",             // 1: Manufacturer
    "Buzzer Controller",      // 2: Product
    "123456",              // 3: Serials, should use chip ID
    "Buzzer Controller Midi", // 4: MIDI
    "sstrdesc 6 item",
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
    size_t rx_size = 0;

    /* read */
    esp_err_t ret = tinyusb_cdcacm_read(itf, rx_buf, CONFIG_TINYUSB_CDC_RX_BUFSIZE, &rx_size);
    if (ret == ESP_OK) {

        app_message_t tx_msg = {
            .buf_len = rx_size,
            .itf = itf,
        };

        /* Copy received message to application queue buffer */
        memcpy(tx_msg.buf, rx_buf, rx_size);
        xQueueSend(app_queue, &tx_msg, 0);
    } else {
        ESP_LOGE(TAG_USB, "Read Error");
    }
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
// Create FreeRTOS primitives
    app_queue = xQueueCreate(5, sizeof(app_message_t));
    assert(app_queue);
    app_message_t msg;

    ESP_LOGI(TAG_USB, "USB initialization");

    tinyusb_config_t const tusb_cfg = {
        .device_descriptor = NULL, // If device_descriptor is NULL, tinyusb_driver_install() will use Kconfig
        .string_descriptor = s_str_desc,
        .string_descriptor_count = sizeof(s_str_desc) / sizeof(s_str_desc[0]),
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
    ESP_ERROR_CHECK(tusb_cdc_acm_init(&acm_cfg));

    tusb_init();

    ESP_ERROR_CHECK(tinyusb_cdcacm_register_callback(
                    TINYUSB_CDC_ACM_0,
                    CDC_EVENT_LINE_STATE_CHANGED,
                    &tinyusb_cdc_line_state_changed_callback));

    ESP_LOGI(TAG_USB, "USB initialization DONE");

    SetupHardware();

    while (true)
    {
        LoopHardware();


        /*
        //ESP_LOGI(TAG_USB, "Test");


        char snum[5];
        // Convert 123 to string [buf]
        itoa(tud_midi_mounted(), snum, 10);
        // Print our string
        //printf("%s\n", snum);

        if (tud_cdc_write_available())
        {
            tud_cdc_write_str("hallo");
            tud_cdc_write_char('a');
            tud_cdc_write_flush();
            ESP_LOGI(TAG_USB, "CDC Write");

            if (xQueueReceive(app_queue, &msg, portMAX_DELAY)) {
            if (msg.buf_len) {

                // Print received data
                ESP_LOGI(TAG_USB, "Data from channel %d:", msg.itf);
                ESP_LOG_BUFFER_HEXDUMP(TAG_USB, msg.buf, msg.buf_len, ESP_LOG_INFO);

                // write back 
                tinyusb_cdcacm_write_queue(msg.itf, msg.buf, msg.buf_len);
                esp_err_t err = tinyusb_cdcacm_write_flush(msg.itf, 0);
                if (err != ESP_OK) {
                    ESP_LOGE(TAG_USB, "CDC ACM write flush error: %s", esp_err_to_name(err));
                }
            }
        }
        }

        if (tud_midi_mounted()) {
            static uint8_t const cable_num = 0;
            static uint8_t const channel = 0;

            uint8_t note_off[3] = {NOTE_OFF | channel, 98, 0};
            uint8_t note_on[3] = {NOTE_ON | channel, 98, 127};
            ESP_LOGI(TAG_USB, "MidiON");
            tud_midi_stream_write(cable_num, note_on, 3);
            vTaskDelay(1000 / portTICK_PERIOD_MS);
            tud_midi_stream_write(cable_num, note_off, 3);

        }
        vTaskDelay(1000 / portTICK_PERIOD_MS);
        */
       vTaskDelay(1);
    }
}
