using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Blockchain.Core
{
	public class Block
	{
		public int Index { get; set; }
		public string Timestamp { get; set; }
		public List<Transaction> Transactions { get; set; }
		public int Proof { get; set; }
		public string PreviousHash { get; set; }

		public Block()
		{
			this.Transactions = new List<Transaction>();
		}

		public override string ToString()
		{
			//return base.ToString();
			return JsonConvert.SerializeObject(this);
		}
	}
}
