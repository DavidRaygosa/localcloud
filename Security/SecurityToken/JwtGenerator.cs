using System.Net.Mime;
using System;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Contracts;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace Security
{
    public class JwtGenerator : IJwtGenerator
    {
        public string CreateToken(User user, List<string> roles, int TimeInDays)
        {
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };
            // ADD ROLES TO JWT
            if(roles != null){
                foreach(var rol in roles){
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("My Secret Password Cloud"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescription = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(TimeInDays),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}