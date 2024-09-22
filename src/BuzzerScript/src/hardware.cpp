#include "hardware.hpp"

BuzzerMngr BuzzMngr = BuzzerMngr();
LEDController ledCntrl = LEDController();
TasterMngr tastMngr = TasterMngr();

int CollorDef[] = CCollorDef;
int CollorPress[] = CCollorPress;
int CollorBlock[] = CCollorBlock;

bool BlockBuzzer = false;
bool ProcessingRequestHold = false;


void SetupAllHardware()
{
    tastMngr.Setup();
    BuzzMngr.Setup();
    ledCntrl.Setup();
}
void LoopAllHardware()
{
  if (!ProcessingRequestHold)
  {
    if (!BlockBuzzer)
    BuzzMngr.CheckAllInputChanges();
    tastMngr.CheckAllInputChanges();
  }
  SetRGBLED();
  SetBuzzLED();
}

// TODO validate LED mode
// TODO disable handling
void SetBuzzLED()
{
    String Mode = BuzzMngr.Mode;
    for(Buzzer &i: BuzzMngr.BuzzerList)
    {
        if (Mode == JsonRequestLEDModeAuto)
        {        
            i.SetLedValue(!i.GetInputState());
        }
        else
        {
            i.SetLedValue(Mode == JsonRequestLEDModeON);
        }
    }
}

void SetRGBLED()
{
  int R, G, B;
  BlockBuzzer = !tastMngr.TasterList[BlockTasterIndex].GetInputState();
  for(Buzzer &i: BuzzMngr.BuzzerList)
  {
    if (i.GetInputState())
    {
      R = CollorPress[0];
      G = CollorPress[1];
      B = CollorPress[2];
    }
    else
    {
      R = CollorDef[0];
      G = CollorDef[1];
      B = CollorDef[2];
    }
    if (BlockBuzzer)
    {
      R = CollorBlock[0];
      G = CollorBlock[1];
      B = CollorBlock[2];
    }
    if (ledCntrl.Mode == JsonRequestLEDModeOFF)
    {
        R, B, B = 0;
    } 
    ledCntrl.SetLED(i.ID, R,G,B);
  }
  for(Taster &i: tastMngr.TasterList)
  {
    if (!i.GetInputState())
    {
      R = CollorPress[0];
      G = CollorPress[1];
      B = CollorPress[2];
    }
    else
    {
      R = CollorDef[0];
      G = CollorDef[1];
      B = CollorDef[2];
    }
    if (ledCntrl.Mode == JsonRequestLEDModeOFF)
    {
        R, B, B = 0;
    } 
    ledCntrl.SetLED(i.ID + NUM_LEDS - CTasterListLength, R,G,B);
  }
  
}
