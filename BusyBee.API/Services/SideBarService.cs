using BusyBee.API.DTOs;
using BusyBee.DataAccess.Repositories;

namespace BusyBee.API.Services
{
    public interface ISideBarService
    {
        Task<SideBarDto> GetSideBarAsync();
    }

    public class SideBarService : ISideBarService
    {
        private readonly ISideBarRepository _repo;

        public SideBarService(ISideBarRepository repo)
        {
            _repo = repo;
        }

        public async Task<SideBarDto> GetSideBarAsync()
        {
            return new SideBarDto
            {
                SpecialistPlate = new SpecialistPlateDto
                {
                    SpecialistAll = await _repo.GetSpecialistAllAsync(),
                    SpecialistCurrent = await _repo.GetSpecialistCurrentAsync()
                },
                CategoryMenu = (await _repo.GetCategoryNamesAsync())
                    .ToList(),
                TagSectionSearching = await _repo.GetTagNamesAsync()
            };
        }
    }
}
