namespace CodesDictionary.Api.Infrastructure.Persistence
{
	public class MedicineCode
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string UnitName { get; set; }
	}
}