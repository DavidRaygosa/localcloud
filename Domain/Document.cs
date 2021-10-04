using System;
namespace Domain
{
    public class Document
    {
        public Guid DocumentId{get;set;}
        public Guid ObjectReference{get;set;}
        public string Name{get;set;}
        public string Extension{get;set;}
        public byte[] Content{get;set;}
        public string ContentReferences{get;set;}
        public DateTime CreatedDate{get;set;}
    }
}