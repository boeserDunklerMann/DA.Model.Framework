using DA.Model.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.Example.Todo.Model
{
	/// <ChangeLog>
	/// <Create Datum="14.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class TodoListItem : BaseModel
	{
		public DateTimeOffset? CheckedDate { get; set; }

		[NotMapped]
		public bool Checked => CheckedDate < DateTime.Now;
		public virtual	TodoList? List { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is TodoListItem)) return false;
			return ID == ((TodoListItem)obj).ID;
		}
		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}
	}
}