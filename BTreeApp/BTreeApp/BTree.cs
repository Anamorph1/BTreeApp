using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTreeApp.Concrete;
using BTreeApp.Abstract;

namespace BTreeApp
{

	public class BTree<T> : ITree<T>
	{
		Node<T> root;
		int height;
		public readonly int size;

		#region Constructors/Destructors
		public BTree(int n)
		{
			size = 2 * n - 1; //Stopień B-drzewa
			root = AllocateNode(NodeType.Root);
			height = 0;
		}
		#endregion

		#region Properties
		public int Height
		{
			get
			{
				return height;
			}
		}
		#endregion

		#region Methods
		private Node<T> AllocateNode(NodeType type, Node<T> root = null)
		{
			if (type == NodeType.Leaf)
			{
				return new Node<T>(size, root, true);
			}
			else if (type == NodeType.Inner)
			{
				return new Node<T>(size, root);
			}
			else if (type == NodeType.Root)
			{
				++height;
				if (this.root == null)
				{
					return new Node<T>(size, null, true);
				}
				else
				{
					return new Node<T>(size);
				}
			}
			else
			{
				throw new ArgumentException("Invalid Node type");
			}
		}

		public bool IsExist(T v)
		{
			return Search(v).Equals(default(T)) ? false : true;
		}
		#endregion

		#region ITree<T>
		public bool Insert(T v)
		{
			if (IsExist(v))
			{
				return false;
			}

			if (root.IsFull)
			{
				Node<T> r = root;
				root = AllocateNode(NodeType.Root);
				r.SetRoot(root);
				r.SplitChild(v);
				return root.InsertNonFull(v);
			}
			else
			{
				return root.InsertNonFull(v);
			}
		}

		public bool Delete(T key)
		{
			throw new NotImplementedException();
		}

		public T Search(T key)
		{
			return default(T);
		}
		#endregion
	}
}
