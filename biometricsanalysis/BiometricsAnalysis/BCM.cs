using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiometricsAnalysis
{
    class BCM
    {


        int numar = 0;
        byte[] intBytes;
        byte[] result;
        int numar2 = 0;
        byte[] intBytes2;
        byte[] result2;

        byte[] intBytes3;
        int result_f = 0;

        string xored = "";
        string plainText = "";
        byte[] plainText_bytes;
        string plainText_bytes_string = "";
        string cyptedText = "";
        string decrypted = "";

        string string_numar1 = "", string_numar2 = "";

        public string getCrypted { get { return cyptedText; } set { cyptedText = value; } }
        public string getDecrypted { get { return BinaryToString(decrypted); } set { decrypted = value; } }
        public string getDecryptes_binary { get { return decrypted; } set { decrypted = value; } }
        
        public string crypt_text(string plaintext, int number1, int number2) { 
        
            getNumber1(number1);
            getNumber2(number2);
            xorNumber();
            getText(plaintext);
            crypt();

            return this.getCrypted;  
        }

        public string crypt_binary(string binary, int number1, int number2) {
            getNumber1(number1);
            getNumber2(number2);
            xorNumber();
            plainText_bytes_string = binary;
            crypt();
            return this.getCrypted;
        }

        public string decrypt_text(string ctext, int number1, int number2)
        {

            getNumber1(number1);
            getNumber2(number2);
            xorNumber();
            this.cyptedText = ctext;
            decrypt();
            return this.getDecrypted;
        }

        public void getNumber1(int number)
        {
            numar = Convert.ToInt32(number);
            intBytes = BitConverter.GetBytes(numar);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);

            result = intBytes;
            numar = BitConverter.ToInt32(result, 0);

            string s = Convert.ToString(numar, 2);

            string_numar1 = s;
        }

        public void getNumber2(int number)
        {

            numar2 = Convert.ToInt32(number);
            intBytes2 = BitConverter.GetBytes(numar2);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes2);

            result2 = intBytes2;
            numar2 = BitConverter.ToInt32(result2, 0);

            string s = Convert.ToString(numar2, 2);

            string_numar2 = s;

        }


        public void getText(string plainText)
        {




            byte[] toBytes = Encoding.ASCII.GetBytes(plainText);

       //     plain_hexa.Text = BitConverter.ToString(toBytes);
        //    plain_hexa_l.Text = (toBytes.Length).ToString();

            plainText_bytes_string = StringToBinary(plainText);

        }

        public void xorNumber()
        {
            int size = 0;

            if (intBytes.Length > intBytes2.Length)
            {
                size = intBytes2.Length;
                intBytes3 = intBytes2;
            }
            else
            {
                size = intBytes.Length;
                intBytes3 = intBytes;
            }
            for (int i = 0; i < size; i++)
            {
                byte temp = (byte)(intBytes[i] ^ intBytes2[i]);


                intBytes3[i] = temp;



            }
            result_f = BitConverter.ToInt32(intBytes3, 0);

            string s_ = Convert.ToString(result_f, 2);


            int remainder;
            string result = string.Empty;
            while (result_f > 0)
            {
                remainder = result_f % 2;
                result_f /= 2;
                result = remainder.ToString() + result;
            }

            xored = s_;

        }

        public void crypt()
        {
            int j = 0;
            cyptedText = "";

            char[] crptText = new char[plainText_bytes_string.Length];

            for (int i = 0; i < plainText_bytes_string.Length; i++)
            {
                if (plainText_bytes_string[i] == '0' && xored[j] == '0' || plainText_bytes_string[i] == '1' && xored[j] == '1')
                {
                    crptText[i] = '1';
                }
                if (plainText_bytes_string[i] == '0' && xored[j] == '1' || plainText_bytes_string[i] == '1' && xored[j] == '0')
                {
                    crptText[i] = '0';
                }
                j++;
                if (j == xored.Length)
                    j = 0;

            }

            //  crypted_text.Text = BinaryToString(cyptedText);

            cyptedText = string.Join("", crptText);
        }

        public void decrypt()
        {
            int j = 0;
            decrypted = "";

            char[] decrText = new char[plainText_bytes_string.Length];

            for (int i = 0; i < plainText_bytes_string.Length; i++)
            {
                if (cyptedText[i] == '0' && xored[j] == '0' || cyptedText[i] == '1' && xored[j] == '1')
                {
                    decrText[i] = '1';
                }
                if (cyptedText[i] == '0' && xored[j] == '1' || cyptedText[i] == '1' && xored[j] == '0')
                {
                    decrText[i] = '0';
                }
                j++;
                if (j == xored.Length)
                    j = 0;

            }

            decrypted = string.Join("", decrText);

        }

        public static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }

            return Encoding.ASCII.GetString(byteList.ToArray());
        }

        public static string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

    }
}
