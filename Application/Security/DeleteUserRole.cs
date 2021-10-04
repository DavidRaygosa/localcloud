using System.Net;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
//using Application.ExceptionHandler;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;

namespace Application.Security
{
    public class deleteUserRole
    {
        public class Run : IRequest{
            public string username{get;set;}
            public string nameRole{get;set;}
        }

        public class ValidateRun : AbstractValidator<Run>{
            public ValidateRun(){
                RuleFor(x=>x.nameRole).NotEmpty();
                RuleFor(x=>x.username).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Run>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            public Handler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager){
                _userManager = userManager;
                _roleManager = roleManager;
            }
            public async Task<Unit> Handle(Run request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.nameRole);
                //if(role==null) throw new ManejadorExcepcion(HttpStatusCode.NotFound, new{mensaje="No se encontro el rol"});
                var usuarioIden = await _userManager.FindByNameAsync(request.username);
                //if(usuarioIden==null) throw new ManejadorExcepcion(HttpStatusCode.NotFound, new{mensaje="No se encontro el usuario"});
                var resultado = await _userManager.RemoveFromRoleAsync(usuarioIden, request.username);
                if(resultado.Succeeded) return Unit.Value;
                throw new Exception("No se pudo eliminar el rol");
            }
        }
    }
}