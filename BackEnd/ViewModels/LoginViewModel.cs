using System.ComponentModel.DataAnnotations;

namespace BackEnd.ViewModels
{
    public class LoginViewModel 
    {   
        [Required]
        public string Email { get; set; }

        [StringLength(255, MinimumLength = 3)]
        public string Senha { get; set; }
    }
}