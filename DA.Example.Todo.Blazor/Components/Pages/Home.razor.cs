using DA.Example.Todo.Model;
using DA.Model.Framework;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace DA.Example.Todo.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="15.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public partial class Home_Code : TodoListPageBase
	{
		[Inject]
		public IConfiguration? configuration { get; set; } = default;
		public TodoList NewItem { get; set; } = default!;

		public List<TodoList>? TodoLists { get; set; } = [];


		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			if (!Loading)
			{
				try
				{
					using var context = await DbContextFactory.CreateDbContextAsync();
					Loading = true;
					NewItem = BaseModel.Create<TodoList>("");
					editContext = new EditContext(NewItem);
					TodoLists = await context.TodoLists.ToListAsync();
				}
				finally
				{
					Loading = false;
				}
			}
		}

		public async Task OnNewListSubmittedAsync()
		{
			if (Loading) return;
			if (editContext != null && editContext.Validate())
			{
				try
				{
					using var context = await DbContextFactory.CreateDbContextAsync();
					Loading = true;
					TodoList newItem = BaseModel.Create<TodoList>(NewItem.Name);

					await context.TodoLists.AddAsync(newItem);
					await context.SaveChangesAsync();
					TodoLists?.Add(newItem);

					// WICHTIG: Für die NÄCHSTE Eingabe alles zurücksetzen!
					NewItem = BaseModel.Create<TodoList>("");
					editContext = new EditContext(NewItem);
				}
				finally
				{
					Loading = false;
					StateHasChanged();
				}
			}
		}
	}
}