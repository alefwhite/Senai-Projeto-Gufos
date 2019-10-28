using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{   
    // Definimos nossa rota do controller e dizemos que é um controller de API
    [Route("api/[controller]")]
    [ApiController] // Aqui dizemos que é um controller de Api para funcionar a nossa rota
    public class TipoUsuarioController : ControllerBase
    {   
        // Instaciamos nosso contexto("Banco")
        GufosContext _contexto = new GufosContext();

        // GET: api/tipousuario
        [HttpGet]
        public async Task<ActionResult<List<TipoUsuario>>> Get() 
        {
            var tipousuarios =  await _contexto.TipoUsuario.ToListAsync();

            if(tipousuarios == null) {
                return NotFound();
            }

            return tipousuarios;
        }

        // GET: api/tipousuario/2
        [HttpGet("{id}")] // "{id}/{outro}" caso a rota tenha dois parametros
        public async Task<ActionResult<TipoUsuario>> Get(int id) 
        {   
            // FindAsync = procura algo especifico no banco
            var tipousuario =  await _contexto.TipoUsuario.FindAsync(id);

            if(tipousuario == null) {
                return NotFound();
            }

            return tipousuario;
        }

         // POST: api/tipousuario
        [HttpPost] 
        public async Task<ActionResult<TipoUsuario>> Post(TipoUsuario tipousuario) 
        {   
            try{
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(tipousuario);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

            } catch(DbUpdateConcurrencyException) {
                throw;
            }

            return tipousuario;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, TipoUsuario tipousuario)
        {   

            // Se o Id do objeto não existir ele retorna o erro 404
            if(id != tipousuario.TipoUsuarioId) {
                return BadRequest(); 
            }

            // Comparamos os atributos que foram modificados através do Entity Framework
            // No caso ele so irá dar um SET nas colunas que foram modificadas
            _contexto.Entry(tipousuario).State = EntityState.Modified;   

            try {
                await _contexto.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                
                // Verificamos se o objeto inserido realmente existe no banco
                var tipousuario_valido = await _contexto.TipoUsuario.FindAsync(id);
                
                if(tipousuario_valido == null) {
                    return NotFound();
                } else {
                    throw;
                }
                
            }
            
            // NoContent() - Retorna 204
            return NoContent();
        }

        //DELETE api/tipousuario/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoUsuario>> Delete(int id)
        {   
            // FindAsync = procura algo especifico no banco
            var tipousuario = await _contexto.TipoUsuario.FindAsync(id);
            
            if(tipousuario == null) {
                return NotFound();
            }

            _contexto.TipoUsuario.Remove(tipousuario);

            await _contexto.SaveChangesAsync();

            return tipousuario;
        }
    }
}