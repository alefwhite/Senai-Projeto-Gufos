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
    public class EventoController : ControllerBase
    {   
        // Instaciamos nosso contexto("Banco")
        //GufosContext _contexto = new GufosContext();
        EventoRepository _repositorio = new EventoRepository();

        // GET: api/evento
        /// <summary>
        /// Pegamos todos os eventos cadastrados
        /// </summary>
        /// <returns>Lista de eventos</returns>
        [HttpGet]
        public async Task<ActionResult<List<Evento>>> Get() 
        {               
            var eventos = await _repositorio.Listar();

            if(eventos == null) {
                return NotFound();
            }

            return eventos;
        }

        // GET: api/evento/2
        [HttpGet("{id}")] // "{id}/{outro}" caso a rota tenha dois parametros
        public async Task<ActionResult<Evento>> Get(int id) 
        {             
            var evento = await _repositorio.BuscarPorID(id);            
           
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
              await _repositorio.Salvar(evento);

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

            try {                
                await _repositorio.Alterar(evento);
            } catch(DbUpdateConcurrencyException) {
                
                // Verificamos se o objeto inserido realmente existe no banco
                var evento_valido = await _repositorio.BuscarPorID(id);
                
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
            var evento = await _repositorio.BuscarPorID(id);
            
            if(evento == null) {
                return NotFound();
            }


            await _repositorio.Excluir(evento);

            return evento;
        }
    }
}