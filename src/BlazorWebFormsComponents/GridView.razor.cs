using BlazorWebFormsComponents.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BlazorWebFormsComponents
{
	/// <summary>
	/// Blazor version of WebForms GridView control
	/// </summary>
	/// <typeparam name="ItemType"></typeparam>
	public partial class GridView<ItemType> : ListView<ItemType>, IColumnCollection
	{

		/// <summary>
		///	Specify if the GridView component will autogenerate its columns
		/// </summary>
		[Parameter] public bool AutogenerateColumns { get; set; } = true;

		[Parameter] public RenderFragment Columns { get; set; }

		public IEnumerable<string> HeaderRow()
		{
			foreach (var x in ColumnList)
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
		public List<BoundField> ColumnList { get; set; } = new List<BoundField>();

		protected override void OnInitialized()
		{
			base.OnInitialized();
			GridViewColumnGenerator.GenerateColumns(this);
			if (Items == null)
			{
				Items = new List<ItemType>();
			}
		}

		public void AddColumn(BoundField column)
		{
			ColumnList.Add(column);
			StateHasChanged();
		}
	}
}
