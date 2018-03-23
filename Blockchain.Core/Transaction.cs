using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Core
{
	public class Transaction
	{
		public string Sender { get; set; }
		public string Recipient { get; set; }
		public decimal Amount { get; set; }
	}
}
