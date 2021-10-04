using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Application.ManejadorError;
//using Application.ExceptionHandler;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Users
{
    public class UpdateUser
    {
        public class Run : IRequest<UserData>{
            public string FullName{get;set;}
            public Boolean isDark{get;set;}
            public string Language{get;set;}
            public string Password{get;set;}
            public string CurrentPassword{get;set;}
            public Boolean firstLogin {get;set;} 
        }

        public class Handler : IRequestHandler<Run, UserData>
        {
            private readonly DataBaseContext _context;
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserSession _userSesion;
            private readonly IPasswordHasher<User> _passwordHasher;
            public Handler(DataBaseContext context, UserManager<User> userManager, IJwtGenerator jwtGenerator, IPasswordHasher<User> passwordHasher, IUserSession userSesion, SignInManager<User> signInManager){
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _passwordHasher = passwordHasher;
                _userSesion = userSesion;
                _signInManager = signInManager;
            }
            public async Task<UserData> Handle(Run request, CancellationToken cancellationToken)
            {
                var userIden = await _userManager.FindByNameAsync(_userSesion.GetUserSession());
                if(userIden==null) throw new ManejadorExcepcion(HttpStatusCode.Accepted, new{message="USER NOT EXIST"});
                if(request.FullName!=null) userIden.FullName = request.FullName;
                userIden.isDark = request.isDark;
                if(request.Language!=null) userIden.Language = request.Language;
                if(request.Password!=null){
                    var password = await _signInManager.CheckPasswordSignInAsync(userIden, request.CurrentPassword, false);
                    if(password.Succeeded) userIden.PasswordHash = _passwordHasher.HashPassword(userIden, request.Password);
                    if(!password.Succeeded) throw new ManejadorExcepcion(HttpStatusCode.Accepted, new{message="Incorrect Current Password"});
                }
                userIden.firstLogin = request.firstLogin;
                // UPDATE
                var updateResult = await _userManager.UpdateAsync(userIden);
                if(updateResult.Succeeded){
                    return new UserData{
                        FullName = userIden.FullName,
                        isDark = userIden.isDark,
                        Language = userIden.Language
                    };
                }
                throw new Exception("API UPDATE USER ERROR");
            }
        }
    }
}