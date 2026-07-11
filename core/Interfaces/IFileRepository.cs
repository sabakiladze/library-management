using Domain.Models;
using LibraryManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFileRepository<T>
    {
        List<T> GetAllLine();
        void SaveAll(List<T> data);

    }
}
