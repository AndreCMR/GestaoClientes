using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace GestaoClientes.Infra.Db;

public sealed class SqlServerDbContext : IDbContext
{
    private readonly string _connectionString;

    public SqlServerDbContext(string connectionString)
        => _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

    public DbConnection CriarConexao() => new SqlConnection(_connectionString);
}