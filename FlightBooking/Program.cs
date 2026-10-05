using FlightBooking.Services.FlightServices;
using FlightBooking.Settings;
using System.Reflection;
using Microsoft.Extensions.Options;
using FlightBooking.Services.BookingServices;
using FlightBooking.Services.CheckInServices;
using FlightBooking.Services.MachineLearningServices;
using FlightBooking.Services;
using FlightBooking.Services.NoShowServices;
using FlightBooking.Services.OverBookingNoShowServices;
using FlightBooking.AgentServices;
using FlightBooking.AgentServices.OpenAIServices;
using FlightBooking.AgentSettings;
using FlightBooking.AgentServices.PromptBuilders;
using FlightBooking.AgentServices.IntentDetectors;
using FlightBooking.Tools.WeatherTool;
using FlightBooking.AgentServices.CityDetectors;
using FlightBooking.AgentServices.GooglePlacesServices;
using FlightBooking.Services.AirportServices;
using FlightBooking.Services.FlightSearchServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();
builder.Services.AddSingleton<FlightMlService>();
builder.Services.AddSingleton<FlightRegressionService>();
builder.Services.AddScoped<MongoFlightDataService>();
builder.Services.AddScoped<OverbookingRecommendationService>();
builder.Services.AddScoped<NoShowService>();
builder.Services.AddScoped<NoShowPredictionService>();
builder.Services.AddScoped<ITravelAgentService, TravelAgentService>();
builder.Services.AddScoped<IOpenAIService, OpenAIService>();
builder.Services.AddScoped<ITravelPromptBuilder, TravelPromptBuilder>();
builder.Services.AddScoped<IIntentDetector, TravelIntentDetector>();
builder.Services.AddScoped<IWeatherTool, WeatherTool>();
builder.Services.AddScoped<ICityExtractor, OpenAICityExtractor>();
builder.Services.AddScoped<IGooglePlaceService, GooglePlacesService>();
builder.Services.AddScoped<IAirportService, AirportService>();
builder.Services.AddScoped<IFlightSearchService, FlightSearchService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettingsKey"));
builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAI"));
builder.Services.AddHttpClient();
builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
