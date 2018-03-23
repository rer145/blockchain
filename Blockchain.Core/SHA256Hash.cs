using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Blockchain.Core
{
	public static class SHA256Hash
	{
		public static string Get(string item)
		{
			SHA256 sha = SHA256Managed.Create();
			byte[] bytes = Encoding.UTF8.GetBytes(item);
			byte[] hash = sha.ComputeHash(bytes);

			StringBuilder result = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				result.Append(hash[i].ToString("X2"));
			}
			return result.ToString();
		}
	}
}
