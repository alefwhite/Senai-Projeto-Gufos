using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Domains;
using BackEnd.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class CategoriaRepository : ICategoria
    {
        public async Task<Categoria> Alterar(Categoria categoria)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Comparamos os atributos que foram modificados através do Entity Framework
                // No caso ele so irá dar um SET nas colunas que foram modificadas
                _contexto.Entry(categoria).State = EntityState.Modified;

                 await _contexto.SaveChangesAsync();
                 
                return categoria;                 
            }           
        }

        public async Task<Categoria> BuscarPorID(int id)
        {
            using(GufosContext _contexto = new GufosContext()){
               return await _contexto.Categoria.FindAsync(id);
            }
        }

        public async Task<Categoria> Excluir(Categoria categoria)
        {
            using(GufosContext _contexto = new GufosContext()){
                 _contexto.Categoria.Remove(categoria);

                await _contexto.SaveChangesAsync();

                return categoria;
            }
        }

        public async Task<List<Categoria>> Listar()
        {
            using(GufosContext _contexto = new GufosContext()){
               return await _contexto.Categoria.ToListAsync();
            }
        }


        public async Task<Categoria> Salvar(Categoria categoria)
        {
            using(GufosContext _contexto = new GufosContext()){
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(categoria);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

                return categoria;
            }
        }
    }
}