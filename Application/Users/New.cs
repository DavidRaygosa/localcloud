using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using FluentValidation;
using Application.ManejadorError;

namespace Application.Users
{
    public class Register
    {
        public class Run : IRequest<UserData>{
            public string Name{get;set;}
            public string LastName{get;set;}
            public string Email{get;set;}
            public string Password{get;set;}
            public string Username{get;set;}
            public string Language{get;set;}
            public Boolean isdark{get;set;}
        }
        public class EjecutaValidador : AbstractValidator<Run>{
            public EjecutaValidador(){
                RuleFor(x=>x.Name).NotEmpty();
                RuleFor(x=>x.LastName).NotEmpty();
                RuleFor(x=>x.Email).NotEmpty();
                RuleFor(x=>x.Password).NotEmpty();
                RuleFor(x=>x.Username).NotEmpty();
                RuleFor(x=>x.Language).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Run, UserData>
        {
            private readonly DataBaseContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            public Handler(DataBaseContext context, UserManager<User> userManager, IJwtGenerator jwtGenerator){
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }
            public async Task<UserData> Handle(Run request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where(x=>x.Email == request.Email).AnyAsync();
                if(existe) throw new ManejadorExcepcion(HttpStatusCode.Accepted, new {message="Email has been registered"});
                var existeUserName = await _context.Users.Where(x=>x.UserName == request.Username).AnyAsync();
                if(existeUserName) throw new ManejadorExcepcion(HttpStatusCode.Accepted, new {message="Username has been used"});
                var usuario = new User{
                    FullName = request.Name + " " + request.LastName,
                    Email = request.Email,
                    UserName = request.Username,
                    Language = request.Language,
                    isDark = request.isdark,
                    firstLogin = true
                };
                var resultado = await _userManager.CreateAsync(usuario, request.Password);
                if(resultado.Succeeded) return new UserData{
                    FullName = usuario.FullName,
                    Token = _jwtGenerator.CreateToken(usuario, null, 1),
                    Username = usuario.UserName,
                    Email = usuario.Email,
                    Language = usuario.Language,
                    isDark = usuario.isDark,
                    Image = null,
                    firstLogin = usuario.firstLogin
                };
                if(!resultado.Succeeded) throw new ManejadorExcepcion(HttpStatusCode.Accepted, new{message="Password mut be: Strong"});
                throw new Exception("Register API Error");
            }
        }
    }
}