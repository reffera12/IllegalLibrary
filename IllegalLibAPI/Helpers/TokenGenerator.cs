using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IllegalLibAPI
{
    public class TokenGenerator
    {
        public string GenerateResetOrRefreshToken()
        {
            const int tokenLength = 32;

            byte[] tokenBytes = new byte[tokenLength];

            using (RandomNumberGenerator randomNumber = RandomNumberGenerator.Create())
            {
                randomNumber.GetBytes(tokenBytes);
                return Convert.ToBase64String(tokenBytes);
            }
        }
    }
}