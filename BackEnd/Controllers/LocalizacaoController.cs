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
    public class LocalizacaoController : ControllerBase
    {   
        // Instaciamos nosso contexto("Banco")
        GufosContext _contexto = new GufosContext();

        // GET: api/localizacao
        [HttpGet]
        public async Task<ActionResult<List<Localizacao>>> Get() 
        {
            var localizacoes =  await _contexto.Localizacao.ToListAsync();

            if(localizacoes == null) {
                return NotFound();
            }

            return localizacoes;
        }

        // GET: api/localizacao/2
        [HttpGet("{id}")] // "{id}/{outro}" caso a rota tenha dois parametros
        public async Task<ActionResult<Localizacao>> Get(int id) 
        {   
            // FindAsync = procura algo especifico no banco
            var localizacao =  await _contexto.Localizacao.FindAsync(id);

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
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(localizacao);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();

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

            // Comparamos os atributos que foram modificados através do Entity Framework
            // No caso ele so irá dar um SET nas colunas que foram modificadas
            _contexto.Entry(localizacao).State = EntityState.Modified;   

            try {
                await _contexto.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                
                // Verificamos se o objeto inserido realmente existe no banco
                var localizacao_valido = await _contexto.Localizacao.FindAsync(id);
                
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
            var localizacao = await _contexto.Localizacao.FindAsync(id);
            
            if(localizacao == null) {
                return NotFound();
            }

            _contexto.Localizacao.Remove(localizacao);

            await _contexto.SaveChangesAsync();

            return localizacao;
        }
    }
}