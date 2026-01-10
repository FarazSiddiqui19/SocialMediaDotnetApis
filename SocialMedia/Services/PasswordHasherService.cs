using SocialMedia.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.SignalR;

namespace SocialMedia.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly IConfiguration _config;
        private  int SaltSize ;
        private  int HashSize ; // 256 bits
        private  int DegreeOfParallelism ; // Number of threads to use
        private  int Iterations ; // Number of iterations
        private  int MemorySize ; // 1 GB
        private readonly string _pepper;
        public PasswordHasherService(IConfiguration config) 
        {
            _config = config;
            SaltSize = int.Parse(_config["Hash_Config:saltSize"]);
            HashSize = int.Parse(_config["Hash_Config:HashSize"]); 
            DegreeOfParallelism = int.Parse(_config["Hash_Config:DegreeOfParallelism"]); 
            Iterations = int.Parse(_config["Hash_Config:Iterations"]);
            MemorySize = int.Parse(_config["Hash_Config:MemoryAllocation"]);
            _pepper = _config["Hash_Config:Pepper"];
        }
      

        public string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SaltSize];
          
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string pepperedPassword = password + _pepper;

            // 2. Convert that combined string into bytes
           

            // Create hash
            byte[] hash = HashPassword(pepperedPassword, salt);

            // Combine salt and hash
            var combinedBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
            Array.Copy(hash, 0, combinedBytes, salt.Length, hash.Length);

            // Convert to base64 for storage
            return Convert.ToBase64String(combinedBytes);
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = Iterations,
                MemorySize = MemorySize
            };

            return argon2.GetBytes(HashSize);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Decode the stored hash
            byte[] combinedBytes = Convert.FromBase64String(hashedPassword);

            // Extract salt and hash
            byte[] salt = new byte[SaltSize];
            byte[] hash = new byte[HashSize];
            Array.Copy(combinedBytes, 0, salt, 0, SaltSize);
            Array.Copy(combinedBytes, SaltSize, hash, 0, HashSize);
            string pepperedPassword = password + _pepper;

            // Compute hash for the input password
            byte[] newHash = HashPassword(pepperedPassword, salt);

            // Compare the hashes
            return CryptographicOperations.FixedTimeEquals(hash, newHash);
        }
    }
}
