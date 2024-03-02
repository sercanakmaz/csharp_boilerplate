using System.Threading.Tasks;

namespace Boilerplate.Infrastructure.Persistence;

public interface IRepository
{
    Task CreateIndexes();
}