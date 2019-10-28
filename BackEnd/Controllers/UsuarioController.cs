using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Domains;
using BackEnd.Repositories;
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
         UsuarioRepository _repositorio = new UsuarioRepository();

        // GET: api/usuario        
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get() 
        {   
            // Include e como se fosse um join, após instalarmos a biblioteca do JSON incluimos os Includes
            // Include("") = Adiciona a arvore 
            var usuarios =  await _repositorio.Listar();

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
            var usuario =  await _repositorio.BuscarPorID(id);
                         
           
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
               
               await _repositorio.Salvar(usuario);

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


            try {

              await _repositorio.Alterar(usuario);

            } catch(DbUpdateConcurrencyException) {
                
                // Verificamos se o objeto inserido realmente existe no banco
                var usuario_valido = await _repositorio.BuscarPorID(id);
                
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
            var usuario = await _repositorio.BuscarPorID(id);
            
            if(usuario == null) {
                return NotFound();
            }

            await _repositorio.Excluir(usuario);

            return usuario;
        }
    }
}