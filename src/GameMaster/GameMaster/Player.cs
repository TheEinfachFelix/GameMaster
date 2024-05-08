﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public class Player :IPlayer
    {
        private string pName;
        private int pPoints;
        public Player(string Name) 
        { 
            pName = Name;
        }

        public string Name 
        {
            get { return pName; }
        }

        public int Points 
        {
            get
            {
                return pPoints;
            } 
            set
            {
                pPoints = value;
            }
        }
    }
}
