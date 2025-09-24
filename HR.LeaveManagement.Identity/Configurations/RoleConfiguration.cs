 using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "2e52980e-49b5-4293-a57e-39cae9198c71",
                Name = "Employee",
                NormalizedName = "EMPLOYEE"        
            },
            new IdentityRole
            {
                Id = "bd26f36a-da86-48f4-87e3-09c6597b00c4",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"        
            }
        ); 
    }
}
