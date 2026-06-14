using DA.Model.Framework;
using System.Text.Json.Serialization;

namespace DA.Example.Todo.Model
{
	/// <ChangeLog>
	/// <Create Datum="14.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class TodoList : BaseModel
	{
		[JsonIgnore]
		public virtual ICollection<TodoListItem>? Items { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is TodoList)) return false;
			return ID == ((TodoList)obj).ID;
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}
	}
}