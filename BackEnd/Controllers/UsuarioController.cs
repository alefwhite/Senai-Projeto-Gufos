using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{   
    // Definimos nossa rota do controller e dizemos que é um controller de API
    [Route("api/[controller]")]
    [ApiController] // Aqui dizemos que é um controller de Api para funcionar a nossa rota
    [Authorize]
    public class UsuarioController : ControllerBase
    {   
        // Instaciamos nosso contexto("Banco")
        GufosContext _contexto = new GufosContext();

        // GET: api/usuario        
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get() 
        {   
            // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
            // Include("") = Adiciona a arvore 
            var usuarios =  await _contexto.Usuario.Include("TipoUsuario").Include("Presenca").ToListAsync();

            if(usuarios == null) {
                return NotFound();
            }

            return usuarios;
        }

        // GET: api/usuario/2
        [HttpGet("{id}")] // "{id}/{outro}" caso a rota tenha dois parametros
        public async Task<ActionResult<Usuario>> Get(int id) 
        {   
            // FindAsync = procura algo especifico no banco
            var usuario =  await _contexto.Usuario.Include("TipoUsuario").Include("Presenca").FirstOrDefaultAsync(u => u.UsuarioId == id);             

             // Forma mais complicada
            // var usuario =  await _contexto.Usuario.Include(c => c.Categoria).Include(l => l.Localizacao).FirstOrDefaultAsync(e => e.UsuarioId == id);
           
            if(usuario == null) {
                return NotFound();
            }

            return usuario;
        }

         // POST: api/usuario
        [HttpPost] 
        public async Task<ActionResult<Usuario>> Post(Usuario usuario) 
        {   
            try{
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(usuario);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

            } catch(DbUpdateConcurrencyException) {
                throw;
            }

            return usuario;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Usuario usuario)
        {   

            // Se o Id do objeto não existir ele retorna o erro 404
            if(id != usuario.UsuarioId) {
                return BadRequest(); 
            }

            // Comparamos os atributos que foram modificados através do Entity Framework
            // No caso ele so irá dar um SET nas colunas que foram modificadas
            _contexto.Entry(usuario).State = EntityState.Modified;   

            try {
                await _contexto.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                
                // Verificamos se o objeto inserido realmente existe no banco
                var usuario_valido = await _contexto.Usuario.FindAsync(id);
                
                if(usuario_valido == null) {
                    return NotFound();
                } else {
                    throw;
                }
                
            }
            
            // NoContent() - Retorna 204
            return NoContent();
        }

        //DELETE api/usuario/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> Delete(int id)
        {   
            // FindAsync = procura algo especifico no banco
            var usuario = await _contexto.Usuario.FindAsync(id);
            
            if(usuario == null) {
                return NotFound();
            }

            _contexto.Usuario.Remove(usuario);

            await _contexto.SaveChangesAsync();

            return usuario;
        }
    }
}