using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        private static int key_constraint = 5;
        private static int Shiffting_Amount_alphabetic = 97;
        private static int index_I = 8, index_J = 9;
        private static char Replacement_letter = 'x';
        public string Decrypt(string cipherText, string key)
        {
            if (key.Length > 26) return null;

            cipherText = cipherText.ToLower();
            key = key.ToLower();

            string PlainText = "";
            int no_char_been_read = 0; // number of alpha taked from the key string
            int row_I_J_inMAtrix = -1, col_I_J_inMAtrix = -1;

            char[,] Key_Matrix = formate_key(key, ref row_I_J_inMAtrix, ref col_I_J_inMAtrix, ref no_char_been_read);

            if (no_char_been_read != 25) return null;

            for (int c = 0; c < cipherText.Length; c += 2) // block of two latters
            {
                char left_letter, right_letter; // the block

                left_letter = cipherText[c];
                right_letter = cipherText[c + 1];

                int leftChar_row = -1, leftChar_col = -1;
                int rightchar_row = -1, rightChar_col = -1;

                FindChar_matrix(Key_Matrix, left_letter, ref leftChar_row, ref leftChar_col);
                FindChar_matrix(Key_Matrix, right_letter, ref rightchar_row, ref rightChar_col);

                if (leftChar_row == rightchar_row)
                {
                    PlainText += Key_Matrix[leftChar_row, ((leftChar_col == 0) ? key_constraint - 1 : leftChar_col - 1)]; // left char
                    PlainText += Key_Matrix[rightchar_row, ((rightChar_col == 0) ? key_constraint - 1 : rightChar_col - 1)]; // right char
                }
                else if (leftChar_col == rightChar_col)
                {
                    PlainText += Key_Matrix[((leftChar_row == 0) ? key_constraint - 1 : leftChar_row - 1), leftChar_col];// left char
                    PlainText += Key_Matrix[((rightchar_row == 0) ? key_constraint - 1 : rightchar_row - 1), rightChar_col];// right char
                }
                else
                {
                    PlainText += Key_Matrix[leftChar_row, rightChar_col]; // row of the letter the col of the other letter
                    PlainText += Key_Matrix[rightchar_row, leftChar_col]; // row Ft  col Sc
                }
            }

            string PLainText_noExtra_replacment = "";

            /* char previous_char = '\0';
            for (int c = 0; c < PlainText.Length; c++) // remove the extra replacement latter
            {
                if (PlainText[c] == Replacement_letter)
                {
                    if (c + 1 < PlainText.Length) // prevent invalid index
                    {
                        if (previous_char == PlainText[c + 1]) // latter before and after the replacement latter are equale 
                        {
                            continue; // skip add the 'X' latter cause it was an addition in the Ecryption method
                        }
                    }
                }
                previous_char = PlainText[c];
                PLainText_noExtra_replacment += PlainText[c];
            }*/

            for (int c = 0; c < PlainText.Length; c+=2) // remove the extra replacement latter
            {
                char left, right;
                left = PlainText[c];
                right = PlainText[c + 1];

                if (right == Replacement_letter)
                {
                    if (c + 2 < PlainText.Length)// prevent invalid index 
                    {
                        if (PlainText[c + 2] == left) // get the next duplicate of the left char
                        {
                            PLainText_noExtra_replacment += left;

                            continue;
                            // skip the right char it was replacment 
                            //c--;
                        }
                    }
                }
                PLainText_noExtra_replacment += left;
                PLainText_noExtra_replacment += right;
            }

            if (PlainText.Length % 2 == 0 && PLainText_noExtra_replacment[PLainText_noExtra_replacment.Length - 1] == Replacement_letter) // remove the last x
                PLainText_noExtra_replacment = PLainText_noExtra_replacment.Substring(0, PLainText_noExtra_replacment.Length - 1);

            return PLainText_noExtra_replacment;
            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            if (key.Length > 26) return null;

            plainText = plainText.ToLower();
            key = key.ToLower();

            string CipherText = "";
            string PLainText_modified = "";
            int no_char_been_read = 0; // number of alpha taked from the key string
            int row_I_J_inMAtrix = -1, col_I_J_inMAtrix = -1;

            char[,] Key_Matrix = formate_key(key, ref row_I_J_inMAtrix, ref col_I_J_inMAtrix, ref no_char_been_read);

            if (no_char_been_read != 25) return null;

            for (int c = 0; c < plainText.Length; c += 2) //check there is no two iddeticale latter
            {
                if (c >= plainText.Length - 1) { PLainText_modified += plainText[c]; break; } // if PT is odd length avoid invalid index

                char left = plainText[c], right = plainText[c + 1];
                if (left == right) // if the block is taken of the same latter
                {
                    PLainText_modified += left;
                    PLainText_modified += Replacement_letter;
                    //PLainText_modified += right; // will become left for the following block

                    c--;
                }
                else
                {
                    PLainText_modified += left;
                    PLainText_modified += right;
                }
            }
            if (PLainText_modified.Length % 2 == 1) PLainText_modified += Replacement_letter; // make the plaintext multiple of pair of two letters

            for (int c = 0; c < PLainText_modified.Length; c += 2) // Block of two latters taken
            {
                char left_letter, right_letter; // the block

                left_letter = PLainText_modified[c];
                right_letter = PLainText_modified[c + 1];

                int leftChar_row = -1, leftChar_col = -1;
                int rightchar_row = -1, rightChar_col = -1;

                FindChar_matrix(Key_Matrix, left_letter, ref leftChar_row, ref leftChar_col);
                FindChar_matrix(Key_Matrix, right_letter, ref rightchar_row, ref rightChar_col);

                if (leftChar_row == rightchar_row)
                {
                    CipherText += Key_Matrix[leftChar_row, ((leftChar_col == key_constraint - 1) ? 0 : leftChar_col + 1)]; // left char
                    CipherText += Key_Matrix[rightchar_row, ((rightChar_col == key_constraint - 1) ? 0 : rightChar_col + 1)]; // right char
                }
                else if (leftChar_col == rightChar_col)
                {
                    CipherText += Key_Matrix[((leftChar_row == key_constraint - 1) ? 0 : leftChar_row + 1), leftChar_col];// left char
                    CipherText += Key_Matrix[((rightchar_row == key_constraint - 1) ? 0 : rightchar_row + 1), rightChar_col];// right char
                }
                else
                {
                    CipherText += Key_Matrix[leftChar_row, rightChar_col]; // row of the letter the col of the other letter
                    CipherText += Key_Matrix[rightchar_row, leftChar_col]; // row Ft  col Sc
                }

            }
            return CipherText;
            //throw new NotImplementedException();
        }

        private char[,] formate_key(string key, ref int row_I_J_inMAtrix, ref int col_I_J_inMAtrix, ref int no_char_been_read)
        {
            char[,] Key_Matrix = new char[key_constraint, key_constraint];
            int[] arr_alpha = new int[26]; for (int i = 0; i < arr_alpha.Length; i++) arr_alpha[i] = 0;


            for (int k = 0; k < key.Length; k++)
            {
                int alpha_index = ((int)(key[k])) - Shiffting_Amount_alphabetic;

                if (arr_alpha[alpha_index] == 0)
                {
                    arr_alpha[alpha_index] = 1;

                    int row, col;

                    row = no_char_been_read / key_constraint;
                    col = no_char_been_read - (row * key_constraint);

                    Key_Matrix[row, col] = key[k];

                    if (alpha_index == index_I || alpha_index == index_J)
                    {
                        arr_alpha[index_J] = 1; arr_alpha[index_I] = 1;
                        row_I_J_inMAtrix = row; col_I_J_inMAtrix = col;
                    }

                    no_char_been_read++;
                }
            }
            for (int alpha = 0; alpha < arr_alpha.Length; alpha++)
            {
                if (arr_alpha[alpha] == 0)
                {
                    arr_alpha[alpha] = 1;

                    int row, col;

                    row = no_char_been_read / key_constraint;
                    col = no_char_been_read - (row * key_constraint);

                    Key_Matrix[row, col] = (char)(alpha + Shiffting_Amount_alphabetic);

                    if (alpha == index_I || alpha == index_J)
                    {
                        arr_alpha[index_J] = 1; arr_alpha[index_I] = 1;
                        row_I_J_inMAtrix = row; col_I_J_inMAtrix = col;
                    }

                    no_char_been_read++;
                }
            }
            //if (no_char_been_read != 25) return null;


            for (int i = 0; i < key_constraint; i++)
            {
                for (int j = 0; j < key_constraint; j++)
                {
                    Console.Write(Key_Matrix[i, j] + "     ");
                }
                Console.WriteLine("\n");
            }
            for (int i = 0; i < 26; i++)
            {
                Console.Write(arr_alpha[i] + "    ");
            }
            Console.WriteLine("\n row      " + row_I_J_inMAtrix + "       col     " + col_I_J_inMAtrix);

            return Key_Matrix;
        }

        private void FindChar_matrix(char[,] matrix, char letter, ref int row, ref int col)
        {
            for (int i = 0; i < key_constraint; i++)
            {
                for (int j = 0; j < key_constraint; j++)
                {
                    if (matrix[i, j] == letter)
                    {
                        row = i;
                        col = j;
                        break;
                    }
                }

            }
        }
    }
}
