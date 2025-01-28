namespace DaberlyProjet.DTO
{
    public class OrderDTO
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public class OrderItemDTO
    {
        public int ProduitPointureCouleurId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
