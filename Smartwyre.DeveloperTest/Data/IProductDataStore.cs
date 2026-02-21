namespace Smartwyre.DeveloperTest.Data;

public interface IProductDataStore
{
    Product GetProduct(string productIdentifier);
}
