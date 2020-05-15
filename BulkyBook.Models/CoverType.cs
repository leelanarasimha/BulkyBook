using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BulkyBook.Models
{
	public class CoverType
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		[DisplayName("Cover Type")]
		public string Name { get; set; }
	}
}
