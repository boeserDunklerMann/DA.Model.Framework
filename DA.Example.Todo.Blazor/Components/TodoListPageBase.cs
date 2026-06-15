using DA.Example.Todo.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace DA.Example.Todo.Blazor.Components
{
	/// <ChangeLog>
	/// <Create Datum="15.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// Base class for razor's codebehind classes
	/// </summary>
	public class TodoListPageBase : ComponentBase
	{
		[Inject]
		protected IConfiguration? Configuration { get; set; } = default;
		[Inject]
		protected IDbContextFactory<TodoListContext> DbContextFactory { get; set; } = default!;
		protected EditContext? editContext;
		protected bool Loading { get; set; } = false;
	}
}
