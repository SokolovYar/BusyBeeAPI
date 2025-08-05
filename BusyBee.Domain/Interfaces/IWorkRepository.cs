using BusyBee.Domain.Models;

namespace BusyBee.DataAccess.Repositories
{
    public interface IWorkRepository
    {
        Task AddWork(Work work);
        Task AddWork(Work work, int categoryId);
        Task DeleteWork(int id);
        Task<IEnumerable<Work>> GetAllWorks();
        Task<Work> GetWorkById(int id);
        Task<IEnumerable<Work>> GetWorksByCategory(int categoryId);
        Task UpdateWork(Work work);
    }
}