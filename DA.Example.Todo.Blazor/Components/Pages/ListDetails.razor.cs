using DA.Example.Todo.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DA.Example.Todo.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="15.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public partial class ListDetails:IDisposable
	{
		[Parameter]
		public int ListID { get; set; }
		[Inject]
		public IConfiguration? Configuration { get; set; } = default;
		private TodoListContext? context;
		private bool Loading { get; set; } = false;
		private List<TodoListItem> Items { get; set; } = [];

		#region IDisposable stuff
		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					if (context != null)
					{
						context.Dispose();
						context = null;
					}
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~Home_Code()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		#endregion

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			if (context==null)
			{
				context = new TodoListContext(Configuration!);
			}
			//if (!Loading)
			//{
			//	try
			//	{
			//		Loading = true;
			//		Items = await context.TodoListItems.Where(i => i.List!.ID == ListID).ToListAsync();
			//	}
			//	finally
			//	{
			//		Loading = false;
			//	}
			//}
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
		private async Task OnCheckedChangedAsync(TodoListItem item, bool newValue)
		{
			if (context == null)
				throw new NullReferenceException(nameof(context));
			item.CheckedDate = DateTime.UtcNow;
			await context.SaveChangesAsync();
			StateHasChanged();
		}
	}
}