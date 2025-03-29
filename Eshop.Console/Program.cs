using Eshop.Domain;

Console.WriteLine("CATEGORIES");
var categories = new List<Category>
{
    new Category(1, "Computers"),
    new Category(2, "Mouses")
};
foreach (var category in categories)
{
    Console.WriteLine($"{category.Id} | {category.Name}");
}
Console.WriteLine("---");

Console.WriteLine("PRODUCTS");
var products = new List<Product>
{
    new Product(1, "Notebook Acer 15", "Best notebook out there", 399.99m, categories[0]),
    new Product(2, "Mouse Razor 123", "Best Mouse", 14.50m, categories[1])
};
foreach (var product in products)
{
    Console.WriteLine($"{product.Id} | {product.Title} | {product.Description} | {product.Price} | {product.Category?.Name}");
}
Console.WriteLine("---");

// Orders
Console.WriteLine("ORDERS");
var order = new Order(1, "Anton", "Street 1");
order.AddItem(new OrderItem(1, 1.23m, products[0]));
order.AddItem(new OrderItem(2, 4.56m, products[1]));
Console.WriteLine($"{order.Id} | {order.Customer} | {order.Address} | {order.CreatedAt}");
Console.WriteLine("---");

Console.WriteLine("ORDER ITEMS");
foreach (var orderItem in order.Items)
{
    Console.WriteLine($"{orderItem.Quantity} | {orderItem.Price} | {orderItem.Product.Title}");
}
Console.WriteLine("---");