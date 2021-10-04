using System;
namespace Application.Documents
{
    public class GenericFile
    {
        public Guid? ObjectReferences{get;set;}
        public Guid? DocumentId{get;set;}
        public string Name{get;set;}
        public string Extension{get;set;}
        public string Data{get;set;}
    }
}