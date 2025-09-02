using System.ComponentModel.DataAnnotations;

namespace eAgenda.Core.Dominio.ModuloCompromisso;

public enum TipoCompromisso
{
    [Display(Name = "Remoto")] Remoto = 0,
    [Display(Name = "Presencial")] Presencial = 1
}