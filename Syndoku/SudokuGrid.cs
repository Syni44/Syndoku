using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Syndoku
{
    /// <summary>
    /// Manages the Sudoku playing area and all cells within.
    /// </summary>
    public class SudokuGrid
    {
        public Random rng = new Random();

        /// <summary>
        /// The xaml Grid control converted for use in the Grid class.
        /// </summary>
        public Grid grid { get; private set; }

        public List<TextBlock> Box0 = new List<TextBlock>();
        public List<TextBlock> Box1 = new List<TextBlock>();
        public List<TextBlock> Box2 = new List<TextBlock>();
        public List<TextBlock> Box3 = new List<TextBlock>();
        public List<TextBlock> Box4 = new List<TextBlock>();
        public List<TextBlock> Box5 = new List<TextBlock>();
        public List<TextBlock> Box6 = new List<TextBlock>();
        public List<TextBlock> Box7 = new List<TextBlock>();
        public List<TextBlock> Box8 = new List<TextBlock>();

        public List<TextBlock> Row0 = new List<TextBlock>();
        public List<TextBlock> Row1 = new List<TextBlock>();
        public List<TextBlock> Row2 = new List<TextBlock>();
        public List<TextBlock> Row3 = new List<TextBlock>();
        public List<TextBlock> Row4 = new List<TextBlock>();
        public List<TextBlock> Row5 = new List<TextBlock>();
        public List<TextBlock> Row6 = new List<TextBlock>();
        public List<TextBlock> Row7 = new List<TextBlock>();
        public List<TextBlock> Row8 = new List<TextBlock>();

        public List<TextBlock> Column0 = new List<TextBlock>();
        public List<TextBlock> Column1 = new List<TextBlock>();
        public List<TextBlock> Column2 = new List<TextBlock>();
        public List<TextBlock> Column3 = new List<TextBlock>();
        public List<TextBlock> Column4 = new List<TextBlock>();
        public List<TextBlock> Column5 = new List<TextBlock>();
        public List<TextBlock> Column6 = new List<TextBlock>();
        public List<TextBlock> Column7 = new List<TextBlock>();
        public List<TextBlock> Column8 = new List<TextBlock>();

        /// <summary>
        /// Constructor for the game grid object.
        /// </summary>
        /// <param name="inGrid"></param>
        public SudokuGrid(Grid inGrid) {
            grid = inGrid;
        }

        /// <summary>
        /// Returns a TextBlock cell object at a specified row and column coordinate.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public TextBlock Position(int row, int column) {
            AdjustToIndex(ref row, ref column);

            return grid.Children
                       .Cast<TextBlock>()
                       .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
            // important to understand. look at it like "returns the first TEXTBLOCK (type inferred) where (=>) after evaluates to true, or
            // the Grid.GetRow method on said textblock returns the same number as row", etc
        }

        /// <summary>
        /// Initializes all cells into their appropriate row, column and box lists based on coordinate.
        /// </summary>
        public void Organize() {
            for (int i = 0; i <= 8; i++) {
                for (int j = 0; j <= 8; j++) {
                    GetBox(i, j).Add(Position(i, j));
                    GetRow(i).Add(Position(i, j));
                    GetColumn(j).Add(Position(i, j));

                    Position(i, j).Text = "";
                }
            }
        }

        /// <summary>
        /// Assigns values to the Text property of each cell in a way that satisfies Sudoku's conditions.
        /// </summary>
        public void Generate() {
            for (int i = 2; i <= 8; i += 3) {
                for (int j = 2; j <= 8; j += 3) {
                    bool nulled;        // TODO: don't use a data flag for loop conditions like this!
                    do {
                        nulled = false;
                        foreach (TextBlock cell in GetBox(i, j)) {
                            int r = Grid.GetRow(cell);
                            int c = Grid.GetColumn(cell);
                            AdjustFromIndex(ref r, ref c);
        
                            List<int> RowInts = GetRow(r).Where(x => x.Text != "").Select(x => x.Text).Select(int.Parse).ToList();
                            List<int> ColumnInts = GetColumn(c).Where(x => x.Text != "").Select(x => x.Text).Select(int.Parse).ToList();
                            List<int> BoxInts = GetBox(i, j).Where(x => x.Text != "").Select(x => x.Text).Select(int.Parse).ToList();
        
                            var IntEnum = Enumerable.Range(1, 9).Except(RowInts).Except(ColumnInts).Except(BoxInts);
                            int count = IntEnum.Count();
        
                            if (count == 0) {
                                ClearGrid();
                                i = 2;
                                j = 2;
        
                                nulled = true;
                                break;
                            }
                            else
                                cell.Text = IntEnum.ElementAt(rng.Next(count)).ToString();
                        }
                    } while (nulled);
                }
            }            
        }

        /// <summary>
        /// Randomly selects cells from a completed grid and sets their values to empty in a mirrored pattern.
        /// </summary>
        public void ObscureCells() {
            List<TextBlock> Obscured = new List<TextBlock>();
            for (int i = 2; i <= 8; i += 3) {
                for (int j = 2; j <= 8; j += 3) {
                    foreach (TextBlock cell in GetBox(i, j)) {
                        int roll = rng.Next(1, 11);

                        switch (roll) {
                            case 1:
                            case 2:
                            case 3: cell.Text = ""; Obscured.Add(cell); break;
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            default: break;
                        }
                    }

                    List<TextBlock> ToObscure = new List<TextBlock>();

                    foreach (TextBlock cell in Obscured) {
                        int r = Grid.GetRow(cell);
                        int c = Grid.GetColumn(cell);
                        AdjustFromIndex(ref r, ref c);

                        ToObscure.Add(Mirror(Position(r, c)));
                        Mirror(Position(r, c)).Text = "";
                    }

                    foreach (TextBlock cell in ToObscure) {
                        if (!Obscured.Contains(cell))
                            Obscured.Add(cell);
                    }
                }
            }
        }

        #region Helper Methods

        /// <summary>
        /// References cells in the sudoku grid that are mirrored from the 8,8 point as origin based on the cell's coordinates from 0,0.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private TextBlock Mirror(TextBlock cell) {
            int r = Grid.GetRow(cell);
            int c = Grid.GetColumn(cell);
            AdjustFromIndex(ref r, ref c);

            switch (r) {
                case 0: r = 8; break;
                case 1: r = 7; break;
                case 2: r = 6; break;
                case 3: r = 5; break;
                case 4: break;
                case 5: r = 3; break;
                case 6: r = 2; break;
                case 7: r = 1; break;
                case 8: r = 0; break;
                default: throw new Exception("Error mirroring row values");
            }

            switch (c) {
                case 0: c = 8; break;
                case 1: c = 7; break;
                case 2: c = 6; break;
                case 3: c = 5; break;
                case 4: break;
                case 5: c = 3; break;
                case 6: c = 2; break;
                case 7: c = 1; break;
                case 8: c = 0; break;
                default: throw new Exception("Error mirroring column values");
            }

            return Position(r, c);
        }

        /// <summary>
        /// Resets the grid to empty values.
        /// </summary>
        private void ClearGrid() {
            for (int i = 0; i <= 8; i++) {
                foreach (TextBlock cell in GetRow(i)) {
                    cell.Text = "";
                }
            }
        }

        /// <summary>
        /// Returns the associated Box from the cell at Row/Column coordinates.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private List<TextBlock> GetBox(int row, int column) {
            // is this the best way to organize these statements?
            switch (row) {
                case 0:
                case 1:
                case 2:
                    if (column < 3)
                        return Box0;
                    else if (column < 6)
                        return Box1;
                    else
                        return Box2;
                case 3:
                case 4:
                case 5:
                    if (column < 3)
                        return Box3;
                    else if (column < 6)
                        return Box4;
                    else
                        return Box5;
                case 6:
                case 7:
                case 8:
                    if (column < 3)
                        return Box6;
                    else if (column < 6)
                        return Box7;
                    else
                        return Box8;
                default: throw new Exception("Error adding cell to box list");
            }
        }

        /// <summary>
        /// Returns the associated Row list from row coordinate.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private List<TextBlock> GetRow(int row) {
            switch (row) {
                case 0: return Row0;
                case 1: return Row1;
                case 2: return Row2;
                case 3: return Row3;
                case 4: return Row4;
                case 5: return Row5;
                case 6: return Row6;
                case 7: return Row7;
                case 8: return Row8;
                default: throw new Exception("Error adding cell to row list");
            }
        }

        /// <summary>
        /// Returns the associated Column list from column coordinate.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private List<TextBlock> GetColumn(int column) {
            switch (column) {
                case 0: return Column0;
                case 1: return Column1;
                case 2: return Column2;
                case 3: return Column3;
                case 4: return Column4;
                case 5: return Column5;
                case 6: return Column6;
                case 7: return Column7;
                case 8: return Column8;
                default: throw new Exception("Error adding cell to column list");
            }
        }

        /// <summary>
        /// Adjust row and column indexing to match xaml coordinates.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        private void AdjustToIndex(ref int row, ref int column) {
            if (row >= 6)
                row += 2;
            else if (row >= 3)
                row++;

            if (column >= 6)
                column += 2;
            else if (column >= 3)
                column++;
        }

        /// <summary>
        /// Adjust row and column indexing to match simple coordinates from xaml.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        private void AdjustFromIndex(ref int row, ref int column) {
            if (row >= 7)
                row -= 2;
            else if (row >= 3)
                row--;

            if (column >= 7)
                column -= 2;
            else if (column >= 3)
                column--;
        }

        #endregion
    }
}
