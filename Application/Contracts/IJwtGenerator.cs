using System.Collections.Generic;
using Domain;

namespace Application.Contracts
{
    public interface IJwtGenerator
    {
         string CreateToken(User user, List<string> roles, int TimeInDays);
    }
}