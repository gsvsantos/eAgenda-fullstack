using System.ComponentModel.DataAnnotations;

namespace eAgenda.Core.Dominio.ModuloTarefa;

public enum StatusTarefa
{
    [Display(Name = "Pendente")] Pendente = 0,
    [Display(Name = "Em Andamento")] EmAndamento = 1,
    [Display(Name = "Concluída")] Concluida = 2,
    [Display(Name = "Cancelada")] Cancelada = 3
}