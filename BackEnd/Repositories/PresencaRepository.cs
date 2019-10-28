using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Domains;
using BackEnd.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class PresencaRepository : IPresenca
    {

        public async Task<Presenca> Alterar(Presenca presenca)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas
                _contexto.Entry(presenca).State = EntityState.Modified;

                 await _contexto.SaveChangesAsync();
                 
                return presenca;                 
            }           
        }

        public async Task<Presenca> BuscarPorID(int id)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
                // Include("") = Adiciona a arvore 
               return await _contexto.Presenca.Include("Presenca").Include("Usuario").FirstOrDefaultAsync(e => e.PresencaId == id);
            }
        }

        public async Task<Presenca> Excluir(Presenca presenca)
        {
            using(GufosContext _contexto = new GufosContext()){
                 _contexto.Presenca.Remove(presenca);

                await _contexto.SaveChangesAsync();

                return presenca;
            }
        }

        public async Task<List<Presenca>> Listar()
        {
            using(GufosContext _contexto = new GufosContext()){
               // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
               // Include("") = Adiciona a arvore 
               return await _contexto.Presenca.Include("Presenca").Include("Usuario").ToListAsync();
              
            }
        }


        public async Task<Presenca> Salvar(Presenca presenca)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(presenca);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return presenca;
            }
        }
        
    }
}