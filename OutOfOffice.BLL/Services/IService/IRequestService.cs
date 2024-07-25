namespace OutOfOffice.BLL.Services.IService
{
    internal interface IRequestService
    {
        Task<bool> CancelRequestAsync(int id);
    }
}
