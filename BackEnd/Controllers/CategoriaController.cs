using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Domains;
using BackEnd.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{   
    // Definimos nossa rota do controller e dizemos que é um controller de API
    [Route("api/[controller]")]
    [ApiController] // Aqui dizemos que é um controller de Api para funcionar a nossa rota
    public class CategoriaController : ControllerBase
    {   
        // Instaciamos nosso contexto("Banco")
        //GufosContext _contexto = new GufosContext();
        CategoriaRepository _repositorio = new CategoriaRepository();

        // GET: api/categoria
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get() 
        {
            var categorias = await _repositorio.Listar();

            if(categorias == null) {
                return NotFound();
            }

            return categorias;
        }

        // GET: api/categoria/2
        [HttpGet("{id}")] // "{id}/{outro}" caso a rota tenha dois parametros
        public async Task<ActionResult<Categoria>> Get(int id) 
        {   
            var categoria =  await _repositorio.BuscarPorID(id);

            if(categoria == null) {
                return NotFound();
            }

            return categoria;
        }

         // POST: api/categoria
        [HttpPost] 
        public async Task<ActionResult<Categoria>> Post(Categoria categoria) 
        {   
            try{
                // // Tratamos contra ataques de SQL Injection
                // await _contexto.AddAsync(categoria);

                // // Salvamos efetivamente o nosso objeto no banco de dados
                // await _contexto.SaveChangesAsync();

                await _repositorio.Salvar(categoria);

            } catch(DbUpdateConcurrencyException) {
                throw;
            }

            return categoria;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Categoria categoria)
        {   

            // Se o Id do objeto não existir ele retorna o erro 404
            if(id != categoria.CategoriaId) {
                return BadRequest(); 
            }

            // Comparamos os atributos que foram modificados através do Entity Framework
            // No caso ele so irá dar um SET nas colunas que foram modificadas
            // _contexto.Entry(categoria).State = EntityState.Modified;   

            try {
                // await _contexto.SaveChangesAsync();
                await _repositorio.Alterar(categoria);

            } catch(DbUpdateConcurrencyException) {
                
                // Verificamos se o objeto inserido realmente existe no banco
                var categoria_valido = await _repositorio.BuscarPorID(id);
                
                if(categoria_valido == null) {
                    return NotFound();
                } else {
                    throw;
                }
                
            }
            
            // NoContent() - Retorna 204
            return NoContent();
        }

        //DELETE api/categoria/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> Delete(int id)
        {   
            // FindAsync = procura algo especifico no banco
            // var categoria = await _contexto.Categoria.FindAsync(id);

            var categoria = await _repositorio.BuscarPorID(id);
            
            if(categoria == null) {
                return NotFound();
            }

            // _contexto.Categoria.Remove(categoria);
            // await _contexto.SaveChangesAsync();

            await _repositorio.Excluir(categoria);

            return categoria;
        }
    }
}