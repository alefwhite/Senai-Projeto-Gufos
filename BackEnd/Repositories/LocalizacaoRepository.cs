using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Domains;
using BackEnd.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class LocalizacaoRepository : ILocalizacao
    {
        public async Task<Localizacao> Alterar(Localizacao localizacao)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas
                _contexto.Entry(localizacao).State = EntityState.Modified;

                 await _contexto.SaveChangesAsync();
                 
                return localizacao;                 
            }           
        }

        public async Task<Localizacao> BuscarPorID(int id)
        {
            using(GufosContext _contexto = new GufosContext()){
               return await _contexto.Localizacao.FindAsync(id);
            }
        }

        public async Task<Localizacao> Excluir(Localizacao localizacao)
        {
            using(GufosContext _contexto = new GufosContext()){
                 _contexto.Localizacao.Remove(localizacao);

                await _contexto.SaveChangesAsync();

                return localizacao;
            }
        }

        public async Task<List<Localizacao>> Listar()
        {
            using(GufosContext _contexto = new GufosContext()){
               return await _contexto.Localizacao.ToListAsync();
            }
        }


        public async Task<Localizacao> Salvar(Localizacao localizacao)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(localizacao);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return localizacao;
            }
        }
    }
}