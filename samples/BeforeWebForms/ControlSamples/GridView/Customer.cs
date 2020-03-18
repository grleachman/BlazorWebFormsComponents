using System.ComponentModel.DataAnnotations;

namespace BeforeWebForms.ControlSamples.GridView
{
	public class Customer
	{
		[Display(Name = "Id", Order = 7)]
		public int CustomerID { get; set; }

		[Display(Name = "FirstName", Order = 6)]

		public string FirstName { get; set; }
		[Display(Name = "LastName", Order = 3)]

		public string LastName { get; set; }

		[Display(Name = "Company", Order = 5)]
		public string CompanyName { get; set; }
	}
}
