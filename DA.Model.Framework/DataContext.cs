using DA.Model.Framework.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace DA.Model.Framework
{
	/// <ChangeLog>
	/// <Create Datum="14.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// in derivewd classes you need to add DbSet propeerties and override methods OnModelCreating (to add your entities)
	/// </summary>
	internal abstract class DataContext : DbContext
	{
		protected IConfiguration? configuration;
		protected string? connectionString = "";
		public DataContext(IConfiguration? config = null) : base()
		{
			if (config != null)
			{
				configuration = config;
				connectionString = config["ConnectionStrings:default"];
			}
		}
		public DataContext(string connString) : base()
		{
			connectionString = connString;
		}
		public void SetConfiguration(IConfiguration cfg)
		{
			if (cfg != null)
			{
				configuration = cfg;
				connectionString = configuration["ConnectionStrings:da_sdp_db"]!;
				Database.SetConnectionString(connectionString);
			}
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// https://stackoverflow.com/questions/74060289/mysqlconnection-open-system-invalidcastexception-object-cannot-be-cast-from-d
			// MariaDB 11+ doesnt work because of nullable PKs? So use MariaDB 10 or lower
			if (!string.IsNullOrEmpty(connectionString))
				optionsBuilder.UseMySQL(connectionString);
		}
		private void UpdateChangeDate()
		{
			DateTimeOffset now = DateTime.UtcNow;
			var createdEntries = ChangeTracker.Entries().Where(entry => entry.State == EntityState.Added).ToList();
			var changedEntries = ChangeTracker.Entries().Where(entry => entry.State == EntityState.Modified).ToList();
			createdEntries.ForEach(e =>
			{
				var prop = e.Properties.FirstOrDefault(prop => prop.Metadata.Name.Equals(nameof(ICurrentTimestamps.CreationDate)));
				if (prop != null)
					prop.CurrentValue = now;
			});
			changedEntries.ForEach(e =>
			{
				var prop = e.Properties.FirstOrDefault(prop => prop.Metadata.Name.Equals(nameof(ICurrentTimestamps.ChangeDate)));
				if (prop != null)
					prop.CurrentValue = now;
			});
		}
		public override int SaveChanges()
		{
			UpdateChangeDate();
			return base.SaveChanges();
		}
		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			UpdateChangeDate();
			return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
		{
			configurationBuilder.Properties<DateTimeOffset>().HaveConversion<DateTimeOffsetToUtcConverter>();
		}
	}
}