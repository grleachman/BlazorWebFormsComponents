namespace BlazorWebFormsComponents.System.Web.Util
{
	//Done
	public static class SR
	{

		

		internal const string TableCell_ColumnSpan = "TableCell_ColumnSpan";
		internal const string TableCell_AssociatedHeaderCellID= "TableCell_AssociatedHeaderCellID";
		internal const string DataControlField_CallbacksNotSupported = "DataControlField_CallbacksNotSupported";
		internal const string DataControlField_Visible = "DataControlField_InsertVisible";
		internal const string DataControlField_SortExpression = "DataControlField_SortExpression";
		internal const string DataControlField_ShowHeader = "DataControlField_ShowHeader";
		internal const string DataControlField_ItemStyle = "DataControlField_ItemStyle";
		internal const string DataControlField_InsertVisible = "DataControlField_InsertVisible";
		internal const string DataControlField_HeaderText = "DataControlField_HeaderText";
		internal const string DataControlField_HeaderStyle = "DataControlField_HeaderStyle";
		internal const string DataControlField_HeaderImageUrl = "DataControlField_HeaderImageUrl";
		internal const string DataControlField_FooterText = "DataControlField_FooterText";
		internal const string DataControlField_FooterStyle = "DataControlField_FooterStyle";
		internal const string DataControlField_ControlStyle = "DataControlField_ControlStyle";
		internal const string DataControlField_AccessibleHeaderText = "DataControlField_AccessibleHeaderText";
		internal static string Parameter_Invalid = "Parameter_Invalid";
		internal static string Parameter_NullOrEmpty = "parameter_NullOrEmpty";
		internal static string Property_Invalid = "Property_Invalid";
		internal static string Property_NullOrEmpty = "Property_NullOrEmpty";
		internal static string Unexpected_Error = "Unexpected_Error";

		public static string GetString(string message, string parameter = "")
		=> $"{message}, {parameter}";
	}
}
