using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Disk
{
    public class DeleteDir
    {
        public class Run : IRequest{
            public string path{get;set;}
        }

        public class runValidator : AbstractValidator<Run>{
            public runValidator(){
                RuleFor(x=>x.path).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Run>
        {
            private readonly DataBaseContext _context;
            public Handler(DataBaseContext context){
                _context = context;                
            }
            public async Task<Unit> Handle(Run request, CancellationToken cancellationToken)
            {
                var folder = new FileInfo(request.path);
                var files = Directory.EnumerateFiles(request.path, "*", SearchOption.AllDirectories);
                foreach(var file in files){
                    var fileDB = await _context.File.Where(x=>x.fullPath==file).FirstAsync();
                    if(fileDB!=null) _context.File.Remove(fileDB);
                }
                Directory.Delete(request.path, true);
                var result = await _context.SaveChangesAsync();
                if(result>0) return Unit.Value;
                throw new ManejadorExcepcion(HttpStatusCode.Accepted, new {message="API DELETE DIR ERROR"});
            }
        }
    }
}