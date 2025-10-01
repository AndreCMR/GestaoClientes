using GestaoClientes.Application.Interfaces;
using GestaoClientes.Dominio.Entidades;
using GestaoClientes.Infra.Db;
using System.Data.Common;

namespace GestaoClientes.Infra.Repositorios;


public sealed class ClienteRepositorio : IClienteRepositorio
{
    private readonly IDbContext _db;
    public ClienteRepositorio(IDbContext db) => _db = db;

    public async Task<bool> ExistePorCpfAsync(string cpfSomenteDigitos, CancellationToken ct)
    {
        await using var con = _db.CriarConexao();
        await con.OpenAsync(ct);

        await using var cmd = con.CreateCommand();
        cmd.CommandText = "select top 1 1 from clientes where cpf = @cpf";
        DbHelpers.AddParameter(cmd, "@cpf", cpfSomenteDigitos);

        var result = await cmd.ExecuteScalarAsync(ct);
        return result is not null;
    }

    public async Task CriarAsync(Cliente c, CancellationToken ct)
    {
        await using var con = _db.CriarConexao();
        await con.OpenAsync(ct);

        await using var cmd = con.CreateCommand();
        cmd.CommandText = @"
            insert into clientes
            (id, nome, data_nascimento, cpf, email, rendimento_anual,
             logradouro, numero, complemento, bairro, cidade, estado, cep,
             ddd_telefone, numero_telefone)
            values
            (@id, @nome, @nasc, @cpf, @mail, @renda,
             @logra, @num, @compl, @bairro, @cidade, @estado, @cep,
             @ddd, @fone)";

        DbHelpers.AddParameter(cmd, "@id", c.Id);
        DbHelpers.AddParameter(cmd, "@nome", c.Nome);
        DbHelpers.AddParameter(cmd, "@nasc", c.DataNascimento.ToDateTime(TimeOnly.MinValue));
        DbHelpers.AddParameter(cmd, "@cpf", c.Cpf);
        DbHelpers.AddParameter(cmd, "@mail", c.Email);
        DbHelpers.AddParameter(cmd, "@renda", c.RendimentoAnual);

        DbHelpers.AddParameter(cmd, "@logra", c.Endereco.Logradouro);
        DbHelpers.AddParameter(cmd, "@num", c.Endereco.Numero);
        DbHelpers.AddParameter(cmd, "@compl", (object?)c.Endereco.Complemento ?? DBNull.Value);
        DbHelpers.AddParameter(cmd, "@bairro", c.Endereco.Bairro);
        DbHelpers.AddParameter(cmd, "@cidade", c.Endereco.Cidade);
        DbHelpers.AddParameter(cmd, "@estado", c.Endereco.Estado);
        DbHelpers.AddParameter(cmd, "@cep", c.Endereco.Cep);

        DbHelpers.AddParameter(cmd, "@ddd", c.DddTelefone);
        DbHelpers.AddParameter(cmd, "@fone", c.NumeroTelefone);

        await cmd.ExecuteNonQueryAsync(ct);
    }


    public async Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct)
    {
        await using var con = _db.CriarConexao();
        await con.OpenAsync(ct);

        await using var cmd = con.CreateCommand();
        cmd.CommandText = @"
            select id, nome, data_nascimento, cpf, email, rendimento_anual, estado, ddd_telefone, numero_telefone
            from clientes
            where id = @id";
        DbHelpers.AddParameter(cmd, "@id", id);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        return await rd.ReadAsync(ct) ? Map(rd) : null;
    }


    private static Cliente Map(DbDataReader r)
    {
        var endereco = new Endereco
        {
            Logradouro = DbHelpers.GetStringOrNull(r, "logradouro") ?? string.Empty,
            Numero = DbHelpers.GetStringOrNull(r, "numero") ?? string.Empty,
            Complemento = DbHelpers.GetStringOrNull(r, "complemento"),
            Bairro = DbHelpers.GetStringOrNull(r, "bairro") ?? string.Empty,
            Cidade = DbHelpers.GetStringOrNull(r, "cidade") ?? string.Empty,
            Estado = DbHelpers.GetString(r, "estado"), 
            Cep = DbHelpers.GetStringOrNull(r, "cep") ?? string.Empty
        };

        return new Cliente(
            id: DbHelpers.GetGuid(r, "id"),
            nome: DbHelpers.GetString(r, "nome"),
            dataNascimento: DbHelpers.GetDateOnly(r, "data_nascimento"),
            cpf: DbHelpers.GetString(r, "cpf"),
            email: DbHelpers.GetString(r, "email"),
            rendimentoAnual: DbHelpers.GetDecimal(r, "rendimento_anual"),
            endereco: endereco,
            dddTelefone: DbHelpers.GetString(r, "ddd_telefone"),
            numeroTelefone: DbHelpers.GetString(r, "numero_telefone")
        );
    }

}
