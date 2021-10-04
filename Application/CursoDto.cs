using System.Collections.Generic;
using System;

namespace Application
{
    public class CursoDto
    {
        public Guid CursoId{get;set;}
        public string Titulo{get;set;}
        public string Descripcion{get;set;}
        public DateTime? FechaPublicacion{get;set;}
        public byte[] FotoPortada{get;set;}

        //DTO
        public DateTime? FechaCreacion{get;set;}
    }
}