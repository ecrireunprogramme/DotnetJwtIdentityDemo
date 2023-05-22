using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetJwtIdentityDemo.Models
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole { Name = "Editeur", NormalizedName = "EDITEUR" },
                new IdentityRole { Name = "Administrateur", NormalizedName = "ADMINISTRATEUR" }
                );
        }
    }
}
