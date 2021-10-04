using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace Application.Disk
{
    public class UpdateFile
    {
        public class Run : IRequest<ActionResult<Unit>>{
            public string ID{get;set;}
            public string Name{get;set;}
        }

        public class ValidateRun : AbstractValidator<Run>{
            public ValidateRun(){
                RuleFor(x=>x.ID).NotEmpty();
                RuleFor(x=>x.Name).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Run, ActionResult<Unit>>
        {
            private readonly DataBaseContext _context;
            public Handler(DataBaseContext context){
                _context = context;
            }
            public async Task<ActionResult<Unit>> Handle(Run request, CancellationToken cancellationToken)
            {
                var file = await _context.File.FindAsync(new Guid(request.ID));
                var newFullPath = filNameInFolder(file.fullPath, request.Name);
                var fileName = changeNameFile(file.fullPath, request.Name, newFullPath);
                var fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
                file.fileName = fileNameOnly;
                file.fullPath = fileName;
                _context.File.Update(file);
                var result = await _context.SaveChangesAsync();           
                if(result>0) return Unit.Value;
                throw new Exception("DB Error - Update File Error");
            }
        }

        static string changeNameFile(string fullPath, string name, string sameName){
            if(sameName!=""){
                System.IO.File.Move(fullPath, sameName);
                return sameName;
            }else{
                string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                string extension = Path.GetExtension(fullPath);
                string path = Path.GetDirectoryName(fullPath);
                string newFullPath = Path.Combine(path, name + extension);
                System.IO.File.Move(fullPath, newFullPath);
                return newFullPath;
            }
        }

        static string filNameInFolder(string fullPath, string Name){
            int count = 1;
            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string path = Path.GetDirectoryName(fullPath);
            string newFullPath = path+"\\"+Name+extension;
            if(System.IO.File.Exists(newFullPath)){
                while(System.IO.File.Exists(newFullPath)){
                    string tempFileName = string.Format("{0}({1})", Name, count++);
                    newFullPath = Path.Combine(path, tempFileName + extension);
                }
                return newFullPath;
            }
            else return "";
        }
    }
}