using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser is null)
        {
            logger.LogWarning("Unauthorized attempt to create restaurant: no current user.");
            throw new UnauthorizedAccessException("User must be authenticated to create a restaurant.");
        }

        logger.LogInformation("{UserEmail} [{UserId}] is creating a new restaurant {@Restaurant}",
            currentUser.Email,
            currentUser.Id,
            request);

        var restaurant = mapper.Map<Restaurant>(request) ?? throw new InvalidOperationException("Failed to map request to Restaurant.");
        restaurant.OwnerId = currentUser.Id;

        int id = await restaurantsRepository.Create(restaurant);
        return id;
    }
}
