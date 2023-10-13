using Microsoft.AspNetCore.DataProtection;

namespace Infraestructure
{
    public static class Encriptation
    {
        
        public static string Encrypt(string input)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(input);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);

            return hash;
        }

        
    }
}

    