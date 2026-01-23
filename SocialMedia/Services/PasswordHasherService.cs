using Konscious.Security.Cryptography;
using Microsoft.Extensions.Options;
using SocialMedia.DTO;
using SocialMedia.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;


namespace SocialMedia.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
      
        private readonly HashConfig _config;
        static int Parallelism, Iterations, memorySize, HashSize, saltSize;



        public PasswordHasherService(IOptions<HashConfig> options) 
        {
            _config = options.Value;
            Parallelism = _config.DegreeOfParallelism;
            Iterations = _config.Iterations;
            memorySize = _config.MemoryAllocation;
            saltSize = _config.SaltSize;
            HashSize = _config.HashSize;
           



        }

        private static byte[] HashPass(byte[] passwordBytes, byte[] salt)
        {
            using var argon2 = new Argon2id(passwordBytes)
            {
                Salt = salt,
                DegreeOfParallelism = Parallelism,
                Iterations = Iterations,
                MemorySize = memorySize
            };

            return argon2.GetBytes(HashSize);
        }

     


        private static byte[] hashPassUsingArgon(string password)
        {
          
            byte[] saltBytes = new byte[saltSize];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] HashedPass = HashPass(passwordBytes, saltBytes);

            byte[] CombinedBytes = new byte[saltSize + HashSize];

            System.Buffer.BlockCopy(HashedPass, 0, CombinedBytes, 0, HashSize) ;

            System.Buffer.BlockCopy(saltBytes, 0, CombinedBytes, HashSize,saltSize);

            return CombinedBytes;


        }

        private static bool VerifyPass(string password, byte[] CombinedBytes)
        {

            byte[] NewPasswordBytes = Encoding.UTF8.GetBytes(password);
            byte[] StoredPasswordBytes = CombinedBytes.Take(HashSize).ToArray();
            byte[] SaltBytes = CombinedBytes.Skip(HashSize).ToArray();

            byte[] NewPassHashedBytes = HashPass(NewPasswordBytes, SaltBytes);

            return CryptographicOperations.FixedTimeEquals(StoredPasswordBytes, NewPassHashedBytes);
        }

        public byte[] HashPassword(string password)
        {

            //byte[] pass = Encoding.UTF8.GetBytes(password);
            //byte[] hash = MD5.HashData(pass);
            //return hash;

            return hashPassUsingArgon(password);

            
        }

       
        public bool VerifyPassword(string password, byte[] hashedPassword)
        {
            //byte[] newHash = HashPassword(password);
            //return CryptographicOperations.FixedTimeEquals(hashedPassword, newHash);

            return VerifyPass(password, hashedPassword);    
        }
    }
}
