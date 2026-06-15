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
		public TodoListItem NewItem { get; set; } = default!;

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			if (!Loading)
			{
				try
				{
					Loading = true;
					using var context = await DbContextFactory.CreateDbContextAsync();
					currentList = await context.TodoLists.Where(l => l.ID == ListID).FirstAsync();
					Items = await context.TodoListItems.Where(i => i.List!.ID == ListID).ToListAsync();
					NewItem = BaseModel.Create<TodoListItem>("");
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
			if (newValue)
				item.CheckedDate = DateTime.UtcNow;
			else
				item.CheckedDate = null;

			using var context = await DbContextFactory.CreateDbContextAsync();
			await context.SaveChangesAsync();
			StateHasChanged();
		}
		public async Task OnNewItemSubmittedAsync()
		{
			if (Loading)
				return;

			try
			{
				Loading = true;
				using var context = await DbContextFactory.CreateDbContextAsync();
				NewItem.List = currentList;
				context.TodoLists.Attach(currentList!);	// we have a new context, which doesn't know anything about currentList and assumes that this must be created again,
														// so primary key constraint exception will raise while saving
				await context.TodoListItems.AddAsync(NewItem);
				await context.SaveChangesAsync();
				Items.Add(NewItem);
				NewItem = BaseModel.Create<TodoListItem>("");
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