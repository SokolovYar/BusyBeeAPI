
namespace BusyBee.DataAccess.Repositories
{
    public interface ISideBarRepository
    {
        Task<int> GetSpecialistAllAsync();
        Task<int> GetSpecialistCurrentAsync();
        Task<List<string>> GetCategoryNamesAsync();
        Task<List<string>> GetTagNamesAsync();
    }
}