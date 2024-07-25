using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.BLL.Services.IService
{
    public interface IAbsenceReasonService<TDto, TViewModel>
    {
        Task<int> AddAsync(TDto dto);

        Task<bool> UpdateAsync(int id, TDto dto);

        Task<IEnumerable<TViewModel>> GetAllAsync();
    }
}
