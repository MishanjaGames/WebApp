using Microsoft.EntityFrameworkCore;

namespace WebApp
{
    public interface ISi
    {
        Itm? GetById(int id);
        Task<Itm?> CreateA(Itm? product); // Асинхронний метод створення продукту
        Task<IEnumerable<Itm?>> ReadA(); // Асинхронний метод отримання всіх продуктів
        Task<Itm?> GetByIdA(int id); // Асинхронний метод отримання продукту з його ID
        Task<Itm?> UpdateA(int id, Itm? product); // Асинхронний метод оновлення продукту
        Task<bool> DeleteA(int id); // Асинхронний метод видалення продукту по ID
    }
    public class Si : ISi
    {
        private readonly iCntx _c;

        public Si(iCntx context)
        {
            _c = context;
        }

        public async Task<Itm?> CreateA(Itm? product)
        {
            if (product == null)
                return null;

            _c.Items.Add(product);
            await _c.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteA(int id)
        {
            var product = await _c.Items.FindAsync(id);
            if (product == null)
                return false;

            _c.Items.Remove(product);
            await _c.SaveChangesAsync();
            return true;
        }

        public Itm? GetById(int id)
        {
            return _c.Items.Find(id);
        }

        public async Task<Itm?> GetByIdA(int id)
        {
            return await _c.Items.FindAsync(id);
        }

        public async Task<IEnumerable<Itm?>> ReadA()
        {
            return await _c.Items.ToListAsync();
        }

        public async Task<Itm?> UpdateA(int id, Itm? product)
        {
            if (product == null)
                return null;

            var existingProduct = await _c.Items.FindAsync(id);
            if (existingProduct == null)
                return null;

            existingProduct.Name = (product.Name==null)? existingProduct.Name : product.Name;
            existingProduct.Price = (product.Price == null) ? existingProduct.Price : product.Price;
            existingProduct.OldPrice = (product.OldPrice == null) ? existingProduct.OldPrice : product.OldPrice;
            existingProduct.Description = (product.Description == null) ? existingProduct.Description : product.Description;
            existingProduct.IsAvaliable = (product.IsAvaliable == null) ? existingProduct.IsAvaliable : product.IsAvaliable;
            existingProduct.Image = (product.Image == null) ? existingProduct.Image : product.Image;
            existingProduct.ImgFormat = (product.ImgFormat == null) ? existingProduct.ImgFormat : product.ImgFormat;

            await _c.SaveChangesAsync();
            return existingProduct;
        }
    }
}
