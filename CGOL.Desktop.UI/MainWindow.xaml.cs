using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;
using CGOL.Core;
using System.Reactive.Subjects;
using System.Collections.Generic;
using Avalonia.Input;
using System.IO;
using System.Threading;
using System.Text;
using Avalonia.Rendering;
using Avalonia.Threading;

namespace CGOL.Desktop.UI
{
    public class MainWindow : Window
    {
        private Avalonia.Controls.Grid _grid;
        private int _cols = 20; ///TODO: Get input from UI
        private int _rows = 20; ///TODO: Get input from UI
        private Universe _universe;
        //private Dictionary<string,Subject<string>> _currentStateObservable;
        private bool _EnableLogGenerations = true; //If true, the app will log every single generation to a text file under user's MyDocuments. Watch out for file growth for long (or infinite) iterations.
        private string _GenerationsOutputFileNameFormat = "CGOL.Generations.{0}.txt";
        private long _GenerationsOutputFileNameTimeStamp;
        private bool _RunTimer = false; // Determines if the game timer should be running or stopped.
        private bool _TimerCreated = false; // Enabled when first timer is created. Used to avoid creating duplicate timers.
        private double _TimerIntervalInSeconds = 0.2;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LogGeneration(string generationOutput)
        {
            if (!_EnableLogGenerations)
                return;
            
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            string filePath = Path.Combine(folderPath, string.Format(_GenerationsOutputFileNameFormat,_GenerationsOutputFileNameTimeStamp));

            LogFile(filePath, generationOutput);
        }

        private void LogFile(string filePath, string message)
        {
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                //Append
                // Create a file to write to.
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(message);
                }
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            //Bind to keydown event
            this.KeyDown += MainWindow_KeyDown;

            // Store grid reference in global var
            _grid = this.FindControl<Avalonia.Controls.Grid>("cgolGrid");

            // Add columns and rows. Add Button controls to each cell.
            InitializeGrid(_cols,_rows);

            NewGame();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e) {
            string message = $"MainWindow_KeyDown - key: {e.Key}";
            //LogFile(e.Key.ToString());

            if (e.Key.ToString() == "N")
            {
                NewGame();
            }
            if (e.Key.ToString() == "S")
            {
                StartGame();
            }
            if (e.Key.ToString() == "T")
            {
                StopGame();
            }
        }

        private void StartGame()
        {
            _GenerationsOutputFileNameTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); //Used for output filename

            Cell[,] currentStateGrid = ReadCurrentStateFromGrid();

            _universe = new Universe(currentStateGrid);
            
            _RunTimer = true;

            if (!_TimerCreated) // This will avoid duplicate timers. If one was already created, reuse it.
            {
                DispatcherTimer.Run(() =>
                {
                    _universe.Tick();
                    RenderCurrentState();
                    
                    return _RunTimer;

                }, TimeSpan.FromSeconds(_TimerIntervalInSeconds));
            }
        }

        private void StopGame()
        {
            _RunTimer = false;
        }

        private void NewGame()
        {
            ResetGrid();
        }

        private void ResetGrid()
        {
            for (int row=0; row<_rows; row++)
                    for (int col=0; col<_cols; col++)
                    {
                        //Get button control
                        string keyName = $"btn_{col}_{row}";
                        Button btn = _grid.Children.Cast<Button>().Where(b => b.Name == keyName).FirstOrDefault();
                        
                        btn.Classes.Remove("live");
                        btn.Classes.Remove("dead");
                    }
        }

        private Cell[,] ReadCurrentStateFromGrid()
        {
            Cell[,] currentState = new Cell[_cols,_rows];

            for (int row=0; row<_rows; row++)
                for (int col=0; col<_cols; col++)
                {
                    //Get button control
                    string keyName = $"btn_{col}_{row}";
                    Button btn = _grid.Children.Cast<Button>().Where(b => b.Name == keyName).FirstOrDefault();
                    
                    bool flag = btn.Classes.Contains("live") ? true : false;
            
                    currentState[col,row] = new Cell(flag);
                }

            return currentState;
        }

        private void RenderCurrentState()
        {
            StringBuilder sb = new StringBuilder();
            
            // Get and render current state of the universe
            var currentGeneration = _universe.Generations.LastOrDefault();
            var currentState = currentGeneration.StateOfUniverse;
            sb.AppendLine($"Generation #: {currentGeneration.TickNumber}");
            for (int row=0; row<_rows; row++)
            {
                string rowStr = string.Empty;
                for (int col=0; col<_cols; col++)
                {
                    string keyName = $"btn_{col}_{row}";
                    //_currentStateObservable[keyName].OnNext(className); //Trigger onchanged event to perform binding
                    
                    Button btn = _grid.Children.Cast<Button>().Where(b => b.Name == keyName).FirstOrDefault();
                    
                    if (currentState[col,row].IsLive)
                    {
                        btn.Classes.Remove("live");
                        btn.Classes.Remove("dead");

                        btn.Classes.Add("live");
                        rowStr += "  X  ";
                    }
                    else
                    {
                        btn.Classes.Remove("live");
                        btn.Classes.Remove("dead");

                        btn.Classes.Add("dead");
                        rowStr += "  O  ";                        
                    }
                }
                sb.AppendLine(rowStr);
            }

            LogGeneration(sb.ToString());
        }

        private void InitializeGrid(int cols, int rows)
        {
            if (_grid == null)
                throw new ArgumentNullException("No grid has been found");

            var grid = _grid;
            grid.ShowGridLines = true;

            // Define the Columns.
            for (int i = 0; i < cols; i++)
            {
                var colDef = new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)};
                grid.ColumnDefinitions.Add(colDef);
            }

            // Define the Rows.
            for (int j = 0; j < rows; j++)
            {
                var rowDef = new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)};
                grid.RowDefinitions.Add(rowDef);
            }

            //Populate grid with buttons
            for (int row=0; row<rows; row++)
                for (int col=0; col<cols; col++)
                {
                    Button b = new Button();
                    b.Name = $"btn_{col}_{row}";
                    b.Classes.Add("dead");
                    b.Click += new EventHandler<RoutedEventArgs>(MyButton_Click);
                    
                    Avalonia.Controls.Grid.SetColumn(b,col);
                    Avalonia.Controls.Grid.SetRow(b,row);

                    _grid.Children.Add(b);
                }
        }

        private void Toggle(Button b)
        {
            if (b.Classes.Contains("live"))
            {
                b.Classes.Remove("live");
                b.Classes.Add("dead");
            }
            else
            {
                b.Classes.Remove("dead");
                b.Classes.Add("live");
            }
        }

        public void MyButton_Click(object sender, RoutedEventArgs e)
        {
            Toggle(sender as Button);
        }
    }
}