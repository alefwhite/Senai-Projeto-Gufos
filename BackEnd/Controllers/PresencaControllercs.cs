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
    public class PresencaController : ControllerBase
    {   
        // Instaciamos nosso contexto("Banco")
        GufosContext _contexto = new GufosContext();

        // GET: api/presenca
        [HttpGet]
        public async Task<ActionResult<List<Presenca>>> Get() 
        {   
            // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
            // Include("") = Adiciona a arvore 
            var presencas =  await _contexto.Presenca.Include("Evento").Include("Usuario").ToListAsync();

            if(presencas == null) {
                return NotFound();
            }

            return presencas;
        }

        // GET: api/presenca/2
        [HttpGet("{id}")] // "{id}/{outro}" caso a rota tenha dois parametros
        public async Task<ActionResult<Presenca>> Get(int id) 
        {   
            // FindAsync = procura algo especifico no banco
            var presenca =  await _contexto.Presenca.Include("Evento").Include("Usuario").FirstOrDefaultAsync(p => p.PresencaId == id);             

             // Forma mais complicada
            // var presenca =  await _contexto.Presenca.Include(c => c.Categoria).Include(l => l.Localizacao).FirstOrDefaultAsync(e => e.PresencaId == id);
           
            if(presenca == null) {
                return NotFound();
            }

            return presenca;
        }

         // POST: api/presenca
        [HttpPost] 
        public async Task<ActionResult<Presenca>> Post(Presenca presenca) 
        {   
            try{
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(presenca);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

            } catch(DbUpdateConcurrencyException) {
                throw;
            }

            return presenca;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Presenca presenca)
        {   

            // Se o Id do objeto não existir ele retorna o erro 404
            if(id != presenca.PresencaId) {
                return BadRequest(); 
            }

            // Comparamos os atributos que foram modificados através do Entity Framework
            // No caso ele so irá dar um SET nas colunas que foram modificadas
            _contexto.Entry(presenca).State = EntityState.Modified;   

            try {
                await _contexto.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                
                // Verificamos se o objeto inserido realmente existe no banco
                var presenca_valido = await _contexto.Presenca.FindAsync(id);
                
                if(presenca_valido == null) {
                    return NotFound();
                } else {
                    throw;
                }
                
            }
            
            // NoContent() - Retorna 204
            return NoContent();
        }

        //DELETE api/presenca/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Presenca>> Delete(int id)
        {   
            // FindAsync = procura algo especifico no banco
            var presenca = await _contexto.Presenca.FindAsync(id);
            
            if(presenca == null) {
                return NotFound();
            }

            _contexto.Presenca.Remove(presenca);

            await _contexto.SaveChangesAsync();

            return presenca;
        }
    }
}