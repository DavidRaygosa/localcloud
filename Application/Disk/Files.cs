using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.ManejadorError;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Disk
{
    public class Files
    {
        public class Run : IRequest<PathList>{
            public string root{get;set;}
            public string path{get;set;}
        }

        public class Handler : IRequestHandler<Run, PathList>
        {
            private List<folderDTO> folders = new List<folderDTO>();
            private List<FileModelDTO> File = new List<FileModelDTO>();
            private readonly DataBaseContext _context;
            public Handler(DataBaseContext context){
                _context = context;
            }
            public async Task<PathList> Handle(Run request, CancellationToken cancellationToken)
            {
                // CREATE ROOT FOLDER
                if(request.path==""){
                    if(!Directory.Exists(request.root+"\\Cloud")){
                        var result = Directory.CreateDirectory(request.root+"\\Cloud");
                    }
                }
                // MIX ROOT WITH PATH
                request.path = request.path.Replace("$path$", "\\");
                var root = request.root+"\\Cloud"+request.path;
                string[] files = Directory.GetFiles(@""+root);
                // GET PATH FOLDERS
                getFolders(root);
                // GET PATH FILES
                foreach (string file in files){
                    var DBFile = await _context.File.Where(x=>x.fullPath==file).FirstAsync();
                    if(DBFile==null) throw new ManejadorExcepcion(HttpStatusCode.Accepted, new{message="File not found"});
                    File.Add(new FileModelDTO{
                        FileID=DBFile.FileID,
                        fileName=DBFile.fileName,
                        fileSize=DBFile.fileSize,
                        fileType=DBFile.fileType,
                        fileCreatedDate=DBFile.fileCreatedDate,
                        fileUpdatedDate=DBFile.fileUpdatedDate,
                        fullPath=DBFile.fullPath,
                        savedDisk=DBFile.savedDisk,
                        fullName=DBFile.userId
                    });
                }
                PathList pathlist = new PathList{
                    Files = File,
                    Folders = folders
                };
                return pathlist;
            }

            public Boolean getFolders(string filePath){
                var foldersList = Directory.GetDirectories(filePath, "*", SearchOption.TopDirectoryOnly);
                foreach(var folderPath in foldersList){
                    var folderName = "";
                    var folderSplit = folderPath.Split(new[] {"\\"}, StringSplitOptions.None);
                    for(int z=0;z<=folderSplit.Length;z++){
                        if(z==folderSplit.Length) folderName = folderSplit[z-1];
                    }
                    var folder = new folderDTO{
                        FolderName = folderName,
                        PathFolder = folderPath
                    };
                    folders.Add(folder);
                }
                return true;
            }

            static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            static string SizeSuffix(Int64 value, int decimalPlaces = 1){
                if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
                if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); } 
                if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }
                // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
                int mag = (int)Math.Log(value, 1024);
                // 1L << (mag * 10) == 2 ^ (10 * mag) 
                // [i.e. the number of bytes in the unit corresponding to mag]
                decimal adjustedSize = (decimal)value / (1L << (mag * 10));
                // make adjustment when the value is large enough that
                // it would round up to 1000 or more
                if (Math.Round(adjustedSize, decimalPlaces) >= 1000){
                    mag += 1;
                    adjustedSize /= 1024;
                }
                return string.Format("{0:n" + decimalPlaces + "} {1}", 
                    adjustedSize, 
                    SizeSuffixes[mag]);
            }
        }
    }
}