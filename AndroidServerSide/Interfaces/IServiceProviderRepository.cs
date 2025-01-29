using AndroidServerSide.Models;

namespace AndroidServerSide.Interfaces
{
    public interface IServiceProviderRepository
    {
        Task<IEnumerable<Models.ServiceProvider>> GetAllAsync();
        Task<Models.ServiceProvider> GetByIdAsync(int id);
        Task<IEnumerable<Models.ServiceProvider>> GetByServiceNameAsync(string serviceName);
        Task<int?> AddAsync(Models.ServiceProvider serviceProvider);
        Task UpdateAsync(Models.ServiceProvider serviceProvider);
        Task DeleteAsync(int id);
        Task<IEnumerable<Models.ServiceProvider>> SearchAsync(string searchTerm);
    }
}
