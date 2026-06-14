using DA.Example.Todo.Model;
using DA.Model.Framework;
using Microsoft.EntityFrameworkCore;

namespace DA.Example.Todo.Cons
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Hello, World!");
			Console.WriteLine("DB will be dropped!!!");
			Console.WriteLine("enter connstring or hit Ctrl+C to cancel");
			string connstring = Console.ReadLine()!;
			await CreateData(connstring);
			await ReadData(connstring);
		}

		static async Task CreateData(string connstring)
		{
			using TodoListContext ctx = new TodoListContext(connstring);
			await ctx.Database.EnsureDeletedAsync();	// drop DB
			await ctx.Database.EnsureCreatedAsync();    // create DB
			
			// create items
			ctx.TodoListItems.AddRange([BaseModel.Create<TodoListItem>("Küche wischen"),
				BaseModel.Create<TodoListItem>("Einkaufen"),
				BaseModel.Create<TodoListItem>("Katzenklo")
			]);
			await ctx.SaveChangesAsync();

			// create list and asign items from above
			var list = BaseModel.Create<TodoList>("zu erledigen");
			var first = ctx.TodoListItems.FirstOrDefault(i => i.ID == 1);
			var second = ctx.TodoListItems.FirstOrDefault(i => i.ID == 2);
			var third = ctx.TodoListItems.FirstOrDefault(i => i.ID == 3);
			// now assign list to items
			first!.List = list;
			second!.List = list;
			third!.List = list;
			ctx.TodoLists.Add(list);
			await ctx.SaveChangesAsync();
		}

		static async  Task ReadData(string connstring)
		{
			using TodoListContext ctx = new TodoListContext(connstring);
			var todolists = await ctx.TodoLists
				.Include(l=>l.Items)
				.ToListAsync();
			todolists.ForEach(l =>
			{
				Console.WriteLine(l.Name);
				l.Items!.ToList().ForEach(i => Console.WriteLine(i.Name));
			});
		}
	}
}
