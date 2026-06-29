using System.ComponentModel.DataAnnotations;

namespace task_manager.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "El campo {0} debe estar entre {2} y {1} caracteres")]
    [Display(Name = "Usuario")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "El campo {0} debe ser un correo valido")]
    [Display(Name = "Correo electronico")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Contraseña")]
    [DataType(DataType.Password)]
    public string PasswordHash { get; set; } = string.Empty;
}
