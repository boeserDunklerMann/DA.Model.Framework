using DA.Example.Todo.Model;
using Microsoft.AspNetCore.Components;
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

		public List<TodoList>? TodoLists { get; private set; }


		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			if (!Loading && context != null)
			{
				try
				{
					Loading = true;
					TodoLists = await context.TodoLists/*.Include(l=>l.Items)*/.ToListAsync();
				}
				finally
				{
					Loading = false;
				}
			}
		}
	}
}