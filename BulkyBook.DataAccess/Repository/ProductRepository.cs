using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		private readonly ApplicationDbContext _db;

		public ProductRepository(ApplicationDbContext db) : base(db)
		{
			this._db = db;
		}
		public void Update(Product Product)
		{
			var objFromDb = this._db.Products.FirstOrDefault(p => p.Id == Product.Id);
			if (objFromDb != null) {

				if (Product.ImageUrl != null) {
					objFromDb.ImageUrl = Product.ImageUrl;
				}
				objFromDb.Title = Product.Title;
				objFromDb.Description = Product.Description;
				objFromDb.CategoryId = Product.CategoryId;
				objFromDb.Author = Product.Author;
				objFromDb.CoverTypeId = Product.CoverTypeId;
				
				objFromDb.ISBN = Product.ISBN;
				objFromDb.ListPrice = Product.ListPrice;
				objFromDb.Price = Product.Price;
				objFromDb.Price100 = Product.Price100;
				objFromDb.Price50 = Product.Price50;
			}

		}
	}
}
