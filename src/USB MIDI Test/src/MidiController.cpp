#include "MidiController.hpp"
#include <Adafruit_TinyUSB.h>

// USB-MIDI Instanz
Adafruit_USBD_MIDI usb_midi;

MidiController::MidiController() {}

void MidiController::begin() {
    usb_midi.begin();               // USB-MIDI starten
    //Serial.begin(115200);           // Serielle Kommunikation starten
    //Serial.println("MIDI Controller Initialized");
}

void MidiController::sendNoteOn(uint8_t channel, uint8_t note, uint8_t velocity) {
    uint8_t midiMsg[3] = {
        static_cast<uint8_t>(0x90 | (channel & 0x0F)), // Note On-Status
        note,                                         // Note
        velocity                                      // Velocity
    };
    usb_midi.write(midiMsg, 3); // MIDI-Daten senden
}

void MidiController::sendNoteOff(uint8_t channel, uint8_t note, uint8_t velocity) {
    uint8_t midiMsg[3] = {
        static_cast<uint8_t>(0x80 | (channel & 0x0F)), // Note Off-Status
        note,                                         // Note
        velocity                                      // Velocity
    };
    usb_midi.write(midiMsg, 3); // MIDI-Daten senden
}
