using System.Threading.Tasks;
using BackEnd.Domains;
using BackEnd.ViewModels;

namespace BackEnd.Interfaces
{
    public interface ILogin
    {        
        Usuario Logar(LoginViewModel login); 

    }
}