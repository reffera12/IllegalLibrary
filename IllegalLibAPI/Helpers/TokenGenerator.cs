using System.Security.Cryptography;

namespace IllegalLibAPI
{
    public class TokenGenerator
    {
        public string GenerateResetToken(string email)
        {
            const int tokenLength = 16;

            byte[] tokenBytes = new byte[tokenLength];

            using (RandomNumberGenerator randomNumber = RandomNumberGenerator.Create())
            {
                randomNumber.GetBytes(tokenBytes);
            }

            string tokenString = BitConverter.ToString(tokenBytes);

            return tokenString;
        }
    }
}