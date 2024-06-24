using OutOfOffice.BLL.Dto;
using OutOfOffice.BLL.ViewModels.ApprovalRequest;
using OutOfOffice.Common.Enums;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.BLL.Services.IService
{
    internal interface IRequestService
    {
        Task<bool> CancelRequestAsync(int id);
    }
}
