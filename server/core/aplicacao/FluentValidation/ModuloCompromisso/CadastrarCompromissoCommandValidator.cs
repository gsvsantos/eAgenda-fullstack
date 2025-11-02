using eAgenda.Core.Aplicacao.ModuloCompromisso.Commands;
using eAgenda.Core.Dominio.ModuloCompromisso;
using FluentValidation;

namespace eAgenda.Core.Aplicacao.FluentValidation.ModuloCompromisso;

public class CadastrarCompromissoCommandValidator : AbstractValidator<CadastrarCompromissoCommand>
{
    public CadastrarCompromissoCommandValidator()
    {
        RuleFor(x => x.Assunto)
           .NotEmpty().WithMessage("O assunto é obrigatório.")
           .MinimumLength(2).WithMessage("O assunto deve ter pelo menos {MinLength} caracteres.")
           .MaximumLength(100).WithMessage("O assunto deve conter no máximo {MaxLength} caracteres.");

        RuleFor(x => x.DataOcorrencia)
           .NotEmpty().WithMessage("A data do compromisso é obrigatória.");

        RuleFor(x => x.HoraInicio)
           .NotEmpty().WithMessage("A hora de inicio é obrigatória.");

        RuleFor(x => x.HoraTermino)
           .NotEmpty().WithMessage("A hora de término é obrigatória.");

        RuleFor(x => x.TipoCompromisso)
           .NotEmpty().WithMessage("O tipo de compromisso é obrigatório.");

        When(x => x.TipoCompromisso == TipoCompromisso.Remoto, () =>
        {
            RuleFor(x => x.Link)
               .NotNull()
               .NotEmpty().WithMessage("O assunto é obrigatório.")
               .MinimumLength(2).WithMessage("O assunto deve ter pelo menos {MinLength} caracteres.")
               .MaximumLength(100).WithMessage("O assunto deve conter no máximo {MaxLength} caracteres.");
        }).Otherwise(() =>
        {
            RuleFor(x => x.Local)
                .NotNull()
                .NotEmpty().WithMessage("O assunto é obrigatório.")
                .MinimumLength(2).WithMessage("O assunto deve ter pelo menos {MinLength} caracteres.")
                .MaximumLength(100).WithMessage("O assunto deve conter no máximo {MaxLength} caracteres.");
        });
    }
}
