using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            string CipherText = ""; int shiffting_amount = 97; // shiffting of the ascii code of (a==97) to be (a==0)
            for (int i = 0; i < plainText.Length; i++) //convert the plain text to ascii code
            {
                int char_PT = ((int)plainText[i]) - shiffting_amount; // get the ascii with (a == 0)

                // apply the algo
                int E_char = (char_PT + key) % 26;

                CipherText += (char)(E_char + shiffting_amount);
            }
            return CipherText;
            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, int key)
        {
            cipherText = cipherText.ToLower();
            string PlainText = ""; int shiffting_amount = 97; // shiffting of the ascii code of (a==97) to be (a==0)
            for (int i = 0; i < cipherText.Length; i++) //convert the plain text to ascii code
            {
                int char_CT = ((int)cipherText[i]) - shiffting_amount; // get the ascii with (a == 0)

                // apply the algo
                int D_char = (26 + (char_CT - key)) % 26; // +26 to prevent the negative subttraction

                PlainText += (char)(D_char + shiffting_amount);
            }
            return PlainText;
        }

        public int Analyse(string plainText, string cipherText)
        {
            if (plainText.Length != cipherText.Length) return -1; // make sure they equle

            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            int key = 0 , shiffting_amount = 97; // shiffting of the ascii code of (a==97) to be (a==0)
            int counter = 0;

            for (int i = 0; i < 26; i++) // brute-force attack
            {
                for (int c = 0; c < plainText.Length; c++)
                {
                    if (((int)cipherText[c] - shiffting_amount) == ((((int)plainText[c] - shiffting_amount) + i) % 26))
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                        break;
                    }
                }
                if (counter == plainText.Length)
                {
                    key = i;
                    break;
                }
            }
            return key;
        }
    }
}
