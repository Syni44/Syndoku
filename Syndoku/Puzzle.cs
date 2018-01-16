using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Syndoku
{
    enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Challenging
    }

    public class Puzzle
    {
        public int ID { get; private set; }
        private Grid ThisGrid { get; set; }
        private Difficulty Difficulty { get; set; }

        public Puzzle(int iID, Grid iGrid) {
            ID = iID;
            ThisGrid = iGrid;
        }

        /// <summary>
        /// Saves the current Sudoku grid by converting cell values to a readable format in a .txt document.
        /// </summary>
        public void Write() {       // TODO: tidy or refactor Write()
            string filePath = @"..\..\..\Syndoku\puzzles.txt";
            string textToAdd = "";
            List<TextBlock> Obscured = new List<TextBlock>();
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);

            foreach (var cell in ThisGrid.Children.OfType<TextBlock>()) {

                if (cell.Text == "") {
                    textToAdd = "X";
                    Obscured.Add(cell);
                }                    
                else
                    textToAdd = cell.Text;

                if (!File.Exists(filePath)) {
                    fs = null;
                    try {
                        fs = new FileStream(filePath, FileMode.CreateNew);
                        using (StreamWriter writer = new StreamWriter(fs)) {
                            writer.Write(textToAdd);
                        }
                    }
                    finally {
                        if (fs != null)
                            fs.Dispose();
                    }
                }
                else {                               
                    fs.Close();
                    try {
                        fs = new FileStream(filePath, FileMode.Append);

                        using (StreamWriter writer = new StreamWriter(fs)) {
                            if (ThisGrid.Children.OfType<TextBlock>().First() == cell) {
                                writer.Write(Environment.NewLine + Environment.NewLine);
                            }
                            writer.Write(textToAdd);
                        }
                    }
                    finally {
                        if (fs != null)
                            fs.Dispose();
                    }
                }
            }

            if (Obscured.Count < 32)
                Difficulty = Difficulty.Easy;
            else if (Obscured.Count < 40)
                Difficulty = Difficulty.Medium;
            else if (Obscured.Count < 52)
                Difficulty = Difficulty.Hard;
            else
                Difficulty = Difficulty.Challenging;

            MessageBox.Show(Difficulty.ToString());
        }

        /// <summary>
        /// Deciphers saved puzzles in a .txt document and reflects the values onto the game grid.
        /// </summary>
        public void Read() {        // TODO: tidy or refactor Read()
            string filePath = @"..\..\..\Syndoku\puzzles.txt";
            FileStream fs = null;
            string line = "";
            try {
                int puzzCounter = 0;
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(fs)) {
                    reader.Read();

                    while (!reader.EndOfStream) {
                        try {
                            line = File.ReadLines(filePath).Skip(2 * puzzCounter).Take(1).First();
                        }
                        catch {
                            if (line == "")
                                throw new Exception("Unable to read puzzle file. Check puzzles.txt for format consistency!");
                        }

                        int counter = 0;

                        foreach (char chr in line) {
                            var cell = ThisGrid.Children.OfType<TextBlock>().ElementAt(counter);

                            switch (chr) {
                                case '1': cell.Text = "1"; break;
                                case '2': cell.Text = "2"; break;
                                case '3': cell.Text = "3"; break;
                                case '4': cell.Text = "4"; break;
                                case '5': cell.Text = "5"; break;
                                case '6': cell.Text = "6"; break;
                                case '7': cell.Text = "7"; break;
                                case '8': cell.Text = "8"; break;
                                case '9': cell.Text = "9"; break;
                                case 'X': cell.Text = ""; break;
                                default: throw new Exception("Unexpected character. Check your puzzles.txt puzzle format!");
                            }

                            counter++;
                        }

                        puzzCounter++;

                        try {
                            File.ReadLines(filePath).Skip(2 * puzzCounter).Take(1).First();
                        }
                        catch {
                            reader.ReadToEnd();
                        }
                    }

                    if (puzzCounter > 1)
                        MessageBox.Show("More than one puzzle in file found. Loading bottom-most puzzle. (Please shuffle desired puzzle to bottom of puzzles.txt after closing program!)");
                }
            }
            finally {
                if (fs != null)
                    fs.Dispose();
            }
        }
    }
}
