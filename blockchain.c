public class Block
{
	public int Index;
	public string Timestamp;
	public string Data;
	public string Hash;
	public string PrevHash;
}

public class Blockchain
{
	public List<Block> Chain = new List<Block>();
}

Blockchain blockChain = new Blockchain();

void Main()
{

	//Create Genesis Block
	CreateGenesisBlock();
	
	
	
	AddBlock(DateTime.Now.Ticks.ToString(), blockChain.Chain[GetLatestBlock()].Hash, "A");
	AddBlock(DateTime.Now.Ticks.ToString(), blockChain.Chain[GetLatestBlock()].Hash, "B");
	AddBlock(DateTime.Now.Ticks.ToString(), blockChain.Chain[GetLatestBlock()].Hash, "C");
	
	//List Blockchain
	blockChain.Dump();
	
	//Blockchain Validity Check
	Console.WriteLine("Is Blockchain Valid? " + IsChainValid());

	//Modify Blockchain
	blockChain.Chain[1].Data = "B";

	//Second Blockchain Validity Checks
	Console.WriteLine("Is Blockchain Valid? " + IsChainValid());

}

public void AddBlock(string timestamp, string prevHash, string data)
{
	var block = new Block();
	block.Index = GetNextIndex();
	block.Timestamp = timestamp;
	block.PrevHash = prevHash;
	block.Data = data;

	var hashString = block.Index.ToString() + block.Timestamp + block.Data + block.PrevHash;


	var hash = CalculateHash(hashString);

	block.Hash = hash;

	blockChain.Chain.Add(block);
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
	block0.Data = "Genesis Block";

	var hashString = block0.Index.ToString() + block0.Timestamp + block0.Data + block0.PrevHash;


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

		var hashString = currentBlock.Index.ToString() + currentBlock.Timestamp + currentBlock.Data + currentBlock.PrevHash;

		var hash = CalculateHash(hashString);

		if (result != false)
		{

			if (currentBlock.Hash != hash)
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


// Define other methods and classes here
