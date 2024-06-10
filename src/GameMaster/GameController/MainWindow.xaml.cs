using GameMaster;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebGameController.Models;

namespace GameController
{
    public partial class MainWindow : Window
    {
        Game game;
        DataBinding dataBinding = new();
        readonly string ConfigLocation = "G:/Felix/GitHub/GameMaster/testconfig.json";
        public MainWindow()
        {
            game = Game.LoadFromFile(ConfigLocation);

            Closing += OnWindowClosing;
            DataContext = dataBinding;
            InitializeComponent();

            UpdateBinding();
        }

        ////////////// Handle Window Closing magic
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
            Trace.WriteLine("OnClosed");
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Game.SaveToFile(ConfigLocation);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Player p1 = new("Peter");
            game.Players.Add(p1);

            dataBinding.ViewType = 1;
            dataBinding.ViewType = 2;
            Trace.WriteLine(game.Players.Count);
            //Game.SaveToFile(ConfigLocation);
        }
        private void UpdateBinding()
        {
            dataBinding.UpdateData();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }



    class DataBinding : INotifyPropertyChanged
    {
        public DataBinding()
        {
            UpdateData();
        }

        public void UpdateData()
        {
            Game game = Game.GetInstance();
            PlayerList = game.Players;
            LevelList = game.Levels;    
            ViewType = 1;
            ViewType = 2;
        }

        private List<ILevel> _LevelList;
        public List<ILevel> LevelList
        {
            get { return _LevelList; }
            set
            {
                _LevelList = value;
                NotifyPropertyChanged();
            }
        }

        private List<IPlayer> _PlayerList;
        public List<IPlayer> PlayerList
        {
            get { return _PlayerList; }
            set
            {
                _PlayerList = value;
                NotifyPropertyChanged();
            }
        }

        private int _ViewType;
        public int ViewType
        {
            get { return _ViewType; }
            set
            {
                _ViewType = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

/*
{
  "$type": "GameMaster.Game, GameMaster",
  "Players": [
    {
      "$type": "GameMaster.Player, GameMaster",
      "Name": "Peter",
      "Points": 0
    },
    {
      "$type": "GameMaster.Player, GameMaster",
      "Name": "Peter",
      "Points": 0
    }
  ],
  "Levels": [
    {
      "$type": "GameMaster.TestLevel, GameMaster",
      "Name": "Sound",
      "Beschreibung": "Spielt Sound"
    },
    {
      "$type": "WebGameController.Models.FirstLevel, GameMaster",
      "Name": "Bzzer to .2",
      "Beschreibung": "Setzt auf BuzzerPress dot2 Values"
    }
  ],
  "LevelID": 0,
  "CLevel": null
}
 */