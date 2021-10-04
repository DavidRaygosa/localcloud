using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.ManejadorError;
using Domain;
using FluentValidation;
using MediatR;

namespace Application.Disk
{
    public class CreateFolder
    {
        public class Run : IRequest{
            public List<CreateFolderList> FolderList {get;set;}
        }
        public class Handler : IRequestHandler<Run>
        {
            public Task<Unit> Handle(Run request, CancellationToken cancellationToken)
            {
                foreach(var folder in request.FolderList){
                    folder.path = folder.path.Replace("$path$", "\\");
                    var fullPath = "";
                    if(folder.isDrop) fullPath = folder.root+"\\Cloud"+folder.path;
                    else fullPath = folder.root+"\\Cloud\\"+folder.path+folder.foldername;
                    if(!Directory.Exists(fullPath)){
                        var result = Directory.CreateDirectory(fullPath);
                    }else if(Directory.Exists(fullPath)){
                        throw new ManejadorExcepcion(System.Net.HttpStatusCode.Accepted, new{message="Folder Exists"});
                    }
                }
                return Task.FromResult<Unit>(
                    Unit.Value
                );
            }
        }
    }
}