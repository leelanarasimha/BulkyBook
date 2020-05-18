using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Utility.Extensions
{
	public static class SessionExtension
	{
		public static void SetObject(this ISession Session, string key, object value) {
			Session.SetString(key, JsonConvert.SerializeObject(value));
		}

		public static T GetObject<T>(this ISession session, string Key) {
			var value = session.GetString(Key);

			return value == null ? default : JsonConvert.DeserializeObject<T>(value);
		} 
	}
}
