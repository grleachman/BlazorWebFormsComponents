using System.Linq;
using System.Reflection;

namespace BlazorWebFormsComponents
{
	/// <summary>
	/// The GridView Column Generator
	/// </summary>
	public static class GridViewColumnGenerator
	{
		/// Generate columns for a given GridView based on the properties of given Type
		/// </summary>
		/// <typeparam name="ItemType"> The type </typeparam>
		/// <param name="gridView"> The GridView </param>
		public static void GenerateColumns<ItemType>(GridView<ItemType> gridView)
		{
			if (!gridView.AutogenerateColumns)
			{
				return;
			}
			var type = typeof(ItemType);
			var propertiesInfo = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).OrderBy(x => x.MetadataToken);
			foreach (var propertyInfo in propertiesInfo)
			{
				var newColumn = new BoundField
				{
					DataField = propertyInfo.Name,
					HeaderText = propertyInfo.Name
				};
				gridView.AddColumn(newColumn);
			}
		}
	}
}
