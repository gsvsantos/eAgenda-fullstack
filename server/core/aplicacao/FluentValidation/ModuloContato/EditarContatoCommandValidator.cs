using eAgenda.Core.Aplicacao.ModuloContato.Commands;
using FluentValidation;
using System.Text.RegularExpressions;

namespace eAgenda.Core.Aplicacao.FluentValidation.ModuloContato;

public class EditarContatoCommandValidator : AbstractValidator<EditarContatoCommand>
{
    public EditarContatoCommandValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("Insira um Nome.")
            .MinimumLength(2).WithMessage("O nome deve ter pelo menos {MinLength} caracteres.")
            .MaximumLength(150).WithMessage("O nome deve ter pelo menos {MaxLength} caracteres.");

        RuleFor(c => c.Telefone)
            .NotEmpty().WithMessage("Insira um Telefone.")
            .Must(TelefoneEValido).WithMessage("Por favor, insira um número de telefone válido, ex: (99) 99999-9999 ou (99) 9999-9999.");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Insira um Email")
            .MinimumLength(6).WithMessage("O email deve ter pelo menos {MinLength} caracteres.")
            .MaximumLength(254).WithMessage("O email deve ter pelo menos {MaxLength} caracteres.");

        RuleFor(c => c.Cargo)
            .MinimumLength(2).WithMessage("O cargo deve ter pelo menos {MinLength} caracteres.")
            .MaximumLength(maximumLength: 150).WithMessage("O cargo deve ter pelo menos {MaxLength} caracteres.")
            .When(c => !string.IsNullOrWhiteSpace(c.Cargo));

        RuleFor(c => c.Empresa)
            .MinimumLength(2).WithMessage("A empresa deve ter pelo menos {MinLength} caracteres.")
            .MaximumLength(maximumLength: 200).WithMessage("A empresa deve ter pelo menos {MaxLength} caracteres.")
            .When(c => !string.IsNullOrWhiteSpace(c.Empresa));
    }

    private static bool TelefoneEValido(string input)
    {
        return Regex.IsMatch(input, "^\\(?\\d{2}\\)?\\s?(9\\d{4}|\\d{4})-?\\d{4}$");
    }
}
