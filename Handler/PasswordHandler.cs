using SS1892.EPLPredictor.Models;
using System.Security.Cryptography;
using System.Text;

namespace SS1892.EPLPredictor.Handler
{
    public class PasswordHandler
    {
        public static string GetHashPassword(string password)
        {
            var pwd=String.Empty;

            using (MD5 md5Hash = MD5.Create())
            {                
                 pwd = PasswordHandler.GetMd5Hash(md5Hash, password);
            }
            return pwd;
        }
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes  
            // and create a string.  
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string.  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.  
            return sBuilder.ToString();
        }
        // Verify a hash against a string.  
        public static bool CompareHash(string inputHash, string expectedHash)
        {

            // Create a StringComparer an compare the hashes.  
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(inputHash, expectedHash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
