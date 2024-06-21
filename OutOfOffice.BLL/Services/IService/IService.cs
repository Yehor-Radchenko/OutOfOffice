namespace OutOfOffice.BLL.Services.IService
{
    internal interface IService<TDto, TViewModel>
    {
        Task<IEnumerable<TViewModel>> GetAllAsync(string? searchValue = null);

        Task<TViewModel>
    }
}
