using Microsoft.EntityFrameworkCore;
using CatalogService.Application.Entities;

namespace CatalogService.Infrastructure
{
    public class CatalogServiceContext : DbContext 
    {
        public CatalogServiceContext(DbContextOptions<CatalogServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Item> Items { get; set; }

    }
}
