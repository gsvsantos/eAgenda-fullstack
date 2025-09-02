using System.ComponentModel.DataAnnotations;

namespace eAgenda.Core.Dominio.ModuloDespesa;

public enum MeiosPagamento
{
    [Display(Name = "Dinheiro")] Dinheiro = 0,
    [Display(Name = "Débito")] Debito = 1,
    [Display(Name = "Crédito")] Credito = 2,
    [Display(Name = "Parcelado")] Parcelado = 3
}
