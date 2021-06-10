using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;
namespace TCCCMS.CryptoUtility
{
    public class EncodeDecode
    {
        #region DecryptString
        public static string DecryptString(string toDecryptString)
        {
            byte[] keyArray;
            byte[] toDecryptArray = Convert.FromBase64String(toDecryptString);
            string key = "7";
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF7.GetBytes(key));
            hashmd5.Clear();
            byte[] key24Array = new byte[24];
            for (int i = 0; i < 16; i++)
            {
                key24Array[i] = keyArray[i];
            }
            for (int i = 0; i < 7; i++)
            {
                key24Array[i + 16] = keyArray[i];
            }
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = key24Array;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = tripledes.CreateDecryptor();
            byte[] resultArray = cryptoTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            tripledes.Clear();
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetString(resultArray);
        }
        #endregion

        #region EncryptString
        public static string EncryptString(string toEncryptString)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncryptString);
            string key = "7";
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF7.GetBytes(key));
            hashmd5.Clear();
            byte[] key24Array = new byte[24];
            for (int i = 0; i < 16; i++)
            {
                key24Array[i] = keyArray[i];
            }
            for (int i = 0; i < 7; i++)
            {
                key24Array[i + 16] = keyArray[i];
            }
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = key24Array;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = tripledes.CreateEncryptor();
            byte[] resultArray = cryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tripledes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion

        #region IsEmailValid
        public static bool IsEmailValid(string EmailAddr)
        {
            bool bReturn = false;

            if (EmailAddr != null || EmailAddr != "")
            {
                string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                                + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{2,4})$";

                //Regex n = new Regex("(?<user>[^@]+)@(?<host>.+)");
                Regex n = new Regex(MatchEmailPattern);
                Match v = n.Match(EmailAddr);

                if (!v.Success || EmailAddr.Length != v.Length)
                {
                    bReturn = false;
                }
                else
                {
                    bReturn = true;
                }

            }
            return bReturn;
        }
        #endregion
    }
}
