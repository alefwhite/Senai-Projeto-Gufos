using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Domains;
using BackEnd.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class EventoRepository : IEvento
    {
        public async Task<Evento> Alterar(Evento evento)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas
                _contexto.Entry(evento).State = EntityState.Modified;

                 await _contexto.SaveChangesAsync();
                 
                return evento;                 
            }           
        }

        public async Task<Evento> BuscarPorID(int id)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
                // Include("") = Adiciona a arvore 
               return await _contexto.Evento.Include("Categoria").Include("Localizacao").FirstOrDefaultAsync(e => e.EventoId == id);
            }
        }

        public async Task<Evento> Excluir(Evento evento)
        {
            using(GufosContext _contexto = new GufosContext()){
                 _contexto.Evento.Remove(evento);

                await _contexto.SaveChangesAsync();

                return evento;
            }
        }

        public async Task<List<Evento>> Listar()
        {
            using(GufosContext _contexto = new GufosContext()){
               // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
               // Include("") = Adiciona a arvore 
               return await _contexto.Evento.Include("Categoria").Include("Localizacao").ToListAsync();
              
            }
        }


        public async Task<Evento> Salvar(Evento evento)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(evento);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return evento;
            }
        }

        
    }
}