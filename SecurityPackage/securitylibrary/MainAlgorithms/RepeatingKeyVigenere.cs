using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        private static int  ascii = 97; //Shiffting_Amount_alphabetic
        int _base = 26;

        public string Analyse(string plainText, string cipherText)
        {
            if (plainText.Length != cipherText.Length) throw new IndexOutOfRangeException();

            string keyword = Decrypt(cipherText, plainText);
            string key = "";

            for (int i = 0; i < keyword.Length; i++) // remove reapetted terms
            {
                if(key.Length != 0) // avoid null index
                if (keyword[i] == key[0])
                {
                    if (keyword.Length - i > key.Length) // if the rest of keyword stil biger than key take chunck from it
                    {
                        string sub_key = keyword.Substring(i, key.Length); // take sub string from the keyword 
                        if (key.Equals(sub_key)) // if the patter match key found
                        {
                            break;
                        }
                    }
                    else
                    {
                        string sub_key = key.Substring(0, keyword.Length - i); //  take sub string from the key
                        if (key.Equals(sub_key))
                        {
                            break;
                        }
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

            string keyword = KeyWord(cipherText , key);

            int[,] vigenere_tableau = VigenereTableau(26);

            for (int _char = 0; _char < cipherText.Length; _char++)
            {
                for (int col = 0; col < _base; col++)  // row  = key ,, col = text 
                {
                    if (((int)(cipherText[_char] - ascii)) == vigenere_tableau[(int)(keyword[_char]-ascii) , col])
                    {
                        PlainText += (char)(col + ascii);
                        break;
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

            string CipherText = "";

            string keyword = KeyWord(plainText, key);

            int[,] vigenere_tableau = VigenereTableau(_base);
            for (int _char = 0; _char < plainText.Length; _char++)
            {
                CipherText += (char)(vigenere_tableau[(int)(plainText[_char] - ascii), (int)(keyword[_char] - ascii)] + ascii); // row  = text ,, col = key 
            }

            return CipherText;
            //throw new NotImplementedException();
        }

        private string KeyWord(string text, string key)
        {
            string keyword = key;
            if (key.Length >= text.Length) return keyword;
            
            for (int lettar = key.Length; lettar < text.Length; lettar++)
            {
                keyword += key[lettar % key.Length];
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