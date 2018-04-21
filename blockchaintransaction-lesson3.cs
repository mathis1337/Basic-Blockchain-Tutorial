public class Block
{
	public int Index;
	public string Timestamp;
	public List<Transaction> Transactions = new List<Transaction>();
	public string Hash;
	public string PrevHash;
	public int Nonce {get;set;} = 0;
}

public class Blockchain
{
	public List<Block> Chain = new List<Block>();
	public int Difficulty {get; set;} = 2;
	public List<Transaction> PendingTransactions = new List<Transaction>();
	public int MiningReward {get; set;} = 100;
}

public class Transaction
{
	public string FromAddress;
	public string ToAddress;
	public decimal Amount;
}

Blockchain blockChain = new Blockchain();

void Main()
{

	//Create Genesis Block
	CreateGenesisBlock();
	
	var tran = new Transaction();
	
	tran.FromAddress = "Aaron";
	tran.ToAddress = "Taylor";
	tran.Amount = 5;
	
	CreateTransactions(tran);
	blockChain.Dump();
	
	MinePendingTransaction("Joe");
	
	//List Blockchain
	blockChain.Dump();
	
	//Blockchain Validity Check
	Console.WriteLine("Is Blockchain Valid? " + IsChainValid());



	//Second Blockchain Validity Checks
	Console.WriteLine("Is Blockchain Valid? " + IsChainValid());

}

public void CreateTransactions(Transaction trans)
{
	blockChain.PendingTransactions.Add(trans);
}

public void MinePendingTransaction(string rewardAddress)
{
	var block = new Block();
	block.Index = GetNextIndex();
	block.Timestamp = DateTime.Now.Ticks.ToString();
	block.PrevHash = blockChain.Chain[GetLatestBlock()].Hash;
	block.Transactions = blockChain.PendingTransactions;
	
	var minedBlock = MineBlock(block);
	
	blockChain.Chain.Add(minedBlock);
}

public Block MineBlock(Block block)
{
	var diff = blockChain.Difficulty;

	char[] diffChar = new char[1];

	diffChar[0] = '0';

	var alpha = new StringBuilder();

	var diffVar = alpha.Append(diffChar[0], diff).ToString();

	var jsonSerialiser = new JavaScriptSerializer();
	var jsonTranList = jsonSerialiser.Serialize(block.Transactions);

	var hashString = block.Index.ToString() + block.Timestamp + jsonTranList + block.PrevHash + block.Nonce;

	var hash = CalculateHash(hashString);

	block.Hash = hash;

	while (block.Hash.Substring(0, diff) != diffVar)
	{
		block.Nonce += 1;

		hashString = block.Index.ToString() + block.Timestamp + jsonTranList + block.PrevHash + block.Nonce;
		
		hash = CalculateHash(hashString);

		block.Hash = hash;
	}
	
	return block;
}

public int GetLatestBlock()
{
	return blockChain.Chain.Count() - 1;
}

public int GetNextIndex()
{
	return blockChain.Chain.Count();
}

public void CreateGenesisBlock()
{
	//Genesis Block
	var block0 = new Block();
	block0.Index = 0;
	block0.Timestamp = DateTime.Now.Ticks.ToString();
	block0.PrevHash = "";
	

	var hashString = block0.Index.ToString() + block0.Timestamp + block0.PrevHash;


	var hash = CalculateHash(hashString);

	block0.Hash = hash;

	blockChain.Chain.Add(block0);
}

public bool IsChainValid()
{
	var result = true;
	
	blockChain.Chain.Where(x => x.Index != 0).ToList().ForEach(x => {
		var currentBlock = x;
		var previousBlock = blockChain.Chain[x.Index - 1];

		var minedBlocked = MineBlock(x);

		if (result != false)
		{

			if (currentBlock.Hash != minedBlocked.Hash)
			{
				result = false;
			}
			
			if(currentBlock.PrevHash != previousBlock.Hash)
			{
				result = false;
			}

		}

	});
	
	return result;
}

public static string CalculateHash(string inputString)
{
	SHA256 sha256 = SHA256Managed.Create();
	byte[] bytes = Encoding.UTF8.GetBytes(inputString);
	byte[] hash = sha256.ComputeHash(bytes);
	return GetStringFromHash(hash);
}

private static string GetStringFromHash(byte[] hash)
{
	StringBuilder result = new StringBuilder();
	for (int i = 0; i < hash.Length; i++)
	{
		result.Append(hash[i].ToString("X2"));
	}
	return result.ToString();
}
