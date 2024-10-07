using Azure;
using Microsoft.AspNetCore.JsonPatch;

namespace BloggingPlatformAPI.Services
{
    public interface ICommonService<T, TI, TU>
    {
        public List<string> Errors { get; }
        Task<IEnumerable<T>> Get();
        Task<T> GetById(int id);
        Task<T> Add(TI insertDTO);
        Task<T> Update(int id, TU updateDTO);
        Task<T> Patch(int id, TU updateDTO);
        Task<T> Delete(int id);
        bool Validate(TI insertDTO);
        bool Validate(TU updateDTO);
    }
}
