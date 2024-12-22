#pragma once
#include "Config.h"

#include "esp_log.h"
#include "driver\gpio.h"
#include "cJSON.h"
#include <stdbool.h>



void SetupHardware();
void LoopHardware();