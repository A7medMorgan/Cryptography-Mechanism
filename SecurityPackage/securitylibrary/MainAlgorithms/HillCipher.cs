using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            if (plainText.Count != cipherText.Count) return null;

            int m = 2;
            int _base = 26;

            List<int> Key =null;
            List<int> sub_P_T = new List<int>();
            List<int> sub_C_T = new List<int>();

            //int[] P_T_arr = new int[m * m];
            int[] C_T_arr = new int[m * m];
            //if (plainText.Count < m * 2 || cipherText.Count < m * 2) throw new TaskCanceledException();


            for (int i = 0; i < plainText.Count; i+=m) // get fisrt 2 letter
            {
                sub_P_T.Add(plainText[i]);
                sub_P_T.Add(plainText[i + 1]);

                //sub_C_T.Add(cipherText[i]);
                //sub_C_T.Add(cipherText[i + 1]);

                //P_T_arr[0] = plainText[i];
                //P_T_arr[2] = plainText[i + 1];

                C_T_arr[0] = cipherText[i];
                C_T_arr[2] = cipherText[i + 1];


                for (int j = i+m; j < plainText.Count; j+=m) // loop on every posible compinetion bettwen the first 2 letter and the rest of the plain text
                {
                    sub_P_T.Add(plainText[j]);
                    sub_P_T.Add(plainText[j + 1]);

                    //sub_C_T.Add(cipherText[j]);
                    //sub_C_T.Add(cipherText[j + 1]);

                    //P_T_arr[1] = plainText[j];
                    //P_T_arr[3] = plainText[j + 1];

                    C_T_arr[1] = cipherText[j];
                    C_T_arr[3] = cipherText[j + 1];

                    for (int c = 0; c < m*m; c++) // because that Matrix Multiply take row wise // sub cipher text needs to be re arranged
                    {
                       // sub_P_T.Add(P_T_arr[c]);
                        sub_C_T.Add(C_T_arr[c]);
                    }
                    try
                    {
                        Key = Decrypt(sub_C_T, sub_P_T);
                    }
                    catch(Exception e)
                    {
                        Key = null;
                        Console.WriteLine(e.Message);
                    }
                    if (Key != null)
                    {
                        List<int> C_T = Encrypt(plainText, Key); // to check the correct of the key
                        bool Key_Solved = true;
                        for (int c = 0; c < cipherText.Count; c++)
                        {
                            if (cipherText[c] != C_T[c]) { Key_Solved = false; break; }
                        }
                        if (Key_Solved) return Key;
                    }

                    sub_P_T.RemoveAt(3); // remove the last chossen char 
                    sub_P_T.RemoveAt(2);

                    //sub_C_T.RemoveAt(3);
                    //sub_C_T.RemoveAt(2);
                    
                    //sub_P_T.Clear();
                    sub_C_T.Clear();

                }

                sub_P_T.Clear();
                sub_C_T.Clear();
            }


            #region brute force attack
            /* bool found_flag = false; 
             for (int no_letter = 0; no_letter < m; no_letter++) // brute force attack on the first two letter
             {
                 for (int i = 0; i < _base; i++)
                 {
                     for (int j = 0; j < _base; j++)
                     {
                         int encrypted_element = (plainText[2] * i) + (plainText[3] * j); // brute force encryption
                         if (cipherText[no_letter + m] == (encrypted_element % _base))
                         {
                             found_flag = true;
                             Key.Add(i);
                             Key.Add(j);
                             break;
                         }
                     }
                     if (found_flag) { found_flag = false; break; }
                 }
                 if (Key.Count != (no_letter * m) + m) throw new KeyNotFoundException();
             }
             foreach (int item in Key)
             {
                 Console.Write("    " + item);
             }*/
            #endregion

            throw new InvalidAnlysisException();
            //throw new NotImplementedException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            int m = (int)Math.Sqrt((double)key.Count); // determine K as m x m matrix

            List<int> inverse = K_inverse(key, m ,26 , true);

            if (inverse == null) throw new InvalidOperationException();

            List<int> PlainText = new List<int>();
            int count_char = 0;
            int no_letter = 0;

            List<int> sub_text = new List<int>();
            foreach (int letter in cipherText)
            {
                sub_text.Add(letter);
                count_char++;

                no_letter++;

                if (no_letter == cipherText.Count)
                {
                    for (int i = 0; i < m - count_char; i++) // allocate the end of the text massage with -1 to not calculate it
                    {
                        sub_text.Add(-1);
                    }
                    count_char = m;
                }

                if (count_char == m)
                {
                    List<int> result_mul = matrix_mul(sub_text, inverse, m, 26); //decrypt

                    foreach (int item in result_mul)
                    {
                        PlainText.Add(item);
                    }

                    count_char = 0;
                    sub_text.Clear();
                }
            }
            if (no_letter != cipherText.Count) throw new ArgumentNullException();

            return PlainText;
            //throw new NotImplementedException();
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int m = (int)Math.Sqrt((double)key.Count); // determine K as m x m matrix

            int count_char = 0;
            int no_letter = 0;

            List<int> CipherText = new List<int>();
            List<int> sub_text = new List<int>();
            foreach (int letter in plainText)
            {
                sub_text.Add(letter);
                count_char++;

                no_letter++;

                if (no_letter == plainText.Count)
                {
                    for (int i = 0; i < m-count_char; i++)
                    {
                        sub_text.Add(0);
                    }
                    count_char = m;
                }

                if (count_char == m)
                {
                    List<int> result_mul = matrix_mul(sub_text, key, m, 26); // encrypt

                    foreach (int item in result_mul)
                    {
                        CipherText.Add(item);
                    }

                    count_char = 0;
                    sub_text.Clear();
                }
            }
            if (no_letter != plainText.Count) return null;

            return CipherText;
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            if (plainText.Count != cipherText.Count) throw new InvalidAnlysisException();

            int m = 3;
            int _base = 26;

            List<int> Key = new List<int>();
            List<int> sub_P_T = new List<int>();
            List<int> sub_C_T = new List<int>();

            //int[] P_T_arr = new int[m * m];
            int[] C_T_arr = new int[m * m]; //  {0 , 1 , 2 } 
                                            //  {3 , 4 , 5 }
                                            //  {6 , 7 , 7 }

            for (int i = 0; i < plainText.Count; i+=m) // first col
            {
                sub_P_T.Add(plainText[i]);
                sub_P_T.Add(plainText[i + 1]);
                sub_P_T.Add(plainText[i + 2]);

                C_T_arr[0] = cipherText[i];
                C_T_arr[3] = cipherText[i + 1];
                C_T_arr[6] = cipherText[i + 2];
                for (int j = i + m; j < plainText.Count; j+=m) // second col
                {
                    sub_P_T.Add(plainText[j]);
                    sub_P_T.Add(plainText[j + 1]);
                    sub_P_T.Add(plainText[j + 2]);

                    C_T_arr[1] = cipherText[j];
                    C_T_arr[4] = cipherText[j + 1];
                    C_T_arr[7] = cipherText[j + 2];
                    for (int k = j + m; k < plainText.Count; k+=m) // third col
                    {
                        sub_P_T.Add(plainText[k]);
                        sub_P_T.Add(plainText[k + 1]);
                        sub_P_T.Add(plainText[k + 2]);

                        C_T_arr[2] = cipherText[k];
                        C_T_arr[5] = cipherText[k + 1];
                        C_T_arr[8] = cipherText[k + 2];

                        for (int c = 0; c < m*m; c++)
                        {
                            sub_C_T.Add(C_T_arr[c]);
                        }

                        try
                        {
                            Key = Decrypt(sub_C_T, sub_P_T);
                        }
                        catch (Exception e)
                        {
                            Key = null;
                        }

                        if (Key != null)
                        {
                            List<int> C_T = Encrypt(plainText, Key); // to check the correct of the key
                            bool Key_Solved = true;
                            for (int c = 0; c < cipherText.Count; c++)
                            {
                                if (cipherText[c] != C_T[c]) { Key_Solved = false; break; }
                            }
                            if (Key_Solved) return Key;
                        }

                        sub_P_T.RemoveAt(8); // remove the last col
                        sub_P_T.RemoveAt(7);
                        sub_P_T.RemoveAt(6);

                        sub_C_T.Clear(); // clear sub cipher imediatly it will be re failed again in for loop
                    }
                    sub_P_T.RemoveAt(5); // remove the middle col
                    sub_P_T.RemoveAt(4);
                    sub_P_T.RemoveAt(3);
                }
                sub_P_T.Clear(); // remove all // and replace
            }
            throw new InvalidAnlysisException();
            //throw new NotImplementedException();
        }

        private List<int> matrix_mul(List<int> sub_text , List<int> key , int m , int mod)
        {
            List<int> result = new List<int>();

            int count = 0;
            List<int> sub_key = new List<int>();
            foreach (int letter in key)
            {
                sub_key.Add(letter);
                count++;

                if (count == m)
                {
                    int sum = 0;
                    for (int i = 0; i < m; i++)
                    {
                        if (sub_text[i] == -1) continue; // do not calculate it is an additonal index for just match the key weight 
                        sum += sub_text[i] * sub_key[i];
                    }
                    result.Add(sum % mod);

                    sub_key.Clear();
                    count = 0;
                }
            }
            return result;

        }
        private int Determinant_vlaue(List<int> det ,int m , int mod , int _base) // det(K)
        {
            int A = 0;
            if (m == 2) // matrix 2 x 2
            {
                A = (det[0] * det[3]) - (det[1] * det[2]); // ad x bc
            }
            else if (m >= 3) // matrix 3 x 3 or greter
            {
                List<int> sub_determinat;
                int row = 0; // first row   ( + , - , + )
                for (int col = 0; col < m; col++)
                {
                    sub_determinat = sub_Determinat(det, row, col, m);
                    int value = Determinant_vlaue(sub_determinat, m - 1, 0, _base); // mod 1 == no modulas
                    A += det[(row * m) + col] * value * ((int)Math.Pow(-1, col)); //calcualte the dterminant 
                }
            }

            if (mod != 0)
            {
                A %= mod;
                A = (A < 0) ? (A + _base) : (A);
            }
            return A;
        }
        private List<int> sub_Determinat(List<int>matrix , int row_no , int col_no , int m) // matrix 3 x 3 or greater
        {
            List<int> result = new List<int>();

            for (int row = 0; row < m; row++)
            {
                for (int col = 0; col < m; col++)
                {
                    if (row == row_no || col == col_no) continue; // skip this elment cause it is in the same row | col of the choosen elment
                    else
                        result.Add(matrix[(row * m) + col]);
                }
            }
            return result;
        }
        private int check_correct_key(List<int> key,int m , int determinant_val , bool GCD_checker)
        {
            //***** det(K) not equal zero
            if (determinant_val == 0 ) return -1; // det(K) != 0

            //**** GCD

            if (GCD_checker)
            {
                int _GCD = 0;
                /*if (determinant_val > 26) _GCD = GCD(determinant_val, 26);
                else _GCD = GCD(26, determinant_val);*/
                _GCD = GCD(26, determinant_val); // no way determinant  > 26 cause mod 26

                if (_GCD != 1) return -1; // Greatest common divisor GCD(det(K) , 26) = 1  if else ret -1

            }
            // ***** all element of key is positive and less than 26
            foreach (int item in key) // all element // positive and less than 26 if else ret -1
            {
                if (item < 0 || item >= 26) return -1;
            }

            //***** cal (b)

            int b = Multiplicative_inverse(26 , determinant_val); // multiplicative inverse of det(K)

            return b;
        }
        public int GCD(int a, int b) // Greatest common divisor
        {
            int remainder = a % b;
            if (remainder == 0) return b;
            else
                return GCD(b, remainder);
        }
        private int Multiplicative_inverse(int _base , int deter_val)
        {
            for (int b = 0; b < _base; b++)
            {
                if ((b * deter_val) % _base == 1)
                {
                    return b;
                }
            }
            return -1;
        }

        private List<int> K_inverse(List<int> matrix, int m ,int _base ,bool GCD_checker)
        {
            List<int> inverse = new List<int>();
            int[,] temp = new int[m, m];

            int determinant_value = Determinant_vlaue(matrix,m,_base , _base);

            int check = check_correct_key(matrix, m, determinant_value , GCD_checker);

            if (check == -1) return null; // key cannot be used
            int b = check; // cal (b) // maltiplicative inverse

            if (m == 2) // matrix 2 x 2 only special case// {a,b} ->  {d,-b}
            {                                            // {c,d} ->  {-c,a}
                int t = 0;
                t = matrix[0]; // a
                matrix[0] = matrix[3]; // a -> d
                matrix[3] = t; //d -> a
                matrix[1] = matrix[1] * -1; // -b
                matrix[2] = matrix[2] * -1; // -c

                foreach (int item in matrix)
                {
                    int k_inverse = (item * b) % _base;
                    k_inverse = (k_inverse < 0) ? (k_inverse + _base) : (k_inverse);
                    inverse.Add(k_inverse); // K(-1) inverse
                }
                // get matrix back
                t = matrix[0]; // a
                matrix[0] = matrix[3]; // a -> d
                matrix[3] = t; //d -> a
                matrix[1] = matrix[1] * -1; // -b
                matrix[2] = matrix[2] * -1; // -c
            }
            else // any size else
            {
                for (int i = 0; i < m; i++) // row
                {
                    for (int j = 0; j < m; j++) // col
                    {
                        List<int> sub_det = sub_Determinat(matrix, i, j, m);
                        int det_val = Determinant_vlaue(sub_det, m - 1 , _base, _base); // cal D(i,j) mod 26

                        int sign = (int)Math.Pow(-1, ((double)i + (double)j)); // cal -1 to pow(row + col)

                        int k_inverse_val = b * sign * det_val;
                        k_inverse_val %= 26;  // k inverse mod 26
                        k_inverse_val = (k_inverse_val < 0) ? (k_inverse_val + _base) : (k_inverse_val);

                        temp[j, i] = k_inverse_val;  // invert row , col apply Matrix Transpose
                    }
                }
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        inverse.Add(temp[i, j]);
                    }
                }
            }
            return inverse;
        }
    }
}
