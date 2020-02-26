using SharedSampleObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BeforeWebForms.ControlSamples.ListViewItem
{
  public partial class Default : System.Web.UI.Page
  {
		private List<Widget> widgets { get; set; }
		protected void simpleListView_ItemCreated(object sender, ListViewItemEventArgs e)
		{
			var item = e.Item;
			if (item.ItemType == ListViewItemType.DataItem)
			{
				var nameLabel = (Label)item.FindControl("NameLabel");
				nameLabel.Font.Italic = true;
			}
		}
		protected void Page_Load(object sender, EventArgs e)
    {
			widgets = Widget.SimpleWidgetList.ToList(); 
			simpleListView.DataSource = widgets;
			simpleListView.ItemInserting += simpleListView_ItemInserting;
			simpleListView.ItemCreated += simpleListView_ItemCreated;
			simpleListView.DataBind();

    }

		private void simpleListView_ItemInserting(object sender, ListViewInsertEventArgs e)
		{
			//foreach (IOrderedDictionary<Widget> de in e.Values)
			//{
			//	if (de.Value == null)
			//	{
			//		Message.Text = "Cannot insert an empty value.";
			//		e.Cancel = true;
			//	}
			//}
		}
	}
}
