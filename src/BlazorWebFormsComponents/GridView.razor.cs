using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace BlazorWebFormsComponents
{
	/// <summary>
	/// Blazor version of WebForms GridView control
	/// </summary>
	/// <typeparam name="ItemType"></typeparam>
	public partial class GridView<ItemType> : ListView<ItemType>
	{

		/// <summary>
		///	Specify if the GridView component will autogenerate its columns
		/// </summary>
		[Parameter] public bool AutogenerateColumns { get; set; } = true;

		[Parameter] public RenderFragment<List<BaseColumn>> Columns { get; set; }

		public List<BoundField> ColumnsList { get; set; } = new List<BoundField>();

		public IEnumerable<string> HeaderRow()
		{
			foreach (var x in ColumnsList)
			{
				yield return x.HeaderText;
			}
		}

		/// <summary>
		/// Text to show when there are no items to show
		/// </summary>
		[Parameter] public string EmptyDataText { get; set; }

		/// <summary>
		/// Not supported yet
		/// </summary>
		[Parameter] public string DataKeyNames { get; set; }

		/// <summary>
		/// The css class of the GridView
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		///<inheritdoc/>

		#region Templates
		#endregion
		protected override void OnInitialized()
		{
			base.OnInitialized();
			GridViewColumnGenerator.GenerateColumns(this);



		}
	}
}
