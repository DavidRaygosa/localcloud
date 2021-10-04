using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Contracts;
using Application.ManejadorError;
using Application.Security;
using Application.Users;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class User : MyControllerBase
    {
        private readonly UserManager<Domain.User> _userManager;
        private readonly IUserSession _userSesion;
        public User(UserManager<Domain.User> userManager, IUserSession userSesion){
            _userManager = userManager;
            _userSesion = userSesion;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserData>> Login(Login.Run data){
            return await Mediator.Send(data);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserData>> Register(Register.Run data){
            return await Mediator.Send(data);
        }

        [HttpPost("currentUser")]
        public async Task<ActionResult<UserData>> returnUser(currentUser.Run data){
            return await Mediator.Send(data);
        }

        [HttpGet("allUsers")]
        public async Task<List<allUserDTO>> allUsers(){
            return await Mediator.Send(new GetAllUsers.Run());
        }

        [HttpPost("tokenEncrypt")]
        [AllowAnonymous]
        public Task<string> returnToken(tokenEncrypt.Run data){
            return Mediator.Send(data);
        }

        [HttpPut]
        public async Task<ActionResult<UserData>> update(UpdateUser.Run data){
            return await Mediator.Send(data);
        }

        [HttpDelete("{UserName}")]
        public async Task<ActionResult<Unit>> delete(string UserName){
            return await Mediator.Send(new Delete.Run{UserName=UserName});
        }
    }
}