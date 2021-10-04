using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class Rol : MyControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(newRole.Run parametros){
            return await Mediator.Send(parametros);
        }

        [HttpDelete]
        public async Task<ActionResult<Unit>> Delete(deleteRole.Run parametros){
            return await Mediator.Send(parametros);
        }

        [HttpGet]
        public async Task<ActionResult<List<IdentityRole>>> List(){
            return await Mediator.Send(new roleList.Run());
        }

        [HttpPost("addUserRole")]
        public async Task<ActionResult<Unit>> AddUserRole(addUserRole.Run parametros){
            return await Mediator.Send(parametros);
        }

        [HttpPost("deleteUserRole")]
        public async Task<ActionResult<Unit>> DeleteUserRole(deleteUserRole.Run parametros){
            return await Mediator.Send(parametros);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<List<string>>> GetRolesByUser(string username){
            return await Mediator.Send(new GetUserRoles.Run{Username=username});
        }
    }
}