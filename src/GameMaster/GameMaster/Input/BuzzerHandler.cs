using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster.Input
{
    public class BuzzerHandler
    {
        public List<Buzzer> BuzzerList = [];
        public int amountOfBuzzer { get; private set; }

        public List<Taster> TasterList = [];
        public int amountOfTaster { get; private set; }


        public BuzzerHandler(int pamountOfBuzzer, int pamountOfTaster) 
        {
            amountOfBuzzer =  pamountOfBuzzer;
            amountOfTaster = pamountOfTaster;
            Setup();
        }

        private void Setup()
        {
            for (int i = 0; i < amountOfBuzzer; i++)
            {
                BuzzerList.Add(new(i));
            }

            for (int i = 0; i < amountOfTaster; i++)
            {
                TasterList.Add(new(i));
            }
        }
    }

    public class Buzzer
    {
        private Game game = Game.GetInstance();

        public int myID { get; set; } = -1;

        private bool _TasterState = false;
        public bool TasterState 
        { 
            get 
            { 
                return _TasterState; 
            } set 
            {
                bool newVal = value;

                if (newVal != _TasterState) 
                {
                    if (newVal)
                    {
                        game.BuzzerPress(myID);
                    }
                    else
                    {
                        game.BuzzerRelease(myID);
                    }
                }
                _TasterState = newVal;
            } 
        }

        private bool _LEDState = false;
        public bool LEDState
        {
            get
            {
                return _LEDState;
            }
            set
            {
                _LEDState = value;
                // TODO: send state
            }
        }

        public Buzzer(int pID)
        {
            myID = pID;
        }
    }
    public class Taster
    {
        private Game game = Game.GetInstance();

        public int myID { get; set; } = -1;

        private bool _TasterState = false;
        public bool TasterState
        {
            get
            {
                return _TasterState;
            }
            set
            {
                bool newVal = value;

                if (newVal != _TasterState)
                {
                    if (newVal)
                    {
                        game.BuzzerPress(myID);
                    }
                    else
                    {
                        game.BuzzerRelease(myID);
                    }
                }
                _TasterState = newVal;
            }
        }


        public Taster(int pID)
        {
            myID = pID;
        }
    }

}
