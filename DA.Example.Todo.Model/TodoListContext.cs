using DA.Model.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DA.Example.Todo.Model
{
	/// <ChangeLog>
	/// <Create Datum="14.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class TodoListContext : DataContext
	{
		public DbSet<TodoListItem> TodoListItems { get; set; }
		public DbSet<TodoList> TodoLists { get; set; }

		// WICHTIG: Dieser Konstruktor MUSS existieren, damit die Factory 
		// die MySQL-Konfiguration aus der Program.cs hier hineinjagen kann!
		[ActivatorUtilitiesConstructor]
		public TodoListContext(DbContextOptions<TodoListContext> options, IConfiguration cfg) : base(options)
		{
			configuration = cfg;
		}
		public TodoListContext(string connectionString):base(BuildContextOptions(connectionString))
		{
		}

		private static DbContextOptions<TodoListContext> BuildContextOptions(string connectionString)
		{
			var optionsBuilder = new DbContextOptionsBuilder<TodoListContext>();
			optionsBuilder.UseMySQL(connectionString);
			return optionsBuilder.Options;
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<TodoListItem>(entity =>
			{
				entity.HasKey(i => i.ID);
				entity.Property(i => i.Name).IsRequired();
			});
			modelBuilder.Entity<TodoList>(entity =>
			{
				entity.HasKey(l => l.ID);
				entity.Property(l => l.Name).IsRequired();
				entity.HasMany(l => l.Items).WithOne(li => li.List);
			});
		}
	}
}