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
    public class TipoUsuarioController : ControllerBase
    {   
        // Instaciamos nosso contexto("Banco")
       TipoUsuarioRepository _repositorio = new TipoUsuarioRepository();

        // GET: api/tipousuario
        [HttpGet]
        public async Task<ActionResult<List<TipoUsuario>>> Get() 
        {
            var tipousuarios =  await _repositorio.Listar();

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
            var tipousuario =  await _repositorio.BuscarPorID(id);

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
               
                await _repositorio.Salvar(tipousuario);

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

          
            try {
               
                await _repositorio.Alterar(tipousuario);

            } catch(DbUpdateConcurrencyException) {
                
                // Verificamos se o objeto inserido realmente existe no banco
                var tipousuario_valido = await _repositorio.BuscarPorID(id);
                
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
            var tipousuario = await _repositorio.BuscarPorID(id);
            
            if(tipousuario == null) {
                return NotFound();
            }

            await _repositorio.Excluir(tipousuario);

            return tipousuario;
        }
    }
}