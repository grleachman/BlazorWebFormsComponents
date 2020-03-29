using System.Diagnostics.CodeAnalysis;

namespace BlazorWebFormsComponents.System.Drawing.Design
{
	[SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	public enum UITypeEditorEditStyle
	{
		/// <include file='doc\UITypeEditorEditStyle.uex' path='docs/doc[@for="UITypeEditorEditStyle.None"]/*' />
		/// <devdoc>
		///    <para>
		///       Indicates no editor style.
		///    </para>
		/// </devdoc>
		None = 1,
		/// <include file='doc\UITypeEditorEditStyle.uex' path='docs/doc[@for="UITypeEditorEditStyle.Modal"]/*' />
		/// <devdoc>
		///    <para>
		///       Indicates a modal editor style.
		///    </para>
		/// </devdoc>
		Modal = 2,
		/// <include file='doc\UITypeEditorEditStyle.uex' path='docs/doc[@for="UITypeEditorEditStyle.DropDown"]/*' />
		/// <devdoc>
		///    <para> Indicates a drop-down editor style.</para>
		/// </devdoc>
		DropDown = 3
	}
}
