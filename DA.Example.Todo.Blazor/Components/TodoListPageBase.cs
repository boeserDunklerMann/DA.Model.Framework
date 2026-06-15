using DA.Example.Todo.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DA.Example.Todo.Blazor.Components
{
	/// <ChangeLog>
	/// <Create Datum="15.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// Base class for razor's codebehind classes
	/// </summary>
	public class TodoListPageBase : ComponentBase, IDisposable
	{
		[Inject]
		protected IConfiguration? Configuration { get; set; } = default;
		protected TodoListContext? context;
		protected EditContext? editContext;
		protected bool Loading { get; set; } = false;

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
		// ~TodoListPageBase()
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
			if (context == null)
			{
				context = new TodoListContext(Configuration!);
			}
		}
	}
}
