using System.Text.Json.Serialization;
using PokeAPI.DAOs;
using PokeAPI.Data;
using PokeAPI.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddTransient<Seed>();
        builder.Services.AddControllers();
        builder.Services.AddControllers().AddJsonOptions((options) => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddScoped<IPokemonDAO, PokemonRepository>();
        builder.Services.AddScoped<ICategoryDAO, CategoryRepository>();
        builder.Services.AddScoped<IRegionDAO, RegionRepository>();
        builder.Services.AddScoped<IOwnerDAO, OwnerRepository>();
        builder.Services.AddScoped<IReviewDAO, ReviewRepository>();
        builder.Services.AddScoped<IReviewerDAO, ReviewerRepository>();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<DataContext>((options) => { });

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        if (args.Length == 1 && args[0].ToLower() == "populate") SeedData(app);

        app.Run();


        void SeedData(IHost app)
        {
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopedFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<Seed>();
                service.SeedDataContext();
            }
        }
    }
}