using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF_Core
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[]? args = null)
        {

            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseMySql(
                "Server = sql11.freemysqlhosting.net; " +
                "Port = 3306; " +
                "Database = sql11498693; " +
                "Uid = sql11498693; " +
                "Pwd = ehEr9WFBFW;",
            new MySqlServerVersion(new Version(8, 0, 11))
            );

            return new Context(optionsBuilder.Options);
        }
    }
}
