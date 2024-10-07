using AutoMapper;
using HairSalon.Contract.Repositories.Entity;
using HairSalon.Contract.Repositories.Interface;
using HairSalon.Contract.Services.Interface;
using HairSalon.ModelViews.ServiceModelViews;
using ServiceEntity = HairSalon.Contract.Repositories.Entity.Service;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core;

namespace HairSalon.Services.Service
{
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        }
        public async Task<BasePaginatedList<ServiceModelView>> GetAllServiceAsync(int pageNumber, int pageSize)
        {
            IQueryable<ServiceEntity> serviceQuery = _unitOfWork.GetRepository<ServiceEntity>().Entities
               .Where(p => !p.DeletedTime.HasValue)
               .OrderByDescending(s => s.CreatedTime);

            int totalCount = await serviceQuery.CountAsync();

            List<ServiceEntity> paginatedShops = await serviceQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            List<ServiceModelView> serviceModelViews = _mapper.Map<List<ServiceModelView>>(paginatedShops);

            return new BasePaginatedList<ServiceModelView>(serviceModelViews, totalCount, pageNumber, pageSize);
        }


        public async Task<ServiceModelView> AddServiceAsync(CreateServiceModelView model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new Exception("Service name cannot be empty.");
            }

            ServiceEntity newService = _mapper.Map<ServiceEntity>(model);
            
            newService.Id = Guid.NewGuid().ToString("N");
            newService.CreatedBy = "claim account";  
            newService.CreatedTime = DateTimeOffset.UtcNow;
            newService.LastUpdatedTime = DateTimeOffset.UtcNow;

            await _unitOfWork.GetRepository<ServiceEntity>().InsertAsync(newService);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ServiceModelView>(newService);

        }

        public async Task<string> DeleteServiceAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new Exception("Please provide a valid Service ID.");
            }

            ServiceEntity existingService = await _unitOfWork.GetRepository<ServiceEntity>().Entities
                .FirstOrDefaultAsync(s => s.Id == id && !s.DeletedTime.HasValue)
                ?? throw new Exception("The Service cannot be found or has been deleted!");

            existingService.DeletedTime = DateTimeOffset.Now;
            existingService.DeletedBy = "claim account"; 

            _unitOfWork.GetRepository<ServiceEntity>().Update(existingService);
            await _unitOfWork.SaveAsync();
            return "Deleted";
        }

        

        public async Task<ServiceModelView> GetServiceAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new Exception("Please provide a valid Shop ID.");
            }

            ServiceEntity existingService = await _unitOfWork.GetRepository<ServiceEntity>().Entities
                .FirstOrDefaultAsync(s => s.Id == id && !s.DeletedTime.HasValue)
                ?? throw new Exception("The Service cannot be found or has been deleted!");

            return _mapper.Map<ServiceModelView>(existingService);
        }

        public async Task<ServiceModelView> UpdateServiceAsync(string id, UpdatedServiceModelView model)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new Exception("Please provide a valid Service ID.");
            }

            ServiceEntity existingService = await _unitOfWork.GetRepository<ServiceEntity>().Entities
               .FirstOrDefaultAsync(s => s.Id == id && !s.DeletedTime.HasValue)
               ?? throw new Exception("The Service cannot be found or has been deleted!");
            
            _mapper.Map(model, existingService);

            existingService.LastUpdatedBy = "claim account";  
            existingService.LastUpdatedTime = DateTimeOffset.UtcNow;

            _unitOfWork.GetRepository<ServiceEntity>().Update(existingService);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ServiceModelView>(existingService);
        }
    }
}
