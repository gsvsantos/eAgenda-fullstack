using System.ComponentModel.DataAnnotations;

namespace eAgenda.Core.Dominio.ModuloTarefa;

public enum StatusItemTarefa
{
    [Display(Name = "Em Andamento")] EmAndamento = 0,
    [Display(Name = "Concluído")] Concluido = 1,
    [Display(Name = "Cancelado")] Cancelado = 2,
}