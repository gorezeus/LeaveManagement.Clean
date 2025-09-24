using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = "bd26f36a-da86-48f4-87e3-09c6597b00c4",
                UserId = "d84a1df5-5cc2-4c5f-99c4-e57351f51bd5"
            },
            new IdentityUserRole<string>
            {
                RoleId = "2e52980e-49b5-4293-a57e-39cae9198c71",
                UserId = "c0a8464a-d1f5-4238-90ca-60f665b151a7"
            }
        );
    }
}
