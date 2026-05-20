using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AutoAdmin.Data;

public class AutoAdminDbContextFactory : IDesignTimeDbContextFactory<AutoAdminDbContext>
{
    public AutoAdminDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AutoAdminDbContext>();

        optionsBuilder.UseSqlite("Data Source=autoadmin.db");

        return new AutoAdminDbContext(optionsBuilder.Options);
    }
}