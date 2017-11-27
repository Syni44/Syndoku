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
            SudokuGrid.Generate();
            SudokuGrid.ObscureCells();

            var CurrentPuzzle = new Puzzle(1, GameGrid);
            CurrentPuzzle.Read();
        }
        
        private void CellClick(object sender, MouseButtonEventArgs e) {
            TextBlock tb = sender as TextBlock;

            TextBox userEntry = new TextBox();
            userEntry.MaxLength = 1;

            string num = userEntry.Text;
            tb.Text = num;
        }
    }
}
