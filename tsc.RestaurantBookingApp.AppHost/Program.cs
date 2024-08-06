var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.tsc_RestaurantBookingApp_API>("tsc-restaurantbookingapp-api");
builder.Build().Run();
