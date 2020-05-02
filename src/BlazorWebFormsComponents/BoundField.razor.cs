using BlazorWebFormsComponents.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BlazorWebFormsComponents
{
	/// <summary>
	/// Binds an object's property to a column by its property name 
	/// </summary>
	partial class BoundField : BaseColumn
	{
		[CascadingParameter(Name = "ColumnCollection")]
		public IColumnCollection ParentColumnsCollection { get; set; }
		/// <summary>
		/// Specifies the name of the object's property bound to the column
		/// </summary>
		[Parameter]
		public string DataField { get; set; }

		/// <summary>
		/// Specifies which string format should be used.
		/// </summary>
		[Parameter]
		public string DataFormatString { get; set; } = null;

		protected override void OnInitialized()
		{
			ParentColumnsCollection.AddColumn(this);
		}
	}
}
