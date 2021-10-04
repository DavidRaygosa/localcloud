using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
//using Application.ExceptionHandler;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Security
{
    public class deleteRole
    {
        public class Run : IRequest{
            public string Name{get;set;}
        }

        public class ValidateRun : AbstractValidator<Run>{
            public ValidateRun(){
                RuleFor(x=>x.Name).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Run>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            public Handler(RoleManager<IdentityRole> roleManager){
                _roleManager = roleManager;
            }
            public async Task<Unit> Handle(Run request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.Name);
                //if(role == null) throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new{mensaje="No existe el rol"});
                var resultado = await _roleManager.DeleteAsync(role);
                if(resultado.Succeeded) return Unit.Value;
                throw new Exception("No se pudo eliminar el rol");
            }
        }
    }
}