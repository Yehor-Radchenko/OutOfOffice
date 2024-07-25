namespace OutOfOffice.BLL.Services.IService
{
    public interface IService<TDto, TFullViewMode, TBriefViewModel, TTableViewModel>
    {
        Task<List<TTableViewModel>> GetTableDataAsync(string? searchValue = null);

        Task<TFullViewMode> GetFullInfoByIdAsync(int id);

        Task<List<TBriefViewModel>> GetBriefListAsync(string? search = null);

        Task<int> RegisterAsync(TDto dto);

        Task<bool> UpdateAsync(int id, TDto expectedEntityValues);
    }
}
