﻿using HairSalon.Core;
using HairSalon.ModelViews.AppointmentModelViews;
using HairSalon.ModelViews.ShopModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Contract.Services.Interface
{
    public interface IAppointmentService
    {
        Task<AppointmentModelView> AddAppointmentAsync(AppointmentCreateModel model);
        Task<BasePaginatedList<AppointmentModelView>> GetAllAppointmentAsync(int pageNumber, int pageSize);
        Task<AppointmentModelView> GetAppointmentAsync(string id);
        Task<AppointmentModelView> UpdateAppointmentAsync(string id, UpdateAppointmentModel model);
        Task<string> DeleteAppointmentAsync(string id);
    }
}
