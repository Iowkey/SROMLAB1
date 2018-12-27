using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        public static string GetNumbers()
        {
            Console.Write("\nEnter a number: ");
            string hex1 = Console.ReadLine();
            return hex1;
        }

        public static void ShowHex(string hex1)
        {
            Console.Write("\nHEX = {0}", hex1);

        }

        public static string GetNumbers(string smth)
        {
            return smth;

        }

        public static ulong[] NumToArr(string num)
        {
            var temp = num;
            while (temp.Length % 8 != 0)
            {
                temp = "0" + temp;
            }
            ulong[] Arr1 = new ulong[temp.Length / 8];
            for (int i = 0; i < temp.Length; i += 8)
            {
                Arr1[i / 8] = Convert.ToUInt64(temp.Substring(i, 8), 16);
            }
            Array.Reverse(Arr1);
            return Arr1;
        }

        public static void ShowDecimal(ulong[] arr)
        {
            Console.Write("\nDecimal: ");
            foreach (ulong x in arr)
            {
                Console.Write(x + " ");
            }
        }

       /* public static ulong[] LongAddition(ulong[] a, ulong[] b)
        {
            ulong carry = 0;
            ulong temp = 0;
            ulong[] Result = new ulong[Math.Max(a.Length, b.Length) + 1];
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                temp = a[i] + b[i] + carry;
                carry = temp >> 32;
                Result[i] = temp & 0xFFFFFFFF;
            }
            Result[a.Length] = carry;
            return Result;
        }*/
        public static ulong[] LongAddition(ulong[] a, ulong[] b)
        {
            var maxlenght = Math.Max(a.Length, b.Length);
            Array.Resize(ref a, maxlenght);
            Array.Resize(ref b, maxlenght);
            var answer = new ulong[maxlenght + 1];

            ulong carry = 0;
            for (int i = 0; i < maxlenght; i++)
            {
                ulong temp = a[i] + b[i] + carry;
                carry = temp >> 32;
                answer[i] = temp & 0xffffffff;
            }
            answer[a.Length] = carry;
            return answer;
        }

        public static ulong[] LongSub(ulong[] a, ulong[] b)
        {
            var maxlenght = Math.Max(a.Length, b.Length);
            Array.Resize(ref a, maxlenght);
            Array.Resize(ref b, maxlenght);
            ulong borrow = 0;
            ulong temp = 0;
            ulong[] ResultSub = new ulong[maxlenght];
            for (int i = 0; i < a.Length; i++)
            {
                temp = a[i] - b[i] - borrow;
                ResultSub[i] = temp & 0xFFFFFFFF;
                if (temp <= a[i])
                    borrow = 0;
                else
                    borrow = 1;
            }

            return ResultSub;
        }

        public static int LongCmp(ulong[] a, ulong[] b)
        {

            var maxlenght = Math.Max(a.Length, b.Length);
            Array.Resize(ref a, maxlenght);
            Array.Resize(ref b, maxlenght);
            for (int i = a.Length - 1; i > -1; i--)
            {
                if (a[i] < b[i]) { return -1; }
                if (a[i] > b[i]) { return 1; }
            }
            return 0;
        }

        public static ulong[] LongMulOneDigit(ulong[] a, ulong b)
        {
            ulong carry = 0;
            ulong temp = 0;
            ulong[] c = new ulong[a.Length + 1];
            for (int i = 0; i < a.Length; i++)
            {
                temp = a[i] * b + carry;
                carry = temp >> 32;
                c[i] = temp & 0xFFFFFFFF;
            }
            c[a.Length] = carry;
            return c;
        }

        public static ulong[] LongShiftDigitsToHigh(ulong[] a, int x)
        {
            ulong[] c = new ulong[a.Length + x];
            for (int i = 0; i < a.Length; i++)
            {
                c[i + x] = a[i];
            }
            return c;
        }

        public static ulong[] LongMul(ulong[] a, ulong[] b)
        {
            var mlength = Math.Max(a.Length, b.Length);
            Array.Resize(ref a, mlength);
            Array.Resize(ref b, mlength);
            ulong[] c = new ulong[(a.Length) * 2];
            ulong[] temp;
            for (int i = 0; i < a.Length; i++)
            {
                temp = LongMulOneDigit(a, b[i]);
                temp = LongShiftDigitsToHigh(temp, i);
                c = LongAddition(c, temp);
            }
            c = RemoveHighZeros(c);
            return c;
        }

        /*public static ulong[] LongSquare(ulong[] a)
        {

        }
        */

        public static ulong[] LongDiv(ulong[] a, ulong[] b)
        {
            var k = BitLength(b);
            var R = a;
            ulong[] Q = new ulong[a.Length];
            ulong[] T = new ulong[a.Length];
            ulong[] C = new ulong[a.Length];
            T[0] = 0x1;

            while (LongCmp(R, b) >= 0)
            {
                var t = BitLength(R);
                C = LongShiftBitsToHigh(b, t - k);
                if (LongCmp(R, C) == -1)
                {
                    t = t - 1;
                    C = LongShiftBitsToHigh(b, t - k);
                }
                R = LongSub(R, C);
                Q = LongAddition(Q, LongShiftBitsToHigh(T, t - k));
            }
            Q = RemoveHighZeros(Q);
            //r = R;
            return Q;
        }

        /* public static ulong[] LongDivMod(ulong[] a, ulong[] b)
         {
             int k = BitLength(b);
             ulong[] R = a;
             ulong[] Q = new ulong[];
             while (R >= b)
             {

             }
         }*/

        public static ulong[] LongPowerWindow(ulong[] a, ulong[] b)
        {
            var uone = NumToArr("1");
            var b_zero = ShowResult(b);
            while(b_zero == "0")
                return uone;

            string numofb = Program.ShowResult(b);
            ulong[] C = new ulong[1];
            C[0] = 0x1;
            ulong[][] D = new ulong[16][];
            D[0] = new ulong[1] { 1 };
            D[1] = a;
            for (int i = 2; i < 16; i++)
            {
                D[i] = LongMul(D[i - 1], a);
                D[i] = RemoveHighZeros(D[i]);
            }
            for (int i = 0; i < numofb.Length; i++)
            {
                C = LongMul(C, D[Convert.ToInt32(numofb.ToString(), 16)]);
                if (i != numofb.Length - 1)
                {
                    for (int k = 1; k <= 4; k++)
                    {
                        C = LongMul(C, C);
                        C = RemoveHighZeros(C);
                    }
                }
            }
            return C;
        }
        

        public static ulong[] LongShiftBitsToHigh(ulong[] a, int b)
        {
            int t = b / 32;
            int s = b - t * 32;
            ulong n, carry = 0;
            ulong[] C = new ulong[a.Length + t + 1];
            for (int i = 0; i < a.Length; i++)
            {
                n = a[i];
                n = n << s;
                C[i + t] = (n & 0xFFFFFFFF) + carry;
                carry = (n & 0xFFFFFFFF00000000) >> 32;
            }
            C[C.Length - 1] = carry;
            return C;
        }

        public static int BitLength(ulong[] a)
        {
            int bit = 0;
            int i = a.Length - 1;
            while (a[i] == 0)
            {
                if (i < 0)
                    return 0;
                i--;
            }

            var ai = a[i];

            while (ai > 0)
            {
                bit++;
                ai = ai >> 1;
            }
            bit = bit + 32 * i;
            return bit;
        }

        public static ulong[] RemoveHighZeros(ulong[] c)
        {
            int i = c.Length - 1;
            while (c[i] == 0)
            {
                i--;
            }
            ulong[] result = new ulong[i + 1];
            Array.Copy(c, result, i + 1);
            return result;
        }

        public static string ShowResult(ulong[] a)
        {
            string st = string.Concat(a.Select(chunk => chunk.ToString("X").PadLeft(sizeof(ulong), '0')).Reverse()).TrimStart('0');

            return string.IsNullOrEmpty(st) ? "0" : st;
        }



        static void Main(string[] args)
        {
            string hex1 = GetNumbers();
            string hex2 = GetNumbers();

            ulong[] Arr2 = NumToArr(hex2);
            ulong[] Arr1 = NumToArr(hex1);

            //ulong[] Result = LongAddition(Arr1, Arr2);
            //Console.Write("\nAddition: " + ShowResult(Result));

            //ulong[] ResultSub = LongSub(Arr1, Arr2);
            //Console.Write("\nSubstraction: " + ShowResult(ResultSub));

            //Console.Write("\nResult: " + LongCmp(hex1, hex2));

            //Console.Write("\nLongMulOneDigit")
            //ulong[] ResultPow = LongPowerWindow(hex1, hex2);
            //ulong[] ResultMul = LongMul(Arr1, Arr2);
            //ulong[] ResultDiv = LongDiv(Arr1, Arr2);
            //Console.Write("\nPower: " + ShowResult(ResultPow));
            //Console.Write("\nDivision: " + ShowResult(ResultDiv));
            //Console.Write("\nMultiplication: " + ShowResult(ResultMul));
            Console.Write("\nPress any key..");
            Console.ReadKey();

        }
    }
}
