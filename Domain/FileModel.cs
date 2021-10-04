using System;
namespace Domain
{
    public class FileModel
    {
        public Guid FileID{get;set;}
        public string fileName{get;set;}
        public string fileSize{get;set;}
        public string fileType{get;set;}
        public DateTime fileCreatedDate{get;set;}
        public DateTime fileUpdatedDate{get;set;}
        public string fullPath{get;set;}
        public string savedDisk{get;set;}
        public string userId{get;set;}
        public User user{get;set;}
    }
}