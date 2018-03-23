using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain.Core
{
	public class Blockchain
	{
		public Block LastBlock
		{
			get { return this.Chain.LastOrDefault(); }
		}
		public List<Transaction> CurrentTransactions { get; set; }
		public List<Block> Chain { get; set; }
		public HashSet<Node> Nodes { get; set; }

		public Blockchain()
		{
			this.CurrentTransactions = new List<Transaction>();
			this.Chain = new List<Block>();
			this.Nodes = new HashSet<Node>();

			NewBlock(100, "1");
		}

		public Block NewBlock(int proof, string previousHash = null)
		{
			Block block = new Block()
			{
				Index = this.Chain.Count + 1,
				Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"),
				Transactions = this.CurrentTransactions,
				Proof = proof,
				PreviousHash = (previousHash == null) ? Hash(this.Chain.LastOrDefault()) : previousHash
			};

			this.CurrentTransactions = new List<Transaction>();
			this.Chain.Add(block);
			return block;
		}

		public int NewTransaction(Transaction transaction)
		{
			return NewTransaction(transaction.Sender, transaction.Recipient, transaction.Amount);
		}

		public int NewTransaction(string sender, string recipient, decimal amount)
		{
			if (this.CurrentTransactions == null)
			{
				this.CurrentTransactions = new List<Transaction>();
			}

			this.CurrentTransactions.Add(new Core.Transaction()
			{
				Sender = sender,
				Recipient = recipient,
				Amount = amount
			});

			return this.LastBlock.Index + 1;
		}

		public static string Hash(Block block)
		{
			return SHA256Hash.Get(block.ToString());
		}

		public int ProofOfWork(int lastProof)
		{
			int proof = 0;
			while (!Blockchain.ValidProof(lastProof, proof))
			{
				proof += 1;
			}
			return proof;
		}

		public static bool ValidProof(int lastProof, int currentProof)
		{
			string guess = lastProof.ToString() + currentProof.ToString();
			string guessHash = SHA256Hash.Get(guess);
			return guessHash.Substring(0, 4) == "0000";
		}

		public Block Mine()
		{
			int proof = ProofOfWork(this.LastBlock.Proof);

			NewTransaction("0", "-1", 1.0M);
			string previousHash = Hash(this.LastBlock);
			Block block = NewBlock(proof, previousHash);

			return block;
		}

		public void RegisterNode(Node node)
		{
			//validate node.Address?
			this.Nodes.Add(node);
		}

		public bool ValidChain()
		{
			return this.ValidChain(this.Chain);
		}

		public bool ValidChain(List<Block> chain)
		{
			Block tempBlock = this.Chain.FirstOrDefault();
			int currentIndex = 0;

			while (currentIndex < this.Chain.Count)
			{
				Block block = this.Chain[currentIndex];

				if (block.PreviousHash != Hash(tempBlock))
					return false;
				if (!ValidProof(tempBlock.Proof, block.Proof))
					return false;

				tempBlock = block;
				currentIndex += 1;
			}

			return true;
		}

		public bool ResolveConflicts()
		{
			HashSet<Node> neighbors = this.Nodes;
			List<Block> newChain = null;
			int maxLength = this.Chain.Count;

			foreach (Node node in neighbors)
			{
				//api get chain from node
				string response = node.Address + "/chain";
				if (!String.IsNullOrEmpty(response))
				{
					int length = 0; //response length
					List<Block> chain = null; //response chain

					if (length > maxLength && ValidChain(chain))
					{
						maxLength = length;
						newChain = chain;
					}
				}
			}

			//if we find a newer, longer chain, replace ours
			if (newChain != null)
			{
				this.Chain = newChain;
				return true;
			}

			return false;
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
