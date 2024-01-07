using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazorToDoWebAppSample.Components.Models;
using System.Diagnostics;

namespace BlazorToDoWebAppSample.Components.Data
{
    public class BlazorToDoWebAppSampleDbContext : DbContext
    {
        // Magic string.
        public static readonly string RowVersion = nameof(RowVersion);

        // Magic strings.
        public static readonly string DbName = $"{nameof(BlazorToDoWebAppSampleDbContext)}".ToLower();

        // Inject options.
        // options: The DbContextOptions{BlazorToDoWebAppSampleDbContext} for the context.
        public BlazorToDoWebAppSampleDbContext(DbContextOptions<BlazorToDoWebAppSampleDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoTask> TodoTask { get; set; } = default!;

        // Define the model.
        // modelBuilder: The ModelBuilder.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This property isn't on the C# class,
            // so we set it up as a "shadow" property and use it for concurrency.
            modelBuilder.Entity<TodoTask>()
                .Property<byte[]>(RowVersion)
                .IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }

        // Dispose pattern.
        public override void Dispose()
        {
            Debug.WriteLine($"{ContextId} context disposed.");
            base.Dispose();
        }

        // Dispose pattern.
        public override ValueTask DisposeAsync()
        {
            Debug.WriteLine($"{ContextId} context disposed async.");
            return base.DisposeAsync();
        }
    }
}
