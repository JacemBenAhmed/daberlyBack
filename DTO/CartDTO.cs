namespace DaberlyProjet.DTO
{
    public class CartDTO
    {
        public int UserId { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
    }

    public class CartItemDTO
    {
        public int ProduitPointureCouleurId { get; set; }
        public int Quantity { get; set; }
    }

}
