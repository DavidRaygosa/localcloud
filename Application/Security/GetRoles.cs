using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Security
{
    public class roleList
    {
        public class Run : IRequest<List<IdentityRole>>{
        }

        public class Handler : IRequestHandler<Run, List<IdentityRole>>
        {
            private readonly DataBaseContext _context;
            public Handler(DataBaseContext context){
                _context = context;
            }
            public async Task<List<IdentityRole>> Handle(Run request, CancellationToken cancellationToken)
            {
                var roles = await _context.Roles.ToListAsync();
                return roles;
            }
        }
    }
}