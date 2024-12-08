#include <Arduino.h>
#include "MidiController.hpp"

MidiController midi;

void setup() {
    midi.begin();
    
}

void loop() {
    midi.sendNoteOn(0, 60, 127); // MIDI-Channel 0, Note C4, Velocity 127
    delay(500);
    midi.sendNoteOff(0, 60, 127); // Stop Note C4
    delay(1000);
}
