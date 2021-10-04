using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.ManejadorError;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Disk
{
    public class DeleteFile
    {
        public class Run : IRequest{
            public Guid ID{get;set;}
        }

        public class Handler : IRequestHandler<Run>
        {
            private readonly DataBaseContext _context;
            public Handler(DataBaseContext context){
                _context = context;
            }
            public async Task<Unit> Handle(Run request, CancellationToken cancellationToken)
            {
                var file = await _context.File.FindAsync(request.ID);
                if(file==null) throw new ManejadorExcepcion(HttpStatusCode.NotFound, new{message="FIle Not Found"});
                _context.File.Remove(file);
                if(!deleteFileFromFolder(file.fullPath)) throw new Exception("Deleted File Folder Error");
                var result = await _context.SaveChangesAsync();
                if(result>0) return Unit.Value;
                throw new Exception("DB Error - File Not Deleted");
            }
        }

        static Boolean deleteFileFromFolder(string Path){
            FileInfo file = new FileInfo(Path);  
            if(file.Exists){file.Delete();return true;}
            return false;
        }
    }
}