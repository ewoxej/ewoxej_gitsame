using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using ewoxej_gitsame.Sources;
using ewoxej_gitsame.Models;
using System.Data.SQLite;

namespace ewoxej_gitsame
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        : base(new SQLiteConnection(){ ConnectionString = new SQLiteConnectionStringBuilder() 
        { DataSource = Helper.DbPath, ForeignKeys = true }.ConnectionString }, true)
        {
            Settings = Settings.getInstance();
        }

        public DbSet<Source> Sources { get; set; }
        public DbSet<File> Files { get; set; }
        public Settings Settings { get; set; }
    }
}


