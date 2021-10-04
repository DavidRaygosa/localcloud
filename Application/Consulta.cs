using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<FileModel>>{}

        public class Manejador : IRequestHandler<ListaCursos, List<FileModel>>
        {
            private readonly DataBaseContext _context;
            private readonly IMapper _mapper;
            public Manejador(DataBaseContext context, IMapper mapper){
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<FileModel>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                // DTO
                var cursos = await _context.File.Include(x=>x.user).ToListAsync();
                var cursosDto = _mapper.Map<List<FileModel>>(cursos);
                return cursosDto;
            }
        }
    }
}