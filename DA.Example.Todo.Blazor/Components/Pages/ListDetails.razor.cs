using DA.Example.Todo.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DA.Example.Todo.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="15.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public partial class ListDetails_Code : TodoListPageBase
	{
		[Parameter]
		public int ListID { get; set; }
		public List<TodoListItem> Items { get; set; } = [];


		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
		}
		protected override async Task OnParametersSetAsync()
		{
			if (!Loading && context != null)
			{
				try
				{
					Loading = true;
					Items = await context.TodoListItems.Where(i => i.List!.ID == ListID).ToListAsync();
				}
				finally
				{
					Loading = false;
				}
			}
			await base.OnParametersSetAsync();
		}
		public async Task OnCheckedChangedAsync(TodoListItem item, bool newValue)
		{
			if (context == null)
				throw new NullReferenceException(nameof(context));
			if (newValue)
				item.CheckedDate = DateTime.UtcNow;
			else
				item.CheckedDate = null;

			await context.SaveChangesAsync();
			StateHasChanged();
		}
	}
}