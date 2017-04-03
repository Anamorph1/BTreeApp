using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTreeApp.Abstract
{
	interface ITree<T>
	{
		bool Insert(T key);
		bool Delete(T key);
		T Search(T key);
	}
}
