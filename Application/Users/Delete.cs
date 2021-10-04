using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users
{
    public class Delete
    {
        public class Run : IRequest{
            public string UserName{get;set;}
        }

        public class ValidateRun : AbstractValidator<Run>{
            public ValidateRun(){
                RuleFor(x=>x.UserName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Run>{
            private readonly UserManager<User> _userManager;

            public Handler(UserManager<User> userManager){
                _userManager = userManager;
            }
            public async Task<Unit> Handle(Run request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                var result = await _userManager.DeleteAsync(user);
                if(result.Succeeded) return Unit.Value;
                throw new Exception("No se pudo eliminar el usuario");
            }
        }
    }
}