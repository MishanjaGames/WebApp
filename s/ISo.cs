namespace WebApp
{
    public interface ISo
    {
        Task<Order> PlaceOrderAsync(string userId, List<CartItem> cartItems);
    }
    public class So : ISo
    {
        private readonly ProductContext _productContext;
        public So(ProductContext productContext)
        {
            _productContext = productContext;
        }

        public async Task<Order> PlaceOrderAsync(string userId, List<CartItem> cartItems)
        {
            var newOrder = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderDetails = new List<OrderDetail>()
            };
            decimal? totalOrderAmount = 0;
            foreach (var item in cartItems)
            {
                var product = await _productContext.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    throw new Exception("Product Not Found");
                }
                var detail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                };
                newOrder.OrderDetails.Add(detail);
                totalOrderAmount += item.Quantity * item.Price;

                _productContext.Orders.Add(newOrder);
                await _productContext.SaveChangesAsync();
            }
            return newOrder;
        }
    }
}
