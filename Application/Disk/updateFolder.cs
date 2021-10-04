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
    public class updateFolder
    {
        public class Run : IRequest{
            public string path{get;set;}
            public string folderName{get;set;}
            public string index{get;set;}
        }

        public class RunValidator : AbstractValidator<Run>{
            public RunValidator(){
                RuleFor(x=>x.path).NotEmpty();
                RuleFor(x=>x.folderName).NotEmpty();
                RuleFor(x=>x.index).NotEmpty();
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
                if(Directory.Exists(folder.Directory+"\\"+request.folderName)) throw new ManejadorExcepcion(HttpStatusCode.Accepted, new {message="Folder name exists, choose another."});
                var files = Directory.EnumerateFiles(request.path, "*", SearchOption.AllDirectories);
                foreach(var file in files){
                    string[] pathSplit = file.Split('\\');
                    pathSplit[(2+Int32.Parse(request.index))] = request.folderName;
                    string newPath = string.Join("\\", pathSplit);
                    var fileDB = await _context.File.Where(x=>x.fullPath==file).FirstAsync();
                    if(fileDB!=null) fileDB.fullPath = newPath;
                }
                Directory.Move(folder.Directory+"\\"+folder.Name, folder.Directory+"\\"+request.folderName);
                var result = await _context.SaveChangesAsync();
                if(result>0) return Unit.Value;
                throw new ManejadorExcepcion(HttpStatusCode.Accepted, new {message="API UPDATE FOLDER ERROR"});
            }
        }
    }
}