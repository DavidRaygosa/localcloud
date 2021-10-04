using System.Net;
using System;

namespace Application.ManejadorError
{
    public class ManejadorExcepcion : Exception
    {
        public HttpStatusCode Codigo{get;}
        public object Errores{get;}
        public ManejadorExcepcion(HttpStatusCode codigo, object message=null){
            Codigo=codigo;
            Errores=message;
        }
    }
}