#include "hardware.hpp"

BuzzerMngr BuzzMngr = BuzzerMngr();
LEDController ledCntrl = LEDController();
TasterMngr tastMngr = TasterMngr();

int CollorDef[] = CCollorDef;
int CollorPress[] = CCollorPress;
int CollorBlock[] = CCollorBlock;

bool ProcessingRequestHold = false;
bool lastval = true;


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
        if (!BuzzMngr.isDisabeled)
            BuzzMngr.CheckAllInputChanges();
        tastMngr.CheckAllInputChanges();
    }
    CheckDisableButton();
    SetRGBLED();
    SetBuzzLED();
}

void SetBuzzLED()
{
    if (BuzzMngr.isDisabeled) return;
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
    if (BuzzMngr.isDisabeled)
    {
      R = CollorBlock[0];
      G = CollorBlock[1];
      B = CollorBlock[2];
    }
    if (ledCntrl.Mode == JsonRequestLEDModeOFF)
    {
        R = 0;
        G = 0;
        B = 0;    
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
        R = 0;
        G = 0;
        B = 0;
    } 
    ledCntrl.SetLED(i.ID + NUM_LEDS - CTasterListLength, R,G,B);
  }
}
void CheckDisableButton()
{
    Taster tast = tastMngr.TasterList[BlockTasterIndex];
    if (!BlockTasterIsAdvanced)
    {
        BuzzMngr.isDisabeled = !tast.GetInputState();
        return;
    }
    if (!lastval && tast.GetInputState())
    {
        BuzzMngr.isDisabeled = !BuzzMngr.isDisabeled;
    }
    lastval = tast.GetInputState();
}


