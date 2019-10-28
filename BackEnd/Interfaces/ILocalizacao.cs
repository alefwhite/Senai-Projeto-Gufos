using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Domains;

namespace BackEnd.Interfaces
{
    public interface ILocalizacao
    {
         Task<List<Localizacao>> Listar();

         Task<Localizacao> BuscarPorID(int id);

         Task<Localizacao> Salvar(Localizacao localizacao); 

         Task<Localizacao> Alterar(Localizacao localizacao);

         Task<Localizacao> Excluir(Localizacao localizacao);
         
    }
}