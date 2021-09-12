using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProteinCase.Infrastructure;
using Quartz;

namespace ProteinCase
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddControllers();
            services.AddEntityFrameworkNpgsql().AddDbContext<CurrencyDbContext>(
                opt => opt.UseNpgsql(Configuration.GetConnectionString("CurrencyDbConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ProteinCase", Version = "v1"});
            });
            services.AddQuartz(q =>
            {
                q.SchedulerId = "GetExchangesScheduler";
                q.SchedulerName = "Quartz Get Exchanges Scheduler";
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 10; });

                var jobKey = new JobKey("GetExchanges");
                q.AddJob<GetExchangeRatesJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("GetExchanges-Trigger")
                    .WithDailyTimeIntervalSchedule(s =>
                        s.OnMondayThroughFriday()
                            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(09, 00))
                            .WithIntervalInMinutes(60)
                            .EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(18, 00))
                    )
                );
            });
            services.AddScoped<GetExchangeRatesJob>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IMapper, Mapper>(provider =>
            {
                var config = new TypeAdapterConfig();
                config.Scan(typeof(Startup).Assembly);
                return new Mapper(config);
            });
            services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProteinCase v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}