using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;
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
        GufosContext _contexto = new GufosContext();

        // GET: api/categoria
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get() 
        {
            var categorias =  await _contexto.Categoria.ToListAsync();

            if(categorias == null) {
                return NotFound();
            }

            return categorias;
        }

        // GET: api/categoria/2
        [HttpGet("{id}")] // "{id}/{outro}" caso a rota tenha dois parametros
        public async Task<ActionResult<Categoria>> Get(int id) 
        {   
            // FindAsync = procura algo especifico no banco
            var categoria =  await _contexto.Categoria.FindAsync(id);

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
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(categoria);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

            } catch(DbUpdateConcurrencyException) {
                throw;
            }

            return categoria;
        }
    }
}