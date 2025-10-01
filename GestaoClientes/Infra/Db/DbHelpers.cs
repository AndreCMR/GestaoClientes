using System.Data.Common;

namespace GestaoClientes.Infra.Db;


public static class DbHelpers
{
    public static void AddParameter(DbCommand cmd, string nome, object? valor)
    {
        var p = cmd.CreateParameter();
        p.ParameterName = nome;
        p.Value = valor ?? DBNull.Value;
        cmd.Parameters.Add(p);
    }

    public static string GetString(DbDataReader r, string coluna)
        => r.GetString(r.GetOrdinal(coluna));

    public static string? GetStringOrNull(DbDataReader r, string name)
    => r[name] == DBNull.Value ? null : r.GetString(r.GetOrdinal(name));


    public static Guid GetGuid(DbDataReader r, string coluna)
        => r.GetGuid(r.GetOrdinal(coluna));

    public static DateOnly GetDateOnly(DbDataReader r, string coluna)
        => DateOnly.FromDateTime(r.GetDateTime(r.GetOrdinal(coluna)));

    public static decimal GetDecimal(DbDataReader r, string coluna)
        => r.GetDecimal(r.GetOrdinal(coluna));
}