using System.Security.Cryptography;
using System.Text;

namespace Utilities.Custom
{
    public class EncriptePassword
    {
        // Formato almacenado: algo$iter$saltBase64$hashBase64
        private const int SaltSize = 16;     // 128 bits
        private const int KeySize = 32;     // 256 bits
        private const int Iterations = 100_000; // coste (ajústalo según tu servidor)

        public string Hash(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256
            );
            var key = pbkdf2.GetBytes(KeySize);

            return $"pbkdf2-sha256${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(key)}";
        }

        public bool Verify(string password, string stored)
        {
            // esperado: pbkdf2-sha256$iter$salt$hash
            var parts = stored.Split('$', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 4 || parts[0] != "pbkdf2-sha256") return false;

            var iter = int.Parse(parts[1]);
            var salt = Convert.FromBase64String(parts[2]);
            var expected = Convert.FromBase64String(parts[3]);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                iter,
                HashAlgorithmName.SHA256
            );
            var key = pbkdf2.GetBytes(expected.Length);

            // comparación en tiempo constante
            return CryptographicOperations.FixedTimeEquals(key, expected);
        }
    }
}
