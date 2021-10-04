using System.Net;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.ManejadorError;

namespace Application.Documents
{
    public class Download
    {
        public class Run : IRequest<GenericFile>{
            public Guid Id{get;set;}
        }

        public class Handler : IRequestHandler<Run, GenericFile>
        {
            private readonly DataBaseContext _context;
            public Handler(DataBaseContext context){
                _context = context;
            }
            public async Task<GenericFile> Handle(Run request, CancellationToken cancellationToken)
            {
                var file = await _context.Document.Where(x=>x.ObjectReference==request.Id).FirstAsync();
                if(file==null) throw new ManejadorExcepcion(HttpStatusCode.Accepted, new{message="Image Not Found"});
                var genericFile = new GenericFile{
                    Data = Convert.ToBase64String(file.Content),
                    Name = file.Name,
                    Extension = file.Extension
                };
                return genericFile;
            }
        }
    }
}