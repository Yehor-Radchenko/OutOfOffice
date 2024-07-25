namespace OutOfOffice.BLL.Services.IService
{
    public interface ISubdivisionService<TDto, TViewModel>
    {
        Task<int> AddAsync(TDto dto);

        Task<bool> UpdateAsync(int id, TDto dto);

        Task<IEnumerable<TViewModel>> GelAllAsync();
    }
}
