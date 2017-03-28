using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTreeApp
{
	class Program
	{
		static void Main(string[] args)
		{
			BTree<int> bTree = new BTree<int>(2);
			bTree.Insert(1);
			bTree.Insert(3);
			bTree.Insert(2);
			bTree.Insert(5);
			bTree.Insert(6);
			bTree.Insert(7);
			//Console.WriteLine(bOk);
			Console.ReadKey();
		}
	}
}
