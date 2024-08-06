using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tsc.RestaurantBookingApp.Core.ViewModels;


namespace tsc.RestaurantBookingApp.Data
{
    public interface IRestaurantRepository
    {
        Task<List<RestaurantModel>> GetAllRestaurantAsync();

        Task<IEnumerable<RestaurantBranchModel>> GetRestaurantBranchsByRestaurantIdAsync(int restaurantId);

        Task<IEnumerable<DiningTableWithTimeSlotsModel>>GetDiningTableByBranchAsync(int branchId, DateTime date);

        Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTableByBranchAsync(int branchId);
    }
}
 