using System.Linq;
using System.Threading.Tasks;
using BackEnd.Domains;
using BackEnd.Interfaces;
using BackEnd.ViewModels;

namespace BackEnd.Repositories
{
    public class LoginRepository : ILogin
    {
        public Usuario Logar(LoginViewModel login)
        {   
            using(GufosContext _contexto = new GufosContext()){
                var usuario = _contexto.Usuario.FirstOrDefault(
                    u => u.Email == login.Email && u.Senha == login.Senha
                );

                return usuario;
            }
        }
    }
}