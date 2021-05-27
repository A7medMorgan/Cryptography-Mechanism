using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN) 
        {

            //return multi_invervse(number, baseN);

            return multi_inverse_methode2(number, baseN);

            //throw new NotImplementedException();
        }
        public int gcdExtended(int a, int b,ref int x,ref int y)
        {

            // Base Case
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            // To store results of recursive call
            int x1 = 0, y1 = 0;
            int gcd = gcdExtended(b % a, a, ref x1, ref y1);

            // Update x and y using results of recursive
            // call
            x = y1 - (b / a) * x1;
            y = x1;

            return gcd;
        }

        private int multi_invervse_method1(int number, int baseN) //Method 1 (Naive)
        {
            //A Naive method is to try all numbers from 1 to base. For every number x, check if (number * x)% m is 1.
            for (int x = 1; x < baseN; x++) 
                if (((number % baseN) * (x % baseN)) % baseN == 1)
                    return x;
            return -1;
        }
        private int multi_inverse_methode2(int number, int baseN) //  Method 2
        {

            // The idea is to use Extended Euclidean algorithms

            int x = 0, y = 0;
            int gcd = gcdExtended(number, baseN, ref x, ref y);
            if (gcd != 1)
                return -1;
            else
            {
                // baseN is added to handle negative x
                int res = (x % baseN + baseN) % baseN;
                return res;
            }
        }
    }
}
