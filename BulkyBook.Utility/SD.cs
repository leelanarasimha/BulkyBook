﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Utility
{
	public static class SD
	{
		public const string Proc_CoverType_Create = "usp_CreateCoverType";
		public const string Proc_CoverType_Get = "usp_GetCoverType";
		public const string Proc_CoverType_GetAll = "usp_GetCoverTypes";
		public const string Proc_CoverType_Update = "usp_UpdateCoverType";
		public const string Proc_CoverType_Delete = "usp_DeleteCoverType";



		public const string Role_User_Indi = "Individual Customer";
		public const string Role_User_Comp = "Company Customer";
		public const string Role_Admin = "Admin";
		public const string Role_Employee = "Employee";

		public const string ssShoppingCart = "Shopping Cart Session";

		public static double GetPriceBasedQuantity(double Quantity, double Price, double Price50, double Price100)
		{
			if (Quantity < 50)
			{
				return Price;
			}
			else
			{
				if (Quantity < 50)
				{
					return Price50;
				}
				else
				{
					return Price100;
				}
			}

		}
	}
}
