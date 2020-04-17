using BlazorWebFormsComponents.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BlazorWebFormsComponents
{
	public abstract class BaseColumn : BaseWebFormsComponent
	{
		[Parameter] public string HeaderText { get; set; }


	}
}
