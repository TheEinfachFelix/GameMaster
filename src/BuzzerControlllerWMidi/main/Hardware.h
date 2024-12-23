#pragma once
#include "Config.h"

#include "esp_log.h"
#include "driver\gpio.h"
#include "cJSON.h"
#include <stdbool.h>
#include "driver/rmt_tx.h"
#include "led_strip_encoder.h"
#include <esp_err.h>
#include <esp_log.h>

void SetupHardware();
void LoopHardware();