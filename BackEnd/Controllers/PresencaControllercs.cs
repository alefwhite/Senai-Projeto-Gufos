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
    public class PresencaController : ControllerBase
    {   
        // Instaciamos nosso contexto("Banco")
       PresencaRepository _repositorio = new PresencaRepository();

        // GET: api/presenca
        [HttpGet]
        public async Task<ActionResult<List<Presenca>>> Get() 
        {    
            var presencas =  await _repositorio.Listar();

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
            var presenca =  await _repositorio.BuscarPorID(id);             
           
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
               
               await _repositorio.Salvar(presenca);

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

            try {
                await _repositorio.Alterar(presenca);
            } catch(DbUpdateConcurrencyException) {
                
                // Verificamos se o objeto inserido realmente existe no banco
                var presenca_valido = await _repositorio.BuscarPorID(id);
                
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
            var presenca = await _repositorio.BuscarPorID(id);
            
            if(presenca == null) {
                return NotFound();
            }

            await _repositorio.Excluir(presenca);
            
            return presenca;
        }
    }
}