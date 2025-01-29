using AndroidServerSide.Interfaces;
using AndroidServerSide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AndroidServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceProvidersController : ControllerBase
    {
        private readonly IServiceProviderRepository _serviceProviderRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMemoryCache _memoryCache;

        public ServiceProvidersController(IServiceProviderRepository serviceProviderRepository, 
            IWebHostEnvironment webHostEnvironment, IMemoryCache memoryCache)
        {
            _serviceProviderRepository = serviceProviderRepository;
            _webHostEnvironment = webHostEnvironment;
            _memoryCache = memoryCache;
        }

        // GET: api/ServiceProviders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.ServiceProvider>>> GetServiceProviders()
        {
            string cacheKey = "AllProviders";

            // Try to get service providers from cache
            if (!_memoryCache.TryGetValue(cacheKey, out var serviceProvidersFromCache))
            {
                var serviceProviders = await _serviceProviderRepository.GetAllAsync();
                
                // Delete from cache automatically after 1 hour
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _memoryCache.Set(cacheKey, serviceProviders, cacheEntryOptions);
                return Ok(serviceProviders);
            }
            return Ok(serviceProvidersFromCache);
        }

        // GET: api/ServiceProviders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.ServiceProvider>> GetServiceProvider(int id)
        {
            string cacheKey = "Provider" + id;
            if (!_memoryCache.TryGetValue(cacheKey, out var serviceProviderFromCache))
            {
                var serviceProvider = await _serviceProviderRepository.GetByIdAsync(id);
                if (serviceProvider == null)
                    return NotFound();

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _memoryCache.Set(cacheKey, serviceProvider, cacheEntryOptions);
                return Ok(serviceProvider);
            }
            return Ok(serviceProviderFromCache);
        }

        // GET: api/ServiceProviders/Service/{serviceName}
        [HttpGet("Service/{serviceName}")]
        public async Task<ActionResult<IEnumerable<Models.ServiceProvider>>> GetServiceProvidersByServiceName(string serviceName)
        {
            // Not saving this in cache because it is not being used anywhere as of now
            var serviceProviders = await _serviceProviderRepository.GetByServiceNameAsync(serviceName);
            if (serviceProviders == null || !serviceProviders.Any())
                return NotFound();
            return Ok(serviceProviders);
        }

        // POST: api/ServiceProviders
        [HttpPost]
        public async Task<ActionResult<Models.ServiceProvider>> CreateServiceProvider([FromForm] ServiceProviderDTO serviceProviderDTO, [FromForm] IFormFile imageFile)
        {
            Models.ServiceProvider serviceProvider = new Models.ServiceProvider
            {
                Name = serviceProviderDTO.ServiceProviderName,
                PhoneNumber = serviceProviderDTO.PhoneNumber,
                Address = serviceProviderDTO.Address,
                Description = serviceProviderDTO.Description,
                Rate = serviceProviderDTO.Rate,
                Discount = serviceProviderDTO.Discount,
                Rating = serviceProviderDTO.Rating,
                ServiceName = serviceProviderDTO.ServiceName
            };

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                var extension = Path.GetExtension(imageFile.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                var imagesDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", "ServiceProviders");
                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }

                var filePath = Path.Combine(imagesDirectory, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                var imageUrl = $"Images/ServiceProviders/{uniqueFileName}";
                serviceProvider.Image = imageUrl;
            }

            int? serviceProviderid = await _serviceProviderRepository.AddAsync(serviceProvider);

            // Invalidate the cache so it reflects the changes
            _memoryCache.Remove("AllProviders");
            return CreatedAtAction(nameof(GetServiceProvider), new { id = serviceProvider.Id }, serviceProvider);
        }

        // PUT: api/ServiceProviders
        [HttpPut]
        public async Task<IActionResult> UpdateServiceProvider([FromForm] ServiceProviderDTO serviceProviderDTO, [FromForm] IFormFile? imageFile)
        {
            Models.ServiceProvider serviceProvider = new Models.ServiceProvider
            {
                Id = serviceProviderDTO.Id,
                Name = serviceProviderDTO.ServiceProviderName,
                PhoneNumber = serviceProviderDTO.PhoneNumber,
                Address = serviceProviderDTO.Address,
                Image = serviceProviderDTO.Image?? string.Empty,
                Description = serviceProviderDTO.Description,
                Rate = serviceProviderDTO.Rate,
                Discount = serviceProviderDTO.Discount,
                Rating = serviceProviderDTO.Rating,
                ServiceName = serviceProviderDTO.ServiceName
            };

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                var extension = Path.GetExtension(imageFile.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                var imagesDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", "ServiceProviders");
                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }

                var filePath = Path.Combine(imagesDirectory, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                var imageUrl = $"Images/ServiceProviders/{uniqueFileName}";
                serviceProvider.Image = imageUrl;
            }

            await _serviceProviderRepository.UpdateAsync(serviceProvider);

            _memoryCache.Remove("AllProviders");
            _memoryCache.Remove("Provider" + serviceProvider.Id);
            return NoContent();
        }

        // DELETE: api/ServiceProviders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceProvider(int id)
        {
            await _serviceProviderRepository.DeleteAsync(id);
            _memoryCache.Remove("AllProviders");
            _memoryCache.Remove("Provider" + id);
            return NoContent();
        }

        // GET: api/ServiceProviders/search/{searchTerm}
        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> SearchServiceProvider(string searchTerm)
        {
            return new OkObjectResult(await _serviceProviderRepository.SearchAsync(searchTerm));
        }
    }
}
