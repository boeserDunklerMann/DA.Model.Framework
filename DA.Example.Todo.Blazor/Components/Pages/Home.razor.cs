using DA.Example.Todo.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DA.Example.Todo.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="15.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public partial class Home_Code : ComponentBase, IDisposable
	{
		[Inject]
		public IConfiguration? configuration { get; set; } = default;

		protected bool Loading { get; set; } = false;
		private TodoListContext? context;
		public List<TodoList>? TodoLists { get; private set; }

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
				context = new TodoListContext(configuration!);
			}
			if (!Loading)
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
