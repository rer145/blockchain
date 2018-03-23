﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Blockchain.Core;

namespace Blockchain.Example
{
	class Program
	{
		static void Main(string[] args)
		{
			Transaction transaction1 = new Transaction()
			{
				Sender = "user1",
				Recipient = "user2",
				Amount = 69.0M
			};
			Transaction transaction2 = new Transaction()
			{
				Sender = "user1",
				Recipient = "user3",
				Amount = 100.0M
			};
			Transaction transaction3 = new Transaction()
			{
				Sender = "user2",
				Recipient = "user3",
				Amount = 15.5M
			};
			Transaction transaction4 = new Transaction()
			{
				Sender = "user3",
				Recipient = "user1",
				Amount = 20.8M
			};

			int index = -1;
			Console.WriteLine("Starting Blockchain");
			Core.Blockchain blockchain = new Core.Blockchain();

			Console.WriteLine("Mining...");
			Block block1 = blockchain.Mine();
			Console.WriteLine(block1.ToString());
			Console.WriteLine();

			Console.WriteLine("Mining...");
			Block block2 = blockchain.Mine();
			Console.WriteLine(block2.ToString());
			Console.WriteLine();

			Console.WriteLine("Post New Transaction...");
			index = blockchain.NewTransaction(transaction1);
			Console.WriteLine(String.Format("Transaction1 added [block index: {0}]", index));
			Console.WriteLine();

			Console.WriteLine("Post New Transaction...");
			index = blockchain.NewTransaction(transaction2);
			Console.WriteLine(String.Format("Transaction2 added [block index: {0}]", index));
			Console.WriteLine();

			Console.WriteLine("Post New Transaction...");
			index = blockchain.NewTransaction(transaction3);
			Console.WriteLine(String.Format("Transaction3 added [block index: {0}]", index));
			Console.WriteLine();

			Console.WriteLine("Mining...");
			Block block3 = blockchain.Mine();
			Console.WriteLine(block3.ToString());
			Console.WriteLine();

			Console.WriteLine("Current Chain:");
			Console.WriteLine(blockchain.ToString());

			Console.WriteLine("done");
			Console.ReadLine();
		}
	}
}
