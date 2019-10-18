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
    public class CategoriaController : ControllerBase
    {   
        // Instaciamos nosso contexto("Banco")
        GufosContext _contexto = new GufosContext();

        // GET: api/categoria
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get() 
        {
            var categorias =  await _contexto.Categoria.ToListAsync();

            if(categorias == null) {
                return NotFound();
            }

            return categorias;
        }
    }
}