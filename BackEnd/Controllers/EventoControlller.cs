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
    public class EventoController : ControllerBase
    {   
        // Instaciamos nosso contexto("Banco")
        GufosContext _contexto = new GufosContext();

        // GET: api/evento
        [HttpGet]
        public async Task<ActionResult<List<Evento>>> Get() 
        {   
            // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
            // Include("") = Adiciona a arvore 
            var eventos =  await _contexto.Evento.Include("Categoria").Include("Localizacao").ToListAsync();

            if(eventos == null) {
                return NotFound();
            }

            return eventos;
        }

        // GET: api/evento/2
        [HttpGet("{id}")] // "{id}/{outro}" caso a rota tenha dois parametros
        public async Task<ActionResult<Evento>> Get(int id) 
        {   
            // FindAsync = procura algo especifico no banco
            var evento =  await _contexto.Evento.Include("Categoria").Include("Localizacao").FirstOrDefaultAsync(e => e.EventoId == id);             

             // Forma mais complicada
            // var evento =  await _contexto.Evento.Include(c => c.Categoria).Include(l => l.Localizacao).FirstOrDefaultAsync(e => e.EventoId == id);
           
            if(evento == null) {
                return NotFound();
            }

            return evento;
        }

         // POST: api/evento
        [HttpPost] 
        public async Task<ActionResult<Evento>> Post(Evento evento) 
        {   
            try{
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(evento);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

            } catch(DbUpdateConcurrencyException) {
                throw;
            }

            return evento;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Evento evento)
        {   

            // Se o Id do objeto não existir ele retorna o erro 404
            if(id != evento.EventoId) {
                return BadRequest(); 
            }

            // Comparamos os atributos que foram modificados através do Entity Framework
            // No caso ele so irá dar um SET nas colunas que foram modificadas
            _contexto.Entry(evento).State = EntityState.Modified;   

            try {
                await _contexto.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                
                // Verificamos se o objeto inserido realmente existe no banco
                var evento_valido = await _contexto.Evento.FindAsync(id);
                
                if(evento_valido == null) {
                    return NotFound();
                } else {
                    throw;
                }
                
            }
            
            // NoContent() - Retorna 204
            return NoContent();
        }

        //DELETE api/evento/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Evento>> Delete(int id)
        {   
            // FindAsync = procura algo especifico no banco
            var evento = await _contexto.Evento.FindAsync(id);
            
            if(evento == null) {
                return NotFound();
            }

            _contexto.Evento.Remove(evento);

            await _contexto.SaveChangesAsync();

            return evento;
        }
    }
}