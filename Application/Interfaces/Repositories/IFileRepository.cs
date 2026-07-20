using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IFileRepository<T>
    {
        List<T> GetAllLine();
        void SaveAll(List<T> data);

    }
}
