﻿using BloggingPlatformAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace BloggingPlatformAPI.Helpers
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app) 
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using BlogContext blogContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
            blogContext.Database.Migrate();
            using UserContext userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
            userContext.Database.Migrate();
        }
    }
}
