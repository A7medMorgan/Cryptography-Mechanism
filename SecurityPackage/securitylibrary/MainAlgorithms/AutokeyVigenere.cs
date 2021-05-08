using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        private static int ascii = 97; //Shiffting_Amount_alphabetic
        int _base = 26;

        public string Analyse(string plainText, string cipherText)
        {
            if (plainText.Length != cipherText.Length) throw new IndexOutOfRangeException();

            string keyword = Decrypt(cipherText, plainText);
            string key = "";

            for (int i = 0; i < keyword.Length; i++) // remove reapetted terms
            {
                if (key.Length != 0) // avoid null index
                {
                    if (keyword[i] == plainText[0])
                    {
                        string sub_key = keyword.Substring(i, keyword.Length - i); // get reamin of keyword

                        if (sub_key.Equals(plainText.Substring(0, sub_key.Length))) break; // match the pattern between  plain text and the extracted final key
                    }
                }

                key += keyword[i];
            }

            return key;
            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            key = key.ToLower();

            string PlainText = "";

            int[,] vigenere_tableau = VigenereTableau(26);

            int P_T_shiffter = 0;
            for (int _char = 0; _char < cipherText.Length; _char++)
            {
                for (int col = 0; col < _base; col++)  // row  = keyword ,, col = text 
                {
                    if (_char < key.Length) // take from key
                    {
                        if (((int)(cipherText[_char] - ascii)) == vigenere_tableau[(int)(key[_char] - ascii), col])
                        {
                            PlainText += (char)(col + ascii);
                            break;
                        }
                    }
                    else // repeaat from P.T
                    {
                        int row = (int)(PlainText[P_T_shiffter % PlainText.Length]-ascii);
                        if (((int)(cipherText[_char] - ascii)) == vigenere_tableau[row , col])
                        {
                            PlainText += (char)(col + ascii);
                            P_T_shiffter++;
                            break;
                        }
                    }
                }
            }
            return PlainText;
            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToLower();
            key = key.ToLower();

            //string keyword = plainText.Substring(0, plainText.Length - key.Length);
            string keyword = KeyWord(plainText, key);

            int[,] Vigenere_Tableau = VigenereTableau(_base);

            string CipherText = "";

            for (int _char = 0; _char < plainText.Length; _char++)
            {
                CipherText += (char)(Vigenere_Tableau[(int)(plainText[_char] - ascii), (int)(keyword[_char] - ascii)] + ascii); // row  = text ,, col = key 
            }

            return CipherText;
            //throw new NotImplementedException();
        }
        private string KeyWord(string text, string key)
        {
            string keyword = key;
            if (key.Length >= text.Length) return keyword;

            int shiffter = 0;
            for (int lettar = key.Length; lettar < text.Length; lettar++)
            {
                keyword += text[shiffter % text.Length];
                shiffter++;
            }
            return keyword;
        }

        private int[,] VigenereTableau(int _base)
        {
            int[,] result = new int[_base, _base];

            for (int row = 0; row < _base; row++)
            {
                for (int col = 0; col < _base; col++)
                {
                    result[row, col] = (row + col) % _base;
                }
            }

            return result;
        }
    }
}
