﻿using Microsoft.AspNetCore.Components;

namespace BlazorWebFormsComponents
{
	/// <summary>
	/// A template field column
	/// </summary>
	public partial class TemplateField : BaseColumn
	{
		/// <summary>
		/// The item template
		/// </summary>
		[Parameter] public RenderFragment ItemTemplate { get; set; }
	}
}
