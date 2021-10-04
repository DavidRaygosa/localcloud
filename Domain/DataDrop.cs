using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Domain
{
    public class DataDrop
    {
        public string paths{get;set;}
        public List<IFormFile> files{get;set;}
    }
}