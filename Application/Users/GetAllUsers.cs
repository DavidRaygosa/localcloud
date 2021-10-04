using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Users
{
    public class GetAllUsers
    {
        public class Run : IRequest<List<allUserDTO>>{}

        public class Handler : IRequestHandler<Run, List<allUserDTO>>
        {
            private readonly DataBaseContext _context;
            private List<allUserDTO> File = new List<allUserDTO>();

            public Handler(DataBaseContext context){
                _context = context;
            }
            public async Task<List<allUserDTO>> Handle(Run request, CancellationToken cancellationToken)
            {
                var users = await _context.User.ToListAsync();
                foreach(var user in users){
                    var image = await _context.Document.Where(x=>x.ObjectReference==new Guid(user.Id)).FirstOrDefaultAsync();
                    if(image!=null){
                        var imageProfile = new Image{
                            Data = Convert.ToBase64String(image.Content),
                            DataReferences = image.ContentReferences,
                            Extension = image.Extension,
                            Name = image.Name
                        };
                        File.Add(new allUserDTO{
                            Id = user.Id,
                            FullName = user.FullName,
                            Image = imageProfile
                        });
                    }else{
                        File.Add(new allUserDTO{
                            Id = user.Id,
                            FullName = user.FullName,
                            Image = null
                        });
                    }
                }
                return File;
            }
        }
    }
}