#pragma once

#include <Arduino.h>

class MidiController {
public:
    MidiController();
    void begin();
    void sendNoteOn(uint8_t channel, uint8_t note, uint8_t velocity);
    void sendNoteOff(uint8_t channel, uint8_t note, uint8_t velocity);

private:
    void sendMidiMessage(uint8_t command, uint8_t data1, uint8_t data2);
};
