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
    public class LocalizacaoController : ControllerBase
    {   
        // Instaciamos nosso contexto("Banco")
        
        LocalizacaoRepository _repositorio = new LocalizacaoRepository();

        // GET: api/localizacao
        [HttpGet]
        public async Task<ActionResult<List<Localizacao>>> Get() 
        {
            var localizacoes =  await _repositorio.Listar();

            if(localizacoes == null) {
                return NotFound();
            }

            return localizacoes;
        }

        // GET: api/localizacao/2
        [HttpGet("{id}")] // "{id}/{outro}" caso a rota tenha dois parametros
        public async Task<ActionResult<Localizacao>> Get(int id) 
        {   
            var localizacao =  await _repositorio.BuscarPorID(id);

            if(localizacao == null) {
                return NotFound();
            }

            return localizacao;
        }

         // POST: api/localizacao
        [HttpPost] 
        public async Task<ActionResult<Localizacao>> Post(Localizacao localizacao) 
        {   
            try{
               await _repositorio.Salvar(localizacao);

            } catch(DbUpdateConcurrencyException) {
                throw;
            }

            return localizacao;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Localizacao localizacao)
        {   

            // Se o Id do objeto não existir ele retorna o erro 404
            if(id != localizacao.LocalizacaoId) {
                return BadRequest(); 
            }          

            try {

               await _repositorio.Alterar(localizacao);

            } catch(DbUpdateConcurrencyException) {
                
                // Verificamos se o objeto inserido realmente existe no banco
                var localizacao_valido = await _repositorio.BuscarPorID(id);
                
                if(localizacao_valido == null) {
                    return NotFound();
                } else {
                    throw;
                }
                
            }
            
            // NoContent() - Retorna 204
            return NoContent();
        }

        //DELETE api/localizacao/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Localizacao>> Delete(int id)
        {   
            // FindAsync = procura algo especifico no banco
            var localizacao = await _repositorio.BuscarPorID(id);
            
            if(localizacao == null) {
                return NotFound();
            }           

            await _repositorio.Excluir(localizacao);

            return localizacao;
        }
    }
}