using GameMaster;
using GameMaster.Input;
using GameMaster.Output;
using GameMaster.Level;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace GameController
{
    
    public partial class MainWindow : Window
    {
        Game game;
        DataBinding dataBinding = new();
        readonly string ConfigLocation = "C:/Github/GameMaster/kinder.json";//"G:/Felix/GitHub/GameMaster/testconfig.json"; // G:\Felix\GitHub\GameMaster "C:/Github/GameMaster/testconfig.json"
        System.Windows.Forms.Timer BindingUpdateTimer;

        public MainWindow()
        {
            
            game = Game.LoadFromFile(ConfigLocation);

            Closing += OnWindowClosing!;
            DataContext = dataBinding;

            // auto binding updates
            BindingUpdateTimer = new();
            BindingUpdateTimer.Tick += new EventHandler(TimedUpdateBinding);
            BindingUpdateTimer.Interval = 100; // Binding Update Speed
            BindingUpdateTimer.Start();

            InitializeComponent();

            game.Setup();
            UpdateBinding();

            // setup other page
            GameDisplay gameDisplay = new GameDisplay();
            gameDisplay.Show();
            gameDisplay.DataContext = dataBinding;

        }

        ////////////// Handle Window Closing magic
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            System.Windows.Application.Current.Shutdown();
            Trace.WriteLine("OnClosed");
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Game.SaveToFile(ConfigLocation);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //AudioPlayer.PlaySound("C:/Users/felix/Downloads/Test-1.wav");
            //AudioPlayer.PlaySound("C:/Users/felix/Downloads/Zu_gut_fuer_dich.wav");
            
            var btn = (System.Windows.Controls.Button)sender;
            string Btn_Name = btn.Content.ToString()!;

            if (Btn_Name == "GO")
            {
                CallBtnFunc();
                return;
            }


            dataBinding.Com_Buffer = Btn_Name;
            //UpdateBinding();

        }
        private void CallBtnFunc() 
        { 
            switch (dataBinding.Com_Buffer) 
            {
                case "Next Level":
                    Trace.WriteLine(game.NextLevel());
                    break;
                case "Set Level":
                    if (Levellist.SelectedIndex == -1) return;
                    game.LevelID = Levellist.SelectedIndex;
                    break;
                case "Level GO":
                    game.CLevel?.GO();
                    break;

                case "Level GO Back":
                    game.CLevel?.GO(-1);
                    break;

                case "Player is Winner":
                    if (Playerlist.SelectedIndex == -1) return;
                    game.CLevel?.WinnerIs(Playerlist.SelectedIndex);
                    break;

                default:
                    Trace.WriteLine($"Btn Name not found:{dataBinding.Com_Buffer}");
                    break;
            }
        }

        private void TimedUpdateBinding(object? source, EventArgs e )
        {
            UpdateBinding();
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
        private void ListView_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BindingUpdateTimer.Stop();
        }
        private void Levellist_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BindingUpdateTimer.Start();
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
            DContent = game.CDisplayContent;
            DFontsize = game.CDisplayFontSize;



            TotalPoints = 0;            
            if (game.Players.Count > 1)
            {
                PointsA = game.Players[0].Points;
                PointsB = game.Players[1].Points;
                TotalPoints = -PointsA-2;
            }
        }

        private List<ILevel>? _LevelList;
        public List<ILevel> LevelList
        {
            get 
            {
                _LevelList ??= [];
                return _LevelList; 
            }
            set
            {
                _LevelList = value;
                NotifyPropertyChanged();
            }
        }

        private List<IPlayer>? _PlayerList;
        public List<IPlayer> PlayerList
        {
            get 
            {
                _PlayerList ??= [];
                return _PlayerList; 
            }
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

        private int _TotalPoints;
        public int TotalPoints
        {
            get { return _TotalPoints; }
            set
            {
                _TotalPoints = value;
                NotifyPropertyChanged();
            }
        }
        private int _PointsA;
        public int PointsA
        {
            get { return _PointsA; }
            set
            {
                _PointsA = value;
                NotifyPropertyChanged();
            }
        }
        private int _PointsB;
        public int PointsB
        {
            get { return _PointsB; }
            set
            {
                _PointsB = value;
                NotifyPropertyChanged();
            }
        }
        public int PointsB1
        {
            get { return _PointsB - 1; } set { NotifyPropertyChanged(); }
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

        private string _DContent = "";
        public string DContent
        {
            get { return _DContent; }
            set
            {
                _DContent = value;
                NotifyPropertyChanged();
            }
        }

        private int _DFontsize;
        public int DFontsize
        {
            get { return _DFontsize; }
            set { _DFontsize = value; NotifyPropertyChanged(); }
        }

        private int _Disabeled;
        public int Disabeled
        {
            get { return _Disabeled; }
            set { _Disabeled = value; NotifyPropertyChanged(); }
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