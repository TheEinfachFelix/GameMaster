namespace GameMaster
{
    public class Player :IPlayer
    {
        private string pName;
        private int pPoints = 0;

        public Player(string Name) 
        { 
            pName = Name;
        }

        public string Name 
        {
            get { return pName; }
            set { pName = value; }
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
                var game = Game.GetInstance();
                var obs = game.obsConnectorList.FirstOrDefault();
                if (obs != null)
                {
                    obs.SetTextfieldValue(pName+"Points", pPoints.ToString());
                }
            }
        }
    }
}
