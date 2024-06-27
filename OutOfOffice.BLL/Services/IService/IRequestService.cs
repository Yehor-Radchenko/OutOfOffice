namespace OutOfOffice.Common.Services.IService
{
    internal interface IRequestService
    {
        Task<bool> CancelRequestAsync(int id);
    }
}
