#include <Arduino.h>


int amountOpPins;
int pins[] = {36,39,34};

bool stats[sizeof(pins) / sizeof(int)] = {false};

void setup() {
  amountOpPins = sizeof(pins) / sizeof(int);
  std::fill_n(stats, amountOpPins, true);
  for (int i = 0; i < amountOpPins; i++)
  {
    pinMode(pins[i], INPUT);
    stats[i] = digitalRead(pins[i]);
  }
  

  Serial.begin(9600);
}

void loop() {
  for(int i = 0; i < amountOpPins; i++)
  {
    bool now = digitalRead(pins[i]);
    bool old = stats[i];
    if (old != now)
    {
        stats[i] = now;
        Serial.print(i);

        if(!now)
        {
          Serial.print(" press");
        } else 
        {
          Serial.print(" release");
        }
        Serial.println(" --");
        delay(1);
    }

  }
}

