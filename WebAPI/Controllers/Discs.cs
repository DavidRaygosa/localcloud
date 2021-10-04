using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Disk;
using Application.ManejadorError;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Newtonsoft.Json;
using System.IO.Compression;

namespace WebAPI.Controllers
{
    public class Discs : MyControllerBase
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly DataBaseContext _context;
        private readonly UserManager<Domain.User> _userManager;
        private readonly IUserSession _userSesion;

        public Discs(IWebHostEnvironment hostingEnvironment, UserManager<Domain.User> userManager, IUserSession userSesion, DataBaseContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _userManager = userManager;
            _userSesion = userSesion;
        }

        // GET ALL DISK
        [HttpGet]
        public Task<List<DiscsModel>> discs(){
            return Mediator.Send(new discs.Run());
        }

        // GET ALL FILE BY PATH
        [HttpPost]
        public Task<PathList> getFiles(Files.Run data){
            return Mediator.Send(data);
        }

        // UPDATE A FILE
        [HttpPut]
        public Task<ActionResult<Unit>> updateFile(UpdateFile.Run data){
            return Mediator.Send(data);
        }

        // DELETE A FILE
        [HttpDelete("{ID}")]
        public async Task<ActionResult<Unit>> delete(string ID){
            return await Mediator.Send(new DeleteFile.Run{ID=new Guid(ID)});
        }

        // CREATE A FOLDER
        [HttpPost("folder")]
        public Task<Unit> createFolder(CreateFolder.Run data){
            return Mediator.Send(data);
        }

        // UPDATE A FOLDER
        [HttpPost("updateFolder")]
        public async Task<Unit> updateFolder(updateFolder.Run data){
            return await Mediator.Send(data);
        }
        
        [HttpPost("deleteDirectory")]
        public async Task<Unit> deleteDir(DeleteDir.Run data){
            return await Mediator.Send(data);
        }

        // CREATE TEMPFILES
        [HttpPost("tempFiles/{root}/{path}")]
        [RequestFormLimits(MultipartBodyLengthLimit = 1000000000000)] 
        [DisableRequestSizeLimit]
        [Consumes("multipart/form-data")]
        public async Task<List<string>> uploadTemp(List<IFormFile> files, string root, string path){
            var userSession = await _userManager.FindByNameAsync(_userSesion.GetUserSession());
            path = path.Replace("$path$", "\\");
            var filePath = "";
            DateTime FileCreatedDate;
            List<string> Id = new List<string>();
            if(path=="null") filePath = root+"\\Cloud";
            else{
                path = path.Replace("$path$", "\\");
                filePath = root+"\\Cloud"+path;
            }
            foreach (var formFile in files){
                if (formFile.Length > 0){
                    string fullPath = filePath+"\\"+formFile.FileName;
                    // COPY FILE INTO CLOUD FOLDER
                    using (var stream = new FileStream(fullPath, FileMode.Create)){
                        await formFile.CopyToAsync(stream);
                        // ADD FILE DATA TO DB
                        var info = new System.IO.FileInfo(fullPath);
                        FileCreatedDate = DateTime.UtcNow;
                        var fileM = new FileModel{
                            FileID = Guid.NewGuid(),
                            fileName = Path.GetFileNameWithoutExtension(fullPath),
                            fileSize = SizeSuffix(info.Length).ToString(),
                            fileType = "TXT",
                            fileCreatedDate = DateTime.Now,
                            fileUpdatedDate = DateTime.Now,
                            fullPath = fullPath,
                            savedDisk = root,
                            userId = userSession.Id
                        };
                        Id.Add(fileM.FileID.ToString());
                        _context.File.Add(fileM);
                    }
                }
            }
            var resultados = await _context.SaveChangesAsync();
            if(resultados==0) throw new Exception("DB Error - Save File");
            return Id;
        }

        // DOWNLOAD A FILE
        [HttpGet("download/{ID}")]
        public FileStreamResult Get(string ID){
            var file = _context.File.Where(x=>x.FileID==new Guid(ID)).First();
            FileStream fileS = new FileStream(file.fullPath, FileMode.Open, FileAccess.Read);
            return File(fileS, "application/octet-stream", file.fileName+"."+file.fileType.ToLower());
        }

        // DOWNLOAD AS ZIP
        [HttpPost("createZip")]
        public async Task<List<string>> createZip([FromBody]DownloadZipModel data){
            var userSession = await _userManager.FindByNameAsync(_userSesion.GetUserSession());
            data.Path = data.Path.Replace("$path$", "\\");
            string[] extractPath = data.Path.Split(data.folderName);
            string zipPath = extractPath[0]+data.folderName+".zip";
            List<string> Id = new List<string>();
            ZipFile.CreateFromDirectory(data.Path, zipPath, CompressionLevel.NoCompression, false);
            var info = new System.IO.DirectoryInfo(data.Path);
            var fileM = new FileModel{
                FileID = Guid.NewGuid(),
                fileName = data.folderName,
                fileSize = SizeSuffix(DirSize(info)).ToString(),
                fileType = "ZIP",
                fileCreatedDate = DateTime.Now,
                fileUpdatedDate = DateTime.Now,
                fullPath = zipPath,
                savedDisk = data.root,
                userId = userSession.Id
            };
            _context.File.Add(fileM);
            Id.Add(fileM.FileID.ToString());
            var result = await _context.SaveChangesAsync();
            if(result==0) throw new Exception("DB Error - Save File");
            return Id;
        }

        [HttpGet("downloadZip/{ID}")]
        public FileStreamResult GetZip(string ID){
            var file = _context.File.Where(x=>x.FileID==new Guid(ID)).First();
            FileStream fileS = new FileStream(file.fullPath, FileMode.Open, FileAccess.Read);
            return File(fileS, "application/zip", file.fileName+".zip");
        }

        public static long DirSize(DirectoryInfo d) 
        {    
            long size = 0;    
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis) 
            {      
                size += fi.Length;    
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis) 
            {
                size += DirSize(di);   
            }
            return size;  
        }

        // UPLOAD Drop
        [HttpPost("uploadFilesDrop/{root}/{path}/{index}")]
        [RequestFormLimits(MultipartBodyLengthLimit = 1000000000000)] 
        [DisableRequestSizeLimit] 
        [Consumes("multipart/form-data")]
        public async Task<int> uploadDrop(IList<IFormFile> files, string root, string path, int index){
            var userSession = await _userManager.FindByNameAsync(_userSesion.GetUserSession());
            var filePath = "";
            if(path=="null") filePath = root+"\\Cloud";
            else{
                path = path.Replace("$path$", "\\");
                filePath = root+"\\Cloud"+path;
            }
            var readPathTemp = await System.IO.File.ReadAllTextAsync(filePath+"\\"+"$Temp$Paths.txt");
            var readLastTemp = await System.IO.File.ReadAllTextAsync(filePath+"\\"+"$Temp$LastModified.txt");
            var lastmodifiedList = JsonConvert.DeserializeObject<List<string>>(readLastTemp);
            var pathList = JsonConvert.DeserializeObject<List<string>>(readPathTemp);
            foreach(var file in files){
                string pathfile = pathList[index].Replace("$path$", "\\");
                string fullPath = root+"\\Cloud"+pathfile;
                string newFullPath = filNameInFolder(fullPath);
                string FileType = "";
                DateTime FileCreatedDate;
                // COPY FILE INTO CLOUD FOLDER
                using (var stream = new FileStream(newFullPath, FileMode.Create)){
                    await file.CopyToAsync(stream);
                    string[] subSplit = Path.GetExtension(newFullPath).Split(".");
                    for(var i = 0; i<subSplit.Length; i++){
                        if(i==1) FileType = subSplit[i];
                    }
                    // ADD FILE DATA TO DB
                    var info = new System.IO.FileInfo(newFullPath);
                    FileCreatedDate = DateTime.UtcNow;
                    FileModel fileM = new FileModel{
                        FileID = Guid.NewGuid(),
                        fileName = Path.GetFileNameWithoutExtension(newFullPath),
                        fileSize = SizeSuffix(info.Length).ToString(),
                        fileType = FileType.ToUpper(),
                        fileCreatedDate = DateTime.Parse(lastmodifiedList[index]),
                        fileUpdatedDate = DateTime.Now,
                        fullPath = newFullPath,
                        savedDisk = root,
                        userId = userSession.Id
                    };
                    await _context.File.AddAsync(fileM);
                }index++;
            }
            var resultados = await _context.SaveChangesAsync();
            if(resultados==0) throw new Exception("DB Error - Save File");
            return index;
        }

        // UPLOAD FILES
        [HttpPost("uploadFiles/{root}/{path}/{date}")]
        [RequestFormLimits(MultipartBodyLengthLimit = 1000000000000)] 
        [DisableRequestSizeLimit] 
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> uploadFiles(List<IFormFile> files, string root, string path, string date){
            path = path.Replace("$path$", "\\");
            // GET CURRENT SESION
            var userSession = await _userManager.FindByNameAsync(_userSesion.GetUserSession());
            // STRING ARRAY TO JSON (DATES)
            var lastmodifiedList = JsonConvert.DeserializeObject<List<string>>(date);
            // MIX PATH WITH DISK
            var filePath = "";
            if(path=="null") filePath = root+"\\Cloud";
            else filePath = root+"\\Cloud"+path;
            if(!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            if (files == null || files.Count == 0) return Content("file not selected");
            long size = files.Sum(f => f.Length);
            var filePaths = new List<string>();
            string FileType = "";
            DateTime FileCreatedDate;
            int index = 0;
            foreach (var formFile in files){
                if (formFile.Length > 0){
                // full path to file in temp location
                    filePaths.Add(filePath); 
                    var fullPath = filePath+"\\"+formFile.FileName;
                    // IF FILE EXISTS, ADD A NUMBER AFTER NAME
                    var newFullPath = filNameInFolder(fullPath);
                    // COPY FILE INTO CLOUD FOLDER
                    using (var stream = new FileStream(newFullPath, FileMode.Create)){
                        await formFile.CopyToAsync(stream);
                        string[] subSplit = Path.GetExtension(newFullPath).Split(".");
                        for(var i = 0; i<subSplit.Length; i++){
                            if(i==1) FileType = subSplit[i];
                        }
                        // ADD FILE DATA TO DB
                        var info = new System.IO.FileInfo(newFullPath);
                        FileCreatedDate = DateTime.UtcNow;
                        var file = new FileModel{
                            FileID = Guid.NewGuid(),
                            fileName = Path.GetFileNameWithoutExtension(newFullPath),
                            fileSize = SizeSuffix(info.Length).ToString(),
                            fileType = FileType.ToUpper(),
                            fileCreatedDate = DateTime.Parse(lastmodifiedList[index]),
                            fileUpdatedDate = DateTime.Now,
                            fullPath = newFullPath,
                            savedDisk = root,
                            userId = userSession.Id
                        };
                        _context.File.Add(file);
                        var resultados = await _context.SaveChangesAsync();
                        if(resultados==0) throw new Exception("DB Error - Save File");
                    }
                }index++;
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, size, filePaths });
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
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

        static string filNameInFolder(string fullPath){
            int count = 1;
            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string path = Path.GetDirectoryName(fullPath);
            string newFullPath = fullPath;
            while(System.IO.File.Exists(newFullPath)){
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }
            return newFullPath;
        }
    }
}