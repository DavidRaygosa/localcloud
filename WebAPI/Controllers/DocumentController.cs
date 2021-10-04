using System;
using System.Threading.Tasks;
using Application.Documents;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class Document : MyControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> UploadFile(Upload.Run data){
            return await Mediator.Send(data);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericFile>> DownloadFile(Guid id){
            return await Mediator.Send(new Download.Run{Id=id});
        }
    }
}