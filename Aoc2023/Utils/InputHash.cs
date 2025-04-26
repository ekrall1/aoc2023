using System.Security.Cryptography;
using System.Text;

namespace Aoc2023
{
    public class InputHash
    {
        private InputHash() { }

        public static string GetListHash(List<string> data)
        {
            using var sha256 = SHA256.Create();
            var joinedDataBytes = Encoding.UTF8.GetBytes(string.Join("|", data));
            var hashed = sha256.ComputeHash(joinedDataBytes);
            return Convert.ToBase64String(hashed);
        }
    }
}