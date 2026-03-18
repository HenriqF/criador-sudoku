using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

public class Sudoku
{
    public static int board_size;
    public static int blockheight;
    public static int blockwidth;
    public static string[] numeros = new string[0];
    public static string[,] board = new string[0, 0];
    public static string[,] solution = new string[0, 0];


    static void copy_references(string[,] original, string[,] copy)
    {
        for (int i = 0 ; i < original.GetLength(0); i++)
        {
            for (int j = 0; j < original.GetLength(1); j++)
            {
                copy[i,j] = original[i,j];
            }
        }
    }


    static private int solve_count_solutions(string[,] board, string[,] solution, int random_numbers)
    {
        List<HashSet<String>> rows = new List<HashSet<String>>(board_size); 
        List<HashSet<String>> columns = new List<HashSet<String>>(board_size); 
        List<HashSet<String>> blocks = new List<HashSet<String>>(board_size); 
        for (int i = 0; i < board_size ; i++)
        {
            rows.Add(new HashSet<String>());
            columns.Add(new HashSet<String>());
            blocks.Add(new HashSet<String>());
        }
        List<(int, int)> empty = new List<(int, int)>();

        for (int r = 0 ; r < board_size; r++)
        {
            for (int c = 0; c < board_size; c++)
            {
                if (board[r, c] == "_") empty.Add((r,c));
                else
                {
                    string ch = board[r, c];
                    rows[r].Add(ch);
                    columns[c].Add(ch);
                    blocks[(r/blockheight)*blockheight+(c/blockwidth)].Add(ch);
                }
            }
        }


        int solutions = 0;
        string[,] first_solution = new string[board_size, board_size];
        string[] n = new string[board_size];
        for (int i = 0 ; i < board_size; i++) n[i] = numeros[i];


        int solve_from(int index)
        {
            if (index == empty.Count)
            {
                solutions++;
                if (solutions == 1) copy_references(solution, first_solution);
                else if (solutions >= 2) return 1;
                return 0;
            }

            int r = empty[index].Item1;
            int c = empty[index].Item2;
            int b = (r/blockheight)*blockheight+(c/blockwidth);

            if (random_numbers == 1)
            {
                Random rng = new Random();
                int len = n.Length;
                for (int i = len-1; i > 0; i--)
                {
                    int j = rng.Next(i+1);
                    string temp = n[i];
                    n[i] = n[j];
                    n[j] = temp;
                }
            }
            
            foreach(string ch in n)
            {
                if ( !rows[r].Contains(ch) && !columns[c].Contains(ch) && !blocks[b].Contains(ch) )
                {
                    solution[r, c] = ch;
                    rows[r].Add(ch);
                    columns[c].Add(ch);
                    blocks[b].Add(ch);

                    if (solve_from(index+1) == 1) return 1;

                    solution[r, c] = "_";
                    rows[r].Remove(ch);
                    columns[c].Remove(ch);
                    blocks[b].Remove(ch);
                }
            }

            return 0;
        }

        int result = solve_from(0);
        copy_references(first_solution, solution);
        return result;
    }

    static private void new_valid_boards(string[,] board, string[,] solution)
    {

    }


    public Sudoku()
    {
        board_size = 6;
        blockheight = 2;
        blockwidth = 3;
        numeros = new string[board_size];

        board = new string[board_size, board_size];
        solution = new string[board_size, board_size];

        for (int i = 0; i < board_size; i++)
        {
            numeros[i] = (i + 1).ToString();
        }


        string[,] sudoku = new string[6, 6]
        {
            { "_", "3", "_", "_", "_", "5" },
            { "6", "_", "5", "1", "_", "_" },
            { "_", "1", "_", "_", "_", "6" },
            { "2", "4", "6", "_", "_", "_" },
            { "3", "5", "1", "_", "6", "_" },
            { "4", "_", "2", "_", "1", "3" }
        };


        solve_count_solutions(sudoku, sudoku, 0);
        for (int i = 0; i < sudoku.GetLength(0); i++)
        {
            for (int j = 0; j < sudoku.GetLength(1); j++)
            {
                Console.Write(sudoku[i, j] + " ");
            }
            Console.WriteLine();
        }


    }

}




class Program
{
    static void Main()
    {
        Sudoku s = new Sudoku();
        Console.WriteLine("wow!");
    }

}