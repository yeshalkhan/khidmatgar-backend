using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AndroidServerSide.Interfaces;
using AndroidServerSide.Models;
using AndroidServerSide.DbContexts;

namespace AndroidServerSide.Repositories
{
    public class ServiceProviderRepository : IServiceProviderRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceProviderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.ServiceProvider>> GetAllAsync()
        {
            return await _context.ServiceProviders.ToListAsync();
        }

        public async Task<Models.ServiceProvider> GetByIdAsync(int id)
        {
            return await _context.ServiceProviders.FindAsync(id);
        }

        public async Task<IEnumerable<Models.ServiceProvider>> GetByServiceNameAsync(string serviceName)
        {
            return await _context.ServiceProviders
                                 .Where(sp => sp.ServiceName == serviceName)
                                 .ToListAsync();
        }

        public async Task<int?> AddAsync(Models.ServiceProvider serviceProvider)
        {
            _context.ServiceProviders.Add(serviceProvider);
            await _context.SaveChangesAsync();

            // After saving, the Id will be set in the serviceProvider object
            return serviceProvider.Id;
        }

        public async Task UpdateAsync(Models.ServiceProvider serviceProvider)
        {
            _context.ServiceProviders.Update(serviceProvider);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var serviceProvider = await _context.ServiceProviders.FindAsync(id);
            if (serviceProvider != null)
            {
                _context.ServiceProviders.Remove(serviceProvider);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Models.ServiceProvider>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return Enumerable.Empty<Models.ServiceProvider>();

            searchTerm = searchTerm.ToLower();
            return await _context.ServiceProviders
                        .Where(serviceProvider => serviceProvider.Name.ToLower().Contains(searchTerm)
                        || serviceProvider.ServiceName.ToLower().Contains(searchTerm))
                        .ToListAsync();
        }
    }
}
