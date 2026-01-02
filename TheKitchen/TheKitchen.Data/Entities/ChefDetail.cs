using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheKitchen.Data.Entities
{
    public class ChefDetail
    {
		public string PhoneNumber { get; set; }
		public string PasspordNumber { get; set; }
		public DateTime? BirthDate { get; set; }
		public string Country { get; set; }
		public string Region { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string House { get; set; }
		public string Apartment { get; set; }
		public string PostalCode { get; set; }
        public string Notes { get; set; }
		public override string ToString() =>
			$"[PhoneNumber={PhoneNumber}, " +
			$"PasspordNumber={PasspordNumber}, " +
			$"BirthDate={BirthDate?.ToString("dd.MM.yyyy") ?? "—"}, " +
			$"Country={Country}, " +
			$"Region={Region}, " +
			$"City={City}, " +
			$"Street={Street}, " +
			$"House={House}, " +
			$"Apartment={Apartment}, " +
			$"PostalCode={PostalCode}, " +
			$"Notes={Notes}]";
    }
}
