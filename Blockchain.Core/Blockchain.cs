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

		public Blockchain()
		{
			this.CurrentTransactions = new List<Transaction>();
			this.Chain = new List<Block>();

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
			//guess = f'{last_proof}{proof}'.encode()
			//guess_hash = hashlib.sha256(guess).hexdigest()
			//return guess_hash[:4] == "0000"

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

		public override string ToString()
		{
			//return base.ToString();
			return JsonConvert.SerializeObject(this);
		}
	}
}
