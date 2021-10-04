using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Printing;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Neodynamic.SDK.Web;

namespace Application.Printers
{
    public class Print
    {
        public class Run : IRequest{
            public string printer{get;set;}
            public string filePath{get;set;}
        }

        public class Handler : IRequestHandler<Run>
        {
            public Task<Unit> Handle(Run request, CancellationToken cancellationToken)
            {
                Process process = new Process();
                process.StartInfo.FileName = "2Printer\\2Printer.exe";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.Arguments = string.Format("-options alerts:no -src \""+request.filePath+"\" -prn \""+request.printer+"\"");
                process.Start();
                System.Threading.Thread.Sleep(3000);
                process.Kill();
                process.Kill(true);
                process.Close();
                return Task.FromResult<Unit>(
                    Unit.Value
                );
            }     
        }     
    }
}