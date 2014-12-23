using DaNangZ.DbFirst.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.DbFirst
{
    public interface IDaNangZEntities : IDisposable
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Entry> Entries { get; set; }
        DbSet<sysdiagram> sysdiagrams { get; set; }
        DbSet<UserProfile> UserProfiles { get; set; }
        DbSet<webpages_Membership> webpages_Membership { get; set; }
        DbSet<webpages_Roles> webpages_Roles { get; set; }

        int SaveChanges();
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
