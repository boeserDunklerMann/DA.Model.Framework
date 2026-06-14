namespace DA.Model.Framework.Contracts
{
	/// <ChangeLog>
	/// <Create Datum="14.06.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal interface ICurrentTimestamps
	{
		/// <summary>
		/// Änderungsdatum
		/// </summary>
		DateTimeOffset? ChangeDate { get; set; }

		/// <summary>
		/// Erstelldatum
		/// </summary>
		DateTimeOffset CreationDate { get; set; }
	}
}