using FluentValidation;
using GestaoClientes.Application.Interfaces;
using GestaoClientes.DTOs.Requests;
using System.Text.RegularExpressions;

namespace GestaoClientes.Application.Validations;

public sealed class ClienteCriarRequestValidator : AbstractValidator<ClienteCriarRequest>
{
    public ClienteCriarRequestValidator(IClienteRepositorio repositorio)
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("Data de Nascimento é obrigatória.")
            .Must(d => d.ToDateTime(TimeOnly.MinValue) <= DateTime.Today)
                .WithMessage("Data de Nascimento deve ser no passado.")
            .Must(d => IdadeEntre(d, 16, 120))
                .WithMessage("Idade deve estar entre 16 e 120 anos.");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("CPF é obrigatório.")
            .Must(IsCpfValido).WithMessage("CPF inválido.")
            .MustAsync(async (cpf, ct) => !await repositorio.ExistePorCpfAsync(Numeros(cpf), ct))
            .WithMessage("CPF já cadastrado.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.")
            .MaximumLength(200);

        RuleFor(x => x.RendimentoAnual)
            .NotNull().WithMessage("Rendimento Anual é obrigatório.")
            .GreaterThan(0).WithMessage("Rendimento Anual deve ser maior que zero.");

        RuleFor(x => x.Endereco.Estado)
              .NotEmpty().WithMessage("Estado é obrigatório.")
              .Matches("^[A-Z]{2}$").WithMessage("Estado deve ser a sigla de 2 letras (ex: SP, RJ).");

        RuleFor(x => x.DddTelefone)
            .NotEmpty().WithMessage("DDD do telefone é obrigatório.")
            .Matches(@"^\d{2}$").WithMessage("DDD deve ter exatamente 2 dígitos.");

        RuleFor(x => x.NumeroTelefone)
            .NotEmpty().WithMessage("Número do telefone é obrigatório.")
            .Matches(@"^\d{8,9}$").WithMessage("Número do telefone deve ter 8 ou 9 dígitos.");
    }

    private static bool IdadeEntre(DateOnly nascimento, int min, int max)
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var idade = hoje.Year - nascimento.Year;

        if (nascimento > hoje.AddYears(-idade)) idade--;

        return idade >= min && idade <= max;
    }

    private static readonly HashSet<string> UFs = new(StringComparer.OrdinalIgnoreCase)
        {
            "AC","AL","AP","AM","BA","CE","DF","ES","GO","MA","MT","MS","MG",
            "PA","PB","PR","PE","PI","RJ","RN","RS","RO","RR","SC","SP","SE","TO"
        };

    private static bool IsCpfValido(string? cpf)
    {
        cpf = Numeros(cpf);
        if (string.IsNullOrEmpty(cpf) || cpf.Length != 11) return false;
        if (new string(cpf[0], 11) == cpf) return false; 

        var soma = 0;
        for (int i = 0; i < 9; i++) soma += (cpf[i] - '0') * (10 - i);
        var resto = soma % 11;
        var dig1 = resto < 2 ? 0 : 11 - resto;

        soma = 0;
        for (int i = 0; i < 10; i++) soma += (cpf[i] - '0') * (11 - i);
        resto = soma % 11;
        var dig2 = resto < 2 ? 0 : 11 - resto;

        return cpf[9] - '0' == dig1 && cpf[10] - '0' == dig2;
    }

    private static string Numeros(string? s) => string.IsNullOrWhiteSpace(s)
        ? string.Empty
        : Regex.Replace(s, "[^0-9]", "");
}