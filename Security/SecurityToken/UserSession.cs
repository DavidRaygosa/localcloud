using System.Net.Mime;
using System.Security.Claims;
using System.Linq;
using Application.Contracts;
using Microsoft.AspNetCore.Http;

namespace Security
{
    public class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserSession(IHttpContextAccessor httpContextAccessor){
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserSession(){
            var userName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier)?.Value;
            return userName;
        }
    }
}