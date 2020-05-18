using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork : IDisposable
	{
		ICategoryRepository Category { get; }
		ICoverTypeRepository CoverType { get; }
		IProductRepository Product { get; }
		IShoppingCartRepository ShoppingCart { get; }
		IOrderDetailsRepository OrderDetails{ get; }
		IOrderHeaderRepository OrderHeader{ get; }

		IApplicationUserRepository ApplicationUser { get; }

		ICompanyRepository Company { get; }
		ISP_Call SP_Call { get; }

		void Save();


	}
}
