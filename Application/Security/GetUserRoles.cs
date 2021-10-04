using System.Net;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
//using Application.ExceptionHandler;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Security
{
    public class GetUserRoles
    {
        public class Run : IRequest<List<string>>{
            public string Username{get;set;}
        }

        public class Handler : IRequestHandler<Run, List<string>>
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            public Handler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager){
                _userManager = userManager;
                _roleManager = roleManager;
            }
            public async Task<List<string>> Handle(Run request, CancellationToken cancellationToken)
            {
                var userIden = await _userManager.FindByNameAsync(request.Username);
                //if(userIden==null) throw new ManejadorExcepcion(HttpStatusCode.NotFound, new{mensaje="No existe el usuario"});
                var result = await _userManager.GetRolesAsync(userIden);
                return new List<string>(result);
            }
        }
    }
}