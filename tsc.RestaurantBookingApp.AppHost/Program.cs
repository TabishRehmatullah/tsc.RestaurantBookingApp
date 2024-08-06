var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.tsc_RestaurantBookingApp_API>("tsc-restaurantbookingapp-api");

builder.AddProject<Projects.FunctionApp>("functionapp");

builder.Build().Run();
