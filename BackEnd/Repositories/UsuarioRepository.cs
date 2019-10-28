using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Domains;
using BackEnd.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class UsuarioRepository : IUsuario
    {
        public async Task<Usuario> Alterar(Usuario usuario)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas
                _contexto.Entry(usuario).State = EntityState.Modified;

                 await _contexto.SaveChangesAsync();
                 
                return usuario;                 
            }           
        }

        public async Task<Usuario> BuscarPorID(int id)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
                // Include("") = Adiciona a arvore 
               return await _contexto.Usuario.Include("TipoUsuario").Include("Presenca").FirstOrDefaultAsync(e => e.UsuarioId == id);
            }
        }

        public async Task<Usuario> Excluir(Usuario usuario)
        {
            using(GufosContext _contexto = new GufosContext()){
                 _contexto.Usuario.Remove(usuario);

                await _contexto.SaveChangesAsync();

                return usuario;
            }
        }

        public async Task<List<Usuario>> Listar()
        {
            using(GufosContext _contexto = new GufosContext()){
               // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
               // Include("") = Adiciona a arvore 
               return await _contexto.Usuario.Include("TipoUsuario").Include("Presenca").ToListAsync();
              
            }
        }


        public async Task<Usuario> Salvar(Usuario usuario)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(usuario);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return usuario;
            }
        }

    }
}