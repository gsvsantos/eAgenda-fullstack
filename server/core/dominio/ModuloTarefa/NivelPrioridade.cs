using System.ComponentModel.DataAnnotations;

namespace eAgenda.Core.Dominio.ModuloTarefa;

public enum NivelPrioridade
{
    [Display(Name = "Baixa")] Baixa = 0,
    [Display(Name = "Média")] Media = 1,
    [Display(Name = "Alta")] Alta = 2
}
