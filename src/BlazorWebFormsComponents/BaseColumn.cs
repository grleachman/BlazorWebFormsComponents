using BlazorWebFormsComponents.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BlazorWebFormsComponents
{
	public abstract class BaseColumn : BaseWebFormsComponent
	{
		///<inheritdoc/>
		[Parameter] public string HeaderText { get; set; }
	}
}
