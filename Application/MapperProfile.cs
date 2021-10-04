using System.Linq;
using AutoMapper;
using Domain;

namespace Application
{
    public class MapperProfile : Profile
    {
        public MapperProfile(){
            CreateMap<FileModel, FileModelDTO>();
        }
    }
}