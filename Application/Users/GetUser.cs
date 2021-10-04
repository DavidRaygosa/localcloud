using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Users
{
    public class currentUser
    {
        public class Run : IRequest<UserData>{
            public Boolean isRemember{get;set;}
        }
        public class Handler : IRequestHandler<Run, UserData>
        {
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerador;
            private readonly IUserSession _userSesion;
            private readonly DataBaseContext _context;
            public Handler(UserManager<User> userManager, IJwtGenerator jwtGenerador, IUserSession userSesion, DataBaseContext context){
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _userSesion = userSesion;
                _context = context;
            }
            public async Task<UserData> Handle(Run request, CancellationToken cancellationToken){
                var user = await _userManager.FindByNameAsync(_userSesion.GetUserSession());
                var resultadoRoles = await _userManager.GetRolesAsync(user); // GET USER ROLES
                var listRoles = new List<string>(resultadoRoles);

                var token = "";
                var tokenEnc = "";
                if(request.isRemember)
                {
                    token = _jwtGenerador.CreateToken(user, listRoles, 1095);
                    using (Aes myAes = Aes.Create())
                    {
                        // Encrypt the string to an array of bytes.
                        byte[] encrypted = EncryptStringToBytes_Aes(token, myAes.Key, myAes.IV);


                        StringBuilder builder = new StringBuilder();  
                        for (int i = 0; i < encrypted.Length; i++){  
                            builder.Append(encrypted[i].ToString("x2"));  
                        }  
                        tokenEnc = builder.ToString(); 
                    }
                }else{
                    token = _jwtGenerador.CreateToken(user, listRoles, 1);
                }

                var image = await _context.Document.Where(x=>x.ObjectReference==new Guid(user.Id)).FirstOrDefaultAsync();
                if(image!=null){
                    var imageProfile = new Image{
                        Data = Convert.ToBase64String(image.Content),
                        DataReferences = image.ContentReferences,
                        Extension = image.Extension,
                        Name = image.Name
                    };
                    return new UserData{
                        ID = user.Id,
                        FullName = user.FullName,
                        Username = user.UserName,
                        Token = token,
                        Image = imageProfile,
                        Email = user.Email,
                        isDark = user.isDark,
                        Language = user.Language,
                        firstLogin = user.firstLogin
                    };
                }else{
                    return new UserData{
                        ID = user.Id,
                        FullName = user.FullName,
                        Username = user.UserName,
                        Token = token,
                        Image = null,
                        Email = user.Email,
                        isDark = user.isDark,
                        Language = user.Language,
                        firstLogin = user.firstLogin
                    };
                }
            }

            static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
            {
                // Check arguments.
                if (plainText == null || plainText.Length <= 0)
                    throw new ArgumentNullException("plainText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");
                byte[] encrypted;

                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create an encryptor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }

                // Return the encrypted bytes from the memory stream.
                return encrypted;
            }
        }
    }
}