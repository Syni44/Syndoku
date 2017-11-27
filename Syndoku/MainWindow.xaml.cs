using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Syndoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {       
        public MainWindow() {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame() {
            var SudokuGrid = new SudokuGrid(GameGrid);
            SudokuGrid.Organize();
            // should we be generating a new puzzle every time, even if just loading a puzzle? likely not right?
            SudokuGrid.Generate();
            SudokuGrid.ObscureCells();

            var CurrentPuzzle = new Puzzle(1, GameGrid);
            // replace CurrentPuzzle.Write(); with CurrentPuzzle.Reaed(); to test puzzle loading functionality
            // refer to puzzles.txt for saved puzzles, and Puzzle.cs for underlying code
            CurrentPuzzle.Write();
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
