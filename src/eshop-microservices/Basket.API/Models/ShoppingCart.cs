namespace Basket.API.Models;

public class ShoppingCart
{
    public string Username { get; set; } = string.Empty;
    public List<ShoppingCartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);

    public ShoppingCart(string username)
    {
        Username = username;
    }
    
    public ShoppingCart()
    {
    }
}