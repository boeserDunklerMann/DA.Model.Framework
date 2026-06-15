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
	public partial class ListDetails_Code : TodoListPageBase
	{
		[Parameter]
		public int ListID { get; set; }
		private TodoList? currentList;
		public List<TodoListItem> Items { get; set; } = [];
		public TodoListItem NewItem { get; set; } = BaseModel.Create<TodoListItem>("");

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			if (!Loading && context != null)
			{
				try
				{
					Loading = true;
					currentList = await context.TodoLists.Where(l => l.ID == ListID).FirstAsync();
					Items = await context.TodoListItems.Where(i => i.List!.ID == ListID).ToListAsync();
					editContext = new EditContext(NewItem);
				}
				finally
				{
					Loading = false;
				}
			}
		}
		protected override async Task OnParametersSetAsync()
		{
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
		public async Task OnNewItemSubmittedAsync()
		{
			if (Loading)
				return;
			if (context == null)
				throw new NullReferenceException(nameof(context));

			try
			{
				Loading = true;
				NewItem.List = currentList;
				await context.TodoListItems.AddAsync(NewItem);
				await context.SaveChangesAsync();
				StateHasChanged();
				Items.Add(NewItem);
			}
			finally
			{
				Loading = false;
			}
		}
	}
}