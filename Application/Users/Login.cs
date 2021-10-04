using System.Threading;
using System.Threading.Tasks;
//using Application.ExceptionHandler;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using FluentValidation;
using Application.Contracts;
using System.Collections.Generic;
using Application.ManejadorError;
using System;
using Persistence;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Application.Users
{
    public class Login
    {
        public class Run : IRequest<UserData>{
            public string Email{get;set;}
            public string Password{get;set;}
            public Boolean isRemember{get;set;}
        }
        public class ValidateRun : AbstractValidator<Run>{
            public ValidateRun(){
                RuleFor(x=>x.Email).NotEmpty();
                RuleFor(x=>x.Password).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Run, UserData>{
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly DataBaseContext _context;
            public Handler(UserManager<User> userManager, SignInManager<User> signInManager, IJwtGenerator jwtGenerator, DataBaseContext context){
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerator = jwtGenerator;
                _context = context;
            }
            public async Task<UserData> Handle(Run request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if(user == null) throw new ManejadorExcepcion(HttpStatusCode.Accepted, new{message="User Not Exists"});
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                var resulRoles = await _userManager.GetRolesAsync(user); // GET USER ROLES
                var listRoles = new List<string>(resulRoles);
                var token = "";
                var tokenEnc = "";
                if(request.isRemember){
                    token = _jwtGenerator.CreateToken(user,listRoles, 1095);
                    var password = "LocalCloud55!Password!$$";
                    tokenEnc = Encrypt(token,password);
                }else token = _jwtGenerator.CreateToken(user,listRoles, 1); 
                
                var image = await _context.Document.Where(x=>x.ObjectReference==new Guid(user.Id)).FirstOrDefaultAsync();
                if(result.Succeeded){
                    if(image!=null){
                        var imageProfile = new Image{
                            Data = Convert.ToBase64String(image.Content),
                            DataReferences = image.ContentReferences,
                            Extension = image.Extension,
                            Name = image.Name
                        };
                        return new UserData{
                            FullName = user.FullName,
                            Token = token,
                            TokenEncrypted = tokenEnc,
                            Username = user.UserName,
                            Email = user.Email,
                            Image = imageProfile
                        };
                    }else{
                        return new UserData{
                            FullName = user.FullName,
                            Token = token,
                            TokenEncrypted = tokenEnc,
                            Username = user.UserName,
                            Email = user.Email,
                            Image = null
                        };
                    }
                }
                if(!result.Succeeded) throw new ManejadorExcepcion(HttpStatusCode.Accepted, new{message="Password not match"});
                throw new Exception("Login API Error");
            }

            public static string Encrypt(string plainText, string passPhrase)
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes("pemgail9uzpgzl88");
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(256 / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                string cipherText = Convert.ToBase64String(cipherTextBytes);
                return cipherText;
            }

        }
    }
}