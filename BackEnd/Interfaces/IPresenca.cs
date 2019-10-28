using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Domains;

namespace BackEnd.Interfaces
{
    public interface IPresenca
    {
         Task<List<Presenca>> Listar();

         Task<Presenca> BuscarPorID(int id);

         Task<Presenca> Salvar(Presenca presenca); 

         Task<Presenca> Alterar(Presenca presenca);

         Task<Presenca> Excluir(Presenca presenca);
    }
}