using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheKitchen.Data.Entities
{
    public class Chef
    {
		public int Id { get; set; }
		public int KitchenId { get; set; }
		public string FName { get; set; }
		public string MName { get; set; }
		public string LName { get; set; }
		public DateTime? HideDate { get; set; }
		public int ExpirienceYears { get; set; }
		public ChefDetail ChefDetail { get; set; }
		public override string ToString() =>
			$"[Id={Id}, KitchenId={KitchenId}, FName={FName}, MName={MName}, LName={LName}, HideDate={HideDate}, ExpirienceYears={ExpirienceYears}, ChefDetail={(ChefDetail != null ? ChefDetail.ToString() : "-")}]";
    }
}
