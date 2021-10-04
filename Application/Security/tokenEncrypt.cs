using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.ManejadorError;
using FluentValidation;
using MediatR;

namespace Application.Security
{
    public class tokenEncrypt
    {
        public class Run : IRequest<string>{
            public string tokenEncrypted{get;set;}
        }

        public class RunValidator : AbstractValidator<Run>{
            public RunValidator(){
                RuleFor(x=>x.tokenEncrypted).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Run, string>
        {
            public Task<string> Handle(Run request, CancellationToken cancellationToken)
            {
                var token = Decrypt(request.tokenEncrypted, "LocalCloud55!Password!$$");
                throw new ManejadorExcepcion(System.Net.HttpStatusCode.Accepted, new{message=token});
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes("pemgail9uzpgzl88");
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(256 / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            symmetricKey.Padding = PaddingMode.PKCS7;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            return plainText;
        }
    }
}