using HairSalon.Contract.Repositories.Entity;
using HairSalon.Contract.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Contract.Repositories.SeedData
{
    public class RoleSeeder
    {
        public static async Task SeedRoles(IUnitOfWork unitOfWork)
        {
            var roleRepository = unitOfWork.GetRepository<ApplicationRoles>();

            if (await roleRepository.Entities.AnyAsync())
            {
                return; // Roles already seeded
            }

            var roles = new List<ApplicationRoles>
            {
                new ApplicationRoles
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    CreatedBy = "System",
                    CreatedTime = DateTimeOffset.UtcNow,
                    LastUpdatedTime = DateTimeOffset.UtcNow
                },
                new ApplicationRoles
                {
                    Id = Guid.NewGuid(),
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    CreatedBy = "System",
                    CreatedTime = DateTimeOffset.UtcNow,
                    LastUpdatedTime = DateTimeOffset.UtcNow
                },
                new ApplicationRoles
                {
                    Id = Guid.NewGuid(),
                    Name = "Stylist",
                    NormalizedName = "STYLIST",
                    CreatedBy = "System",
                    CreatedTime = DateTimeOffset.UtcNow,
                    LastUpdatedTime = DateTimeOffset.UtcNow
                },
                new ApplicationRoles
                {
                    Id = Guid.NewGuid(),
                    Name = "User",
                    NormalizedName = "USER",
                    CreatedBy = "System",
                    CreatedTime = DateTimeOffset.UtcNow,
                    LastUpdatedTime = DateTimeOffset.UtcNow
                }
            };

            foreach (var role in roles)
            {
                await roleRepository.InsertAsync(role);
            }

            await unitOfWork.SaveAsync();
        }
    }
}
