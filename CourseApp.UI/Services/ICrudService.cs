using CourseApp.UI.Models;

namespace CourseApp.UI.Services
{
    public interface ICrudService
    {
        Task<PaginatedResponse<TResponse>> GetAllPaginated<TResponse>(string path, int page);
        Task<TResponse> Get<TResponse>(string path);
        Task<TResponse> Create<TRequest, TResponse>(string path, TRequest data);
        Task<TResponse> Edit<TRequest, TResponse>(string path, TRequest data);
        Task Delete(string path);
    }
}
