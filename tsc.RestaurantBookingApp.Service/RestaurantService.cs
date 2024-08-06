using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tsc.RestaurantBookingApp.Core.ViewModels;
using tsc.RestaurantBookingApp.Data;

namespace tsc.RestaurantBookingApp.Service
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }
        public async Task<IEnumerable<RestaurantModel>> GetAllRestaurantAsync()
        {
            return await _restaurantRepository.GetAllRestaurantAsync();
        }

        public async Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId)
        {
            return await _restaurantRepository.GetDiningTableByBranchAsync(branchId);
        }

        public async Task<IEnumerable<DiningTableWithTimeSlotsModel>> GetDiningTablesByBranchAsync(int branchId, DateTime date)
        {
            return await _restaurantRepository.GetDiningTableByBranchAsync(branchId, date);
        }

        public async Task<IEnumerable<RestaurantBranchModel>> GetRestaurantBranchsByRestaurantIdAsync(int restaurantId)
        {
            return await _restaurantRepository.GetRestaurantBranchsByRestaurantIdAsync(restaurantId);
        }
    }
}
