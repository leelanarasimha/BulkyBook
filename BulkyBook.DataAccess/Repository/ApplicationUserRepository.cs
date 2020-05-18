using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
	public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
	{
		private readonly ApplicationDbContext _db;
		public ApplicationUserRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;

		}

		public IEnumerable<ApplicationUser> getUserWithRole()
		{
			var users = this.GetAll(includeProperties: "Company");
			var userRole = _db.UserRoles.ToList();
			var roles = _db.Roles.ToList();

			foreach (var user in users)
			{
				var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
				user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
				if (user.Company == null) {
					user.Company = new Company()
					{
						Name = ""
					};
				}
			}

			return users;
		}

		public ApplicationUser GetUserById(string id) {
			return _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
		}

		public void Update(ApplicationUser user) {
			_db.Update(user);
		}
	}
}
