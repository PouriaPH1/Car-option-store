using System;
using System.Linq;
using _0_Framework.Application;
using AccountMangement.Infrastructure.EFCore;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceHost
{
    public static class DatabaseSeeder
    {
        public static void SeedDatabase(IServiceScope scope)
        {
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<AccountContext>();
                var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

                // Seed Roles with all permissions for admin
                if (!context.Roles.Any())
                {
                    // All permissions for admin (10-100 covers all permission codes)
                    var adminPermissions = new System.Collections.Generic.List<Permission>();
                    for (int i = 1; i <= 100; i++)
                    {
                        adminPermissions.Add(new Permission(i));
                    }
                    
                    var adminRole = new Role("مدیر سیستم", adminPermissions);
                    var userRole = new Role("کاربر سیستم", new System.Collections.Generic.List<Permission>());
                    var contentRole = new Role("محتوا گذار", new System.Collections.Generic.List<Permission>());
                    
                    context.Roles.Add(adminRole);
                    context.Roles.Add(userRole);
                    context.Roles.Add(contentRole);
                    context.SaveChanges();
                    
                    Console.WriteLine($"Roles seeded. Admin Role Id: {adminRole.Id} with {adminPermissions.Count} permissions");
                }

                // Seed Admin Account
                if (!context.Accounts.Any())
                {
                    var adminRole = context.Roles.FirstOrDefault(r => r.Name == "مدیر سیستم");
                    if (adminRole != null)
                    {
                        var hashedPassword = passwordHasher.Hash("admin123");
                        var admin = new Account(
                            fullname: "مدیر سایت",
                            username: "admin",
                            password: hashedPassword,
                            mobile: "09123456789",
                            roleId: adminRole.Id,
                            profilePhoto: ""
                        );
                        context.Accounts.Add(admin);
                        context.SaveChanges();
                        Console.WriteLine($"Admin account seeded with RoleId: {adminRole.Id}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding error: {ex.Message}");
            }
        }
    }
}
