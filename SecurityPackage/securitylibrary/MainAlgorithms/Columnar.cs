using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        private static char Replacement_letter = 'x';
        static int max = 10; // max no of col can perform
        public List<int> Analyse(string plainText, string cipherText)
        {
            if (plainText.Length != cipherText.Length) return null;

            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();

            int min_col = 2;
            int max_col = (plainText.Length / min_col);
            max_col = (max_col > 10) ? (max) : (max_col);

            Console.WriteLine("col  " + max_col);

            //List<int> _key = new List<int>();
            for (int col = min_col; col < max_col ; col++)
            {
                int[] _key = new int[col];
                for (int k = 0; k < col; k++)
                {
                    //_key.Add(k);
                    _key[k] = k+1;
                }
                List<List<int>> possible_key;
                possible_key = List_Permutations(_key);

                //PrintResult(possible_key);

                if (possible_key != null)
                {
                    foreach (List<int> key in possible_key)
                    {
                        string C_T = Encrypt(plainText, key);
                        if (cipherText.Equals(C_T)) return key;
                    }
                }

                //_key.Clear();
            }
            throw new KeyNotFoundException();
            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            cipherText = cipherText.ToLower();

            int no_col = key.Count; // for each permutation
            float temp = (float)cipherText.Length / no_col;
            int no_row = (temp % 1 > 0) ? ((int)temp + 1) : ((int)temp); // row wise

            char[,] C_T_matrix = new char[no_row, no_col];
            int no_letter = 0;
            int no_replacement = 0;
            int no_empty_cell = (no_row * no_col) - cipherText.Length; // the no of cell that should be empty at the last row


            // write each col to its correponding index based on permutation
            for (int col = 0; col < no_col; col++)// col in 1 based
            {
                for (int row = 0; row < no_row; row++)
                {
                    // avoid writing in the cell that was empty
                    if (row + 1 == no_row && no_empty_cell > no_col - key.IndexOf(col + 1)) continue;

                    if (no_letter < cipherText.Length)
                    {
                        // convert col to  0 based
                        C_T_matrix[row, key.IndexOf(col + 1)] = cipherText[no_letter];
                        no_letter++;
                    }
                    else
                        break;
                }
            }
            if (no_letter != cipherText.Length) throw new FormatException();

            for (int i = 0; i < no_row; i++)
            {
                for (int j = 0; j < no_col; j++)
                {
                    Console.Write(C_T_matrix[i, j] + "    ");
                }
                Console.WriteLine();
            }
            string PlainText = "";
            for (int row = 0; row < no_row; row++) // read matrix row wise to get the original text
            {
                for (int col = 0; col < no_col; col++)
                {
                    if (C_T_matrix[row, col] != '\0')
                        PlainText += C_T_matrix[row, col];
                }
            }
            if (PlainText.Length != cipherText.Length) throw new FormatException();

            return PlainText;
            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, List<int> key)
        {
            plainText = plainText.ToLower();

            int no_col = key.Count; // for each permutation
            float temp = (float)plainText.Length / no_col;
            int no_row = (temp % 1 > 0) ? ((int)temp + 1) : ((int)temp); // row wise

            // create plain text matrix write row wise
            char[,] P_T_matrix = new char[no_row, no_col];
            int no_letter = 0;
            int no_replacement = 0;

            for (int row = 0; row < no_row; row++) // write row by row
            {
                for (int col = 0; col < no_col; col++)
                {
                    if (no_letter < plainText.Length)
                    {
                        P_T_matrix[row, col] = plainText[no_letter];
                        no_letter++;
                    }
                    else
                    {
                        /*  P_T_matrix[row, col] = Replacement_letter; // replace the empty cell
                          no_replacement++;*/
                    }
                }
            }
            if (no_letter != plainText.Length) throw new FormatException();

            for (int i = 0; i < no_row; i++)
            {
                for (int j = 0; j < no_col; j++)
                {
                    Console.Write(P_T_matrix[i, j] + "   ");
                }
                Console.WriteLine();
            }
            // Encrypt each col with certain permutation
            string CipherText = "";

            for (int col = 0; col < no_col; col++) // the permutation is in one(1) based
            {
                for (int row = 0; row < no_row; row++)
                {
                    if (P_T_matrix[row, key.IndexOf(col + 1)] != '\0')
                    {
                        CipherText += P_T_matrix[row, key.IndexOf(col + 1)]; // read column wise
                    }
                }
            }

            /* for (int col = 0; col < no_col; col++) 
             {
                 for (int row = 0; row < no_row; row++)
                 {
                     if (P_T_matrix[row, col] != '\0')
                     {
                                 CipherText += P_T_matrix[row, col];
                     }
                 }
             }*/
            if (plainText.Length + no_replacement != CipherText.Length) throw new RankException();

            return CipherText;
            //throw new NotImplementedException();
        }


        static List<List<int>> List_Permutations(int[] nums)
        {
            List<List<int>> list = new List<List<int>>();
            return GetPermutation(nums, 0, nums.Length - 1, list);
        }
        static List<List<int>> GetPermutation(int[] nums, int start, int end, List<List<int>> list)
        {
            if (start == end)
            {
                // We have one of our possible n! solutions,
                // add it to the list.
                list.Add(new List<int>(nums));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    Swap(ref nums[start], ref nums[i]);
                    GetPermutation(nums, start + 1, end, list);
                    Swap(ref nums[start], ref nums[i]); // reset for next pass
                }
            }
            return list;
        }
        static void Swap(ref int a, ref int b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        static void PrintResult(List<List<int>> lists)
        {
            Console.WriteLine("{");
            foreach (var list in lists)
            {
                Console.WriteLine($"    [{string.Join(",", list)}]");
            }
            Console.WriteLine("}");
        }
    }
}
