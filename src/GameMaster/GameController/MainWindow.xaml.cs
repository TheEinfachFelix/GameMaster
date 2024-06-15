﻿using GameMaster;
using GameMaster.Output;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Reflection.Metadata.BlobBuilder;

namespace GameController
{
    public partial class MainWindow : Window
    {
        Game game;
        DataBinding dataBinding = new();
        readonly string ConfigLocation = "G:/Felix/GitHub/GameMaster/testconfig.json"; // G:\Felix\GitHub\GameMaster "C:/Github/GameMaster/testconfig.json"
        
        obsConnector myobs = new("ws://127.0.0.1:4455", "123456"); //, "","Gameshow"
        public MainWindow()
        {
            game = Game.LoadFromFile(ConfigLocation); // Game.GetInstance();//

            Closing += OnWindowClosing!;
            DataContext = dataBinding;
            game.LevelID *= 1; // causes the level setup to run

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
            string Btn_Name = (sender as Button).Content.ToString()!;

            if (Btn_Name == "GO")
            {
                CallBtnFunc();
                return;
            }

            dataBinding.Com_Buffer = Btn_Name;
            UpdateBinding();
        }
        private void CallBtnFunc() 
        { 
            switch (dataBinding.Com_Buffer) 
            {
                case "Next Level":
                    myobs.SetScene("2");
                    Trace.WriteLine(game.NextLevel());
                    break;
                case "Set Level":
                    myobs.SetScene("1");
                    if (Levellist.SelectedIndex == -1) return;
                    game.LevelID = Levellist.SelectedIndex;
                    break;
                case "Level GO":
                    game.CLevel.GO();
                    break;

                case "Level GO Back":
                    game.CLevel.GO(-1);
                    break;

                case "Player is Winner":
                    if (Playerlist.SelectedIndex == -1) return;
                    game.CLevel.WinnerIs(Playerlist.SelectedIndex);
                    break;

                default:
                    Trace.WriteLine($"Btn Name not found:{dataBinding.Com_Buffer}");
                    break;
            }
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
            CLevelID = game.LevelID;
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

        private int _CLevelID;
        public int CLevelID
        {
            get { return _CLevelID; }
            set { _CLevelID = value; NotifyPropertyChanged(); }
        }

        private string _Com_Buffer = "";
        public string Com_Buffer
        {
            get { return _Com_Buffer; }
            set 
            { 
                _Com_Buffer = value;
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