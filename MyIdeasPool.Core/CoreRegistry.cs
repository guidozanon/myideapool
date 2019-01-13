﻿using Microsoft.Extensions.DependencyInjection;
using MyIdeasPool.Core.DAL;
using MyIdeasPool.Core.Services;
using Microsoft.EntityFrameworkCore;
using MyIdeasPool.Core.Domain;

namespace MyIdeasPool.Core
{
	public static class CoreRegistry
	{
		public static void AddCoreRegistry(this IServiceCollection services, string connString)
		{
			services.AddScoped<IdeasContext>();
			services.AddScoped<IIdeasService, IdeasService>();

			services.AddDbContext<IdeasContext>(options =>
				options.UseSqlServer(connString)
			);

			services.AddDefaultIdentity<UserEntity>()
				.AddEntityFrameworkStores<IdeasContext>();
		}
	}
}