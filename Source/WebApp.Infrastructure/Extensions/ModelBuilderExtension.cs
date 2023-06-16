using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApp.Infrastructure.Extensions
{
    public static class ModelBuilderExtension
    {
        public static ModelBuilder EntitiesOfType<TEntity>(this ModelBuilder modelBuilder, Action<EntityTypeBuilder> buildAction) where TEntity : class
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => typeof(TEntity).IsAssignableFrom(e.ClrType)))
            {
                buildAction(modelBuilder.Entity(entityType.ClrType));
            }
            return modelBuilder;
        }
    }
}
