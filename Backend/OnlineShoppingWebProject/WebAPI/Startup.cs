using AutoMapper;
using Business.TokenHelper;
using Data.Context;
using Data.Mapping;
using Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace WebAPI
{
	public class Startup
	{
		private readonly string _cors = "cors";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			ConfigureSwagger(services);

			ConfigureAuthentication(services);

			ConfigureCors(services);

			services.AddDbContext<OnlineShopDbContext>
				(options => options.UseSqlServer(Configuration.GetConnectionString("OnlineShopDbConnectionString")));

			AddRepositories(services);

			services.AddScoped<IUserTokenIssuer>(sp => new UserTokenIssuer(Configuration["SecretKey"]));

			ConfigureMapping(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
			}

			app.UseHttpsRedirection();

			app.UseCors(_cors);

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		public void ConfigureSwagger(IServiceCollection services)
		{
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "bearer"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id="Bearer"
							}
						},
						new string[]{}
					}
				});
			});
		}

		public void AddRepositories(IServiceCollection services)
		{
			services.AddScoped<IAdminRepository, AdminRepository>();

			services.AddScoped<ICustomerRepository, CustomerRepository>();

			services.AddScoped<ISellerRepository, SellerRepository>();

			services.AddScoped<IOrderRepository, OrderRepository>();

			services.AddScoped<IItemRepository, ItemRepository>();

			services.AddScoped<IArticleRepository, ArticleRepository>();
		}

		public void ConfigureMapping(IServiceCollection services)
		{
			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			IMapper mapper = mapperConfig.CreateMapper();
			services.AddSingleton(mapper);
		}

		public void ConfigureAuthentication(IServiceCollection services)
		{
			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
		  .AddJwtBearer(options =>
		  {
			  options.TokenValidationParameters = new TokenValidationParameters
			  {
				  ValidateIssuer = true,
				  ValidateAudience = false,
				  ValidateLifetime = true,
				  ValidateIssuerSigningKey = true,
				  ValidIssuer = "http://localhost:44398",
				  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
			  };
		  });
		}

		public void ConfigureCors(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(name: _cors, builder =>
				{
					builder.WithOrigins("https://localhost:4200")
						   .AllowAnyHeader()
						   .AllowAnyMethod()
						   .AllowCredentials();
				});
			});

		}
	}
}
