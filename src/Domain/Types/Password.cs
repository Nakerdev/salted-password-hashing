using SaltedPasswordHashing.Src.Domain.Security;
using System.Linq;

namespace SaltedPasswordHashing.Src.Domain.Types
{
    public sealed class Password
    {
        public string Value { get; private set; }
        public Salt SaltProp { get; private set; }

        private Password(string value)
        {
            this.Value = value;
            this.SaltProp = null;
        }
        
        public static CreationResult<Password, Error> Create(string value)
        {
            if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return CreationResult<Password, Error>.CreateInvalidResult(error: Error.Required);
            }
            if(!IsValidPassword(password: value)){
                return CreationResult<Password, Error>.CreateInvalidResult(error: Error.InvalidFormat);
            }
            Password password = new Password(value: value);
            return CreationResult<Password, Error>.CreateValidResult(result: password);
        }

        public void Encrypt(
            PasswordEncryptionService passwordEncryptionService,
            SecurePseudoRandomGenerator securePseudoRandomGenerator)
        {
            Salt salt = securePseudoRandomGenerator.Generate();
            var encryptedPassword = passwordEncryptionService.Encrypt(this.Value, salt);
            this.Value = encryptedPassword;
            this.SaltProp = salt;
        }

        private static bool IsValidPassword(string password)
        {
            const int MIN_ALLOWED_PASSWORD_LENGHT = 8;
            return password.Length >= MIN_ALLOWED_PASSWORD_LENGHT 
                    && IsAlphanumeric()
                    && ContainsAtLeastOfOneUpperCaseLetter()
                    && ContainsAtLeastOfOneSymbol();

            bool IsAlphanumeric()
            {
                return password.Any(char.IsNumber) && password.Any(char.IsLetter); 
            }

            bool ContainsAtLeastOfOneUpperCaseLetter(){
                return password.Any(char.IsUpper);
            }

            bool ContainsAtLeastOfOneSymbol(){
                return password.Any(char.IsSymbol);
            }
        }

        public sealed class Salt
        {
            public long Value { get; }
            public Salt(long value)
            {
                this.Value = value;
            }
        }
    }
}