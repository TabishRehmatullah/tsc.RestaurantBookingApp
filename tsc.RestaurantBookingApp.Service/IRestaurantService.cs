using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tsc.RestaurantBookingApp.Core.ViewModels;

namespace tsc.RestaurantBookingApp.Service
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantModel>> GetAllRestaurantAsync();
        Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId);

        Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId, DateTime date);

        Task<IEnumerable<RestaurantBranchModel>> GetRestaurantBranchsByRestaurantIdAsync(int restaurantId);

    }
}
