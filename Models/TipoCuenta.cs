using System.ComponentModel.DataAnnotations;
using ManejoPresupuesto.Validations;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Models;

public class TipoCuenta : IValidatableObject
{
    public int Id { get; set; }
    
    //[CapitalizeFirstLetter]
    [Required(ErrorMessage = "El Campo {0} Es Requerido.")]
    [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "La Longitud del campo {0} debe de estar entre {2} y {1}")]
    [Display(Name = "Nombre del Tipo Cuenta")]
    [Remote(action:"AccountTypeExists", controller: "TiposCuentas")]
    public string? Nombre { get; set; }
    
    public int UsuarioId { get; set; }
    
    public int Orden { get; set; }
    
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Nombre is not { Length: > 0 }) yield break;
        var firstLetter = Nombre.ToCharArray()[0].ToString();
        if (firstLetter != firstLetter.ToUpper())
        {
            yield return new ValidationResult("La primera letra debe ser mayúscula",new[] { nameof(Nombre) });
        }
    }
}