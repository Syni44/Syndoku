using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Syndoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        public SudokuGrid CurrentGrid;
        public Puzzle CurrentPuzzle;

        public MainWindow() {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame() {
            CurrentGrid = new SudokuGrid(GameGrid);

            LoadLastPuzzle();       // toggle between LoadLastPuzzle(); and CreateNewPuzzle(); on this line
        }

        /// <summary>
        /// Initializes a new sudoku grid puzzle and calls the Write method to save it to puzzles.txt.
        /// </summary>
        private void CreateNewPuzzle() {
            CurrentGrid.Organize();
            CurrentGrid.Generate();
            CurrentGrid.ObscureCells();

            CurrentPuzzle = new Puzzle(1, GameGrid);
            CurrentPuzzle.Write();
        }

        /// <summary>
        /// Loads the bottom-listed puzzle from puzzles.txt into the sudoku grid on game init.
        /// </summary>
        private void LoadLastPuzzle() {
            CurrentPuzzle = new Puzzle(1, GameGrid);
            CurrentPuzzle.Read();
        }
        
        // currently unimplemented
        private void CellClick(object sender, MouseButtonEventArgs e) {
            TextBlock tb = sender as TextBlock;

            TextBox userEntry = new TextBox();
            userEntry.MaxLength = 1;

            string num = userEntry.Text;
            tb.Text = num;
        }
    }
}
