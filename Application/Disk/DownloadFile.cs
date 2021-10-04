using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Disk
{
    public class DownloadFile
    {
        public class Run : IRequest<FileContentResult>{
            public string filePath{get;set;}
        }

        public class ValidateRun : AbstractValidator<Run>{
            public ValidateRun(){
                RuleFor(x=>x.filePath).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Run, FileContentResult>{
            public Task<FileContentResult> Handle(Run request, CancellationToken cancellationToken)
            {
                var data = File.ReadAllBytes(request.filePath);
                var result = new FileContentResult(data, "application/octet-stream"){
                    FileDownloadName = "File.csv"
                };
                return Task.FromResult<FileContentResult>(
                    result
                );
            }
        }
    }
}