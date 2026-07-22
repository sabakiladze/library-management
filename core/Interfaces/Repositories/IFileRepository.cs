using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IFileRepository<T>
    {
        Task<List<T>> GetAllLineAsync();
        Task SaveAllAsync(List<T> data);
    }
}
