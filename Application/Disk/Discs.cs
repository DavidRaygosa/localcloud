using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;

namespace Application.Disk
{
    public class discs
    {
        public class Run : IRequest<List<DiscsModel>>{
            
        }

        public class Handler : IRequestHandler<Run, List<DiscsModel>>
        {
            public Task<List<DiscsModel>> Handle(Run request, CancellationToken cancellationToken)
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                List<DiscsModel> discs = new List<DiscsModel>();
                foreach (DriveInfo d in allDrives)
                {
                    double FreeSpace = d.TotalFreeSpace / 1024 / 1024 / 1024;
                    double Size = d.TotalSize / 1024 / 1024 / 1024;
                    discs.Add(new DiscsModel{DriverName = d.Name, VolumeLabel = d.VolumeLabel, FreeSpace = FreeSpace, TotalSize = Size});
                }
                return Task.FromResult<List<DiscsModel>>(
                    discs
                );
            }
        }
    }
}