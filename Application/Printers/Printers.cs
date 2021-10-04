using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Management;
using Domain;

namespace Application.Printer
{
    public class printers
    {
        public class Run : IRequest<List<PrinterModel>>{}

        public class Handler : IRequestHandler<Run, List<PrinterModel>>
        {
            
            public Task<List<PrinterModel>> Handle(Run request, CancellationToken cancellationToken)
            {
                var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
                List<PrinterModel> printers = new List<PrinterModel>();
                foreach (var printerpropieties in printerQuery.Get()){
                    var name = printerpropieties.GetPropertyValue("Name");
                    var UInt32status = printerpropieties.GetPropertyValue("PrinterStatus"); // 3 -> Online, else Offline  
                    var status = "Offline";
                    if(UInt32status.ToString()=="3") status = "Online";
                    printers.Add(new PrinterModel{printerName = name.ToString(), printerStatus = status.ToString()});
                }
                return Task.FromResult<List<PrinterModel>>(
                    printers
                );
            }
        }
    }
}