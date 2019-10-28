using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Domains;

namespace BackEnd.Interfaces
{
    public interface IEvento
    {   
         Task<List<Evento>> Listar();

         Task<Evento> BuscarPorID(int id);

         Task<Evento> Salvar(Evento evento); 

         Task<Evento> Alterar(Evento evento);

         Task<Evento> Excluir(Evento evento);
         
    }
}