using ProductApi.Domain.Entities;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class ProductConversion
    {
        public static Product ToEntity(ProductDTO product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Quantity = product.Quantity,
            Price = product.Price,
        };

        public static (ProductDTO?, IEnumerable<ProductDTO?>) FromEntity(Product product, IEnumerable<Product>? products)
        {
            //return single
            if (product is null || products is null)
            {
                var singleProduct = new ProductDTO
                    (
                    product!.Id,
                    product.Name!,
                    product.Quantity,
                    product.Price
                    );

                return (singleProduct, null);
            }

            if (product is not null || product is null)
            {
                var _products = products.Select(pDTO =>
                    new ProductDTO(pDTO.Id, pDTO.Name, pDTO.Quantity, pDTO.Price)).ToList();

                return (null, _products);
            }

            return (null, null);
        }
    }
}
