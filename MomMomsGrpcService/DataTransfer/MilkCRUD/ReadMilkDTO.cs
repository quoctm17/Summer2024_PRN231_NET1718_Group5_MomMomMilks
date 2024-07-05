namespace DataTransfer.MilkCRUD
{
	public class ReadMilkDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Quantity { get; set; }
		public float Price { get; set; }
		public string UserManual { get; set; }
		public string Warning { get; set; }
		public string PreserveInstructions { get; set; }
		public int BrandId { get; set; }
		public int BrandName { get; set; }
		public int CategoryId { get; set; }
		public int CategoryName { get; set; }
		public int MilkAgeId { get; set; }
		public int MilkAgeName { get; set; }
		public int SupplierId { get; set; }
		public int SupplierName { get; set; }
		public string ImageUrl { get; set; }
	}
}
