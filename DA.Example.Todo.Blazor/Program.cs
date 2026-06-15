using DA.Example.Todo.Blazor.Components;
using DA.Example.Todo.Model;
using Microsoft.EntityFrameworkCore;

namespace DA.Example.Todo.Blazor
{
	/// <ChangeLog>
	/// <Create Datum="15.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorComponents()
				.AddInteractiveServerComponents();
			builder.Configuration.AddJsonFile("appsettings.local.json", false);    // there are some secrets which will not be committed to git
			builder.Services.AddDbContextFactory<TodoListContext>(options =>
			options.UseMySQL(connectionString: builder.Configuration.GetConnectionString("default")!));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
			app.UseAntiforgery();

			app.MapStaticAssets();
			app.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();

			app.Run();
		}
	}
}