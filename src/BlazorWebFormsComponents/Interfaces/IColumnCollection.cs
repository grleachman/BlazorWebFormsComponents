using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorWebFormsComponents.Interfaces
{
	public interface IColumnCollection
	{
		List<BoundField> ColumnList { get; set; }

		void AddColumn(BoundField column);

	}
}
