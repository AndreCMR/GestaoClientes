using System.Data.Common;

namespace GestaoClientes.Infra.Db;

public interface IDbContext
{
    DbConnection CriarConexao();
}