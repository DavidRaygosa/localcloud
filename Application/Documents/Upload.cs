using System.Net;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.ManejadorError;
using FluentValidation;

namespace Application.Documents
{
    public class Upload
    {
        public class Run : IRequest{
            public string ObjectReference{get;set;}
            public string Data{get;set;}
            public string Name{get;set;}
            public string Extension{get;set;}
            public string DataReferences{get;set;}
        }

        public class ValidateRun : AbstractValidator<Run>{
            public ValidateRun(){
                RuleFor(x=>x.ObjectReference).NotEmpty();
                RuleFor(x=>x.Data).NotEmpty();
                RuleFor(x=>x.Name).NotEmpty();
                RuleFor(x=>x.Extension).NotEmpty();
                RuleFor(x=>x.DataReferences).NotEmpty();
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
                var document = await _context.Document.Where(x=>x.ObjectReference==new Guid(request.ObjectReference)).FirstOrDefaultAsync();
                if(document==null){
                    // CREATE DOCUMENT
                    var doc = new Document{
                        Content = Convert.FromBase64String(request.Data),
                        ContentReferences = request.DataReferences,
                        Name = request.Name,
                        Extension = request.Extension,
                        DocumentId = Guid.NewGuid(),
                        CreatedDate = DateTime.UtcNow,
                        ObjectReference = new Guid(request.ObjectReference)
                    };
                    _context.Document.Add(doc);
                }else{
                    // UPDATE DOCUMENT
                    document.Content = Convert.FromBase64String(request.Data);
                    document.Name = request.Name;
                    document.Extension = request.Extension;
                    document.CreatedDate = DateTime.UtcNow;
                }
                var result = await _context.SaveChangesAsync();
                if(result>0) return Unit.Value;
                throw new ManejadorExcepcion(HttpStatusCode.Accepted, new {message="API UPLOAD FILE ERROR"});
            }
        }
    }
}