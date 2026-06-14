using DA.Model.Framework;
using Microsoft.EntityFrameworkCore;

namespace DA.Example.Todo.Model
{
	/// <ChangeLog>
	/// <Create Datum="14.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class TodoListContext : DataContext
	{
		public DbSet<TodoListItem> TodoListItems { get; set; }
		public DbSet<TodoList> TodoLists { get; set; }
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