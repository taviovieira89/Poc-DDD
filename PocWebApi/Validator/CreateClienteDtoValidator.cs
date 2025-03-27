using FluentValidation;

public class CreateClienteDtoValidator : AbstractValidator<CreateClienteDto>
{
    public CreateClienteDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório")
            .MaximumLength(100)
            .WithMessage("O nome não pode ter mais que 100 caracteres");

        RuleFor(x => x.Nascimento)
            .NotEmpty()
            .WithMessage("A data de nascimento é obrigatória")
            .LessThan(DateTime.Now)
            .WithMessage("A data de nascimento não pode ser futura");
    }
}
