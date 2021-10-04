using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Printer;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class Printer : MyControllerBase
    {
        [HttpGet]
        public Task<List<PrinterModel>> printers(){
            return Mediator.Send(new printers.Run());
        }

        [HttpPost]
        public Task<Unit> print(Application.Printers.Print.Run parametros){
            return Mediator.Send(parametros);
        }
    }
}