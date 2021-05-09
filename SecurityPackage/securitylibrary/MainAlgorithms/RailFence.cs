using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            if (plainText.Length != cipherText.Length) return -1;

            cipherText = cipherText.ToLower();

            int start_key = 2; // depth of min 2 row
            float temp = (float)plainText.Length / 2; // depth max of 2 col
            int end_key = (temp % 1 > 0) ? (((int)temp) + 1) : ((int)temp);

            // brute force attack guessing the depth at min 2 row until max 2 col  ()
            for (int key = start_key; key < end_key; key++)
            {
                string temp_C_T = Encrypt(plainText, key);

                if (temp_C_T.Equals(cipherText))
                {
                    return key;
                }
            }

            return -1; // not found
            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, int key)
        {
            cipherText = cipherText.ToLower();

            int no_row = key; // depth
            float temp = (float)cipherText.Length / key;
            int no_col = (temp % 1 > 0) ? ((cipherText.Length / key) + 1) : (cipherText.Length / key); // column wise

            char[,] C_T_matrix = new char[no_row, no_col];
            int no_empty_cell = (no_row * no_col) - cipherText.Length;
            int no_letter = 0;

            // dcrypt as replacing cipher text in row wise 

            for (int row = 0; row < no_row; row++) // row wise decryption
            {
                for (int col = 0; col < no_col; col++)
                {
                    // if it is the last column && the number of remaining row is equal to the row that was empty
                    if (col + 1 == no_col && no_empty_cell == no_row - row) continue; // avoid write the cell that in the original Text was empty

                    if (no_letter < cipherText.Length)
                    {
                        C_T_matrix[row, col] = cipherText[no_letter];
                        no_letter++;
                    }
                    else
                        break;
                }
            }
            if (no_letter != cipherText.Length) throw new FormatException();
            // for decrypt read cipher text in column wise
            string PlainText = "";

            for (int col = 0; col < no_col; col++)
            {
                for (int row = 0; row < no_row; row++)
                {
                    if(C_T_matrix[row,col] != '\0')
                    PlainText += C_T_matrix[row, col];
                }
            }
            if (cipherText.Length != PlainText.Length) throw new RankException();

            return PlainText;
            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, int key)
        {
            plainText = plainText.ToLower();

            int no_row = key; // depth
            float temp = (float)plainText.Length / key;
            int no_col = (temp % 1 > 0) ? ((plainText.Length / key) + 1) : (plainText.Length / key); // column wise

            char[,] P_T_matrix = new char[no_row, no_col]; // row , col    ->  depth , column wise

            int no_letter = 0;

            for (int col = 0; col < no_col; col++) // Write P.T column wise (considering depth)
            {
                for (int row = 0; row < no_row; row++)
                {
                    if (no_letter < plainText.Length) // avoid adding null char
                    {
                        P_T_matrix[row, col] = plainText[no_letter];
                        no_letter++;
                    }
                    else
                        break;
                }
            }
            if (no_letter != plainText.Length) throw new FormatException();

            for (int i = 0; i < no_row; i++)
            {
                for (int j = 0; j < no_col; j++)
                {
                    Console.Write(P_T_matrix[i, j]+"    ");
                }
                Console.WriteLine();
            }

            // for encryption read row wise 

            string CipherText = "";

            for (int row = 0; row < no_row; row++)
            {
                for (int col = 0; col < no_col; col++)
                {
                    if (P_T_matrix[row, col] != '\0')
                    {
                        CipherText += P_T_matrix[row, col];
                    }
                }
            }

            if (CipherText.Length != plainText.Length) throw new RankException();

            return CipherText;
            //throw new NotImplementedException();
        }
    }
}
