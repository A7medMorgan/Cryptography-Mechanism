using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        private static int shiffting_Amount_Alphabetic = 97; // to get the ascii code of the char by an index of array 26
        public string Analyse(string plainText, string cipherText)
        {
            if (plainText.Length != cipherText.Length) return null; // must be at the same length

            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();

            string key = "";
            char[] arr_key = new char[26];
            int[] arr_alpha = new int[26]; for (int i = 0; i < arr_alpha.Length; i++) arr_alpha[i] = 0;

            for (int c = 0; c < plainText.Length; c++)
            {
                arr_key[((int)plainText[c]) - shiffting_Amount_Alphabetic] = cipherText[c]; // P.T is the alphabet , C.T is the corespond key char
                arr_alpha[((int)cipherText[c]) - shiffting_Amount_Alphabetic] = 1; // mark the token char of the key
            }

            for (int k = 0; k < arr_key.Length; k++) // re fill the empty places at the key
            {
                if (arr_key[k] == '\0') // empty cell of the key
                {
                    for (int alpha = 0; alpha < arr_alpha.Length; alpha++)
                    {
                        if (arr_alpha[alpha] == 0) // if the char not taken
                        {
                            arr_key[k] = (char)(alpha + shiffting_Amount_Alphabetic);
                            arr_alpha[alpha] = 1; // mark as taken
                            break; // no need to continue
                        }
                    }
                }
                key += arr_key[k];
            }

            return key;
            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            if (key.Length != 26) return null; // check if the key does not equal the alphabet

            cipherText = cipherText.ToLower();
            key = key.ToLower();

            string PLainText = "";

            for (int c = 0; c < cipherText.Length; c++)
            {
                for (int alpha = 0; alpha < 26; alpha++) // loop on all the keys until find the one used for E*
                {
                    if (cipherText[c] == key[alpha])
                    {
                        PLainText += (char)(alpha + shiffting_Amount_Alphabetic); // insert the original alphabet corresbond to its ascii code
                        break;
                    }
                }
            }
            return PLainText;
            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            if (key.Length != 26) return null; // check if the key does not equal the alphabet

            plainText = plainText.ToLower();
            key = key.ToLower();

            string CipherText = "";

            for (int c = 0; c < plainText.Length; c++)
            {
                CipherText += key[((int)plainText[c]) - shiffting_Amount_Alphabetic]; // get the index by the order of the alphabet
            }
            return CipherText;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            cipher = cipher.ToLower();
            string PlainText = "";
            int[] alpha_freq = new int[26]; for (int i = 0; i < alpha_freq.Length; i++) alpha_freq[i] = 0;

            char[] Frequency_Information = { 'e', 't', 'a', 'o', 'i', 'n', 's', 'r', 'h', 'l', 'd', 'c', 'u', 'm', 'f', 'p', 'g', 'w', 'y', 'b', 'v', 'k', 'x', 'j', 'q', 'z' };

            for (int c = 0; c < cipher.Length; c++)
            {
                alpha_freq[((int)cipher[c]) - shiffting_Amount_Alphabetic] += 1;  // count the repettion of each latter
            }

            Dictionary<char, char> Mapping = new Dictionary<char, char>(); // for mapping the predicted key with the freq_info
            int max_freq = -1;
            int index_max_freq_latter = -1;

            for (int alpha = 0; alpha < Frequency_Information.Length; alpha++) // loop from the highest freq in the requency_Information
            {
                for (int f = 0; f < alpha_freq.Length; f++) // loop to get the coorespondeing latter match to generate the key
                {
                    if (alpha_freq[f] >= max_freq)
                    {
                        max_freq = alpha_freq[f];
                        index_max_freq_latter = f;
                    }
                }

                Mapping.Add(((char)(index_max_freq_latter + shiffting_Amount_Alphabetic)), Frequency_Information[alpha]); // key : Encrypted latter  , value : predicted latter based on freq-info- 

                alpha_freq[index_max_freq_latter] = -2; // delete the freq of that letter index

                max_freq = -1;
                index_max_freq_latter = -1;
            }

            foreach (KeyValuePair<char,char> item in Mapping)
            {
                Console.WriteLine("K:  "+item.Key+"    V:   "+item.Value);
            }

            for (int c = 0; c < cipher.Length; c++)
            {
                PlainText += Mapping[cipher[c]];
            }

            return PlainText;
            //throw new NotImplementedException();
        }
    }
}
