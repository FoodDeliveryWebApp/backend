﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IRestaurantRatingRepository
    {
        Task AddAsync(RestaurantRating rating);
        Task<List<RestaurantRating>> GetByRestaurantIdAsync(long restaurantId);
    }
}
