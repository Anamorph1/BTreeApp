using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTreeApp.Concrete
{

	public class Node<T>
	{
		Node<T> root = null;
		KeyChildList entry; //TODO Change property name

		#region Properties
		bool isLeaf = true;
		bool isFull = false;
		readonly int size;
		#endregion

		#region Getters/Setters
		public bool IsLeaf
		{
			get
			{
				return isLeaf;
			}
		}

		public bool IsFull
		{
			get
			{
				return isFull;
			}
		}

		public int Count
		{
			get
			{
				return entry.Count;
			}
		}
		#endregion

		#region Constructors/Destructors
		public Node(int size, Node<T> root = null, bool isLeaf = false)
		{
			this.size = size;
			this.isLeaf = isLeaf;
			this.root = root;
			entry = new KeyChildList();
		}
		#endregion

		#region Methods

		#region InsertNonFull()
		public bool InsertNonFull(T v)
		{
			if (isLeaf)
			{
				int iOk = entry.Insert(v);
				if (iOk != -1)
				{
					if (entry.Count == size)
					{
						isFull = true;
					}
					return true;
				}
				return false;
			}
			else
			{
				Node<T> n = entry.FindProperNode(v);
				if (n.isFull)
				{
					n.SplitChild(v);
				}
				return n.InsertNonFull(v);
			}
		}
		#endregion

		#region SplitChild()
		public void SplitChild(T v)
		{
			Node<T> leftSibling = AllocateNewNodeBasedOn();
			T key = ExtractMidKey();
			leftSibling.RewriteLeftHalfFrom(this);
			SendUpKeyToRoot(key, leftSibling, this);
		}

		private Node<T> AllocateNewNodeBasedOn()
		{
			return new Node<T>(size, root, isLeaf);
		}

		private T ExtractMidKey()
		{
			int index = entry.Count / 2;
			T key = entry[index];

			//Entry is not full anymore
			entry.Remove(key);
			isFull = false;

			return key;
		}

		private void RewriteLeftHalfFrom(Node<T> node)
		{
			int count = node.Count / 2;
			entry = node.entry.GetRange(0, count);
			node.entry.RemoveRange(0, count);
		}

		private void SendUpKeyToRoot(T key, Node<T> left, Node<T> right)
		{
			int iOk = root.entry.Insert(key);
			root.entry.SetChild(key, left, right);
		}
		#endregion

		internal void SetRoot(Node<T> node)
		{
			root = node;
		}

		#endregion

		#region KeyChildList
		class KeyChildList
		{
			List<T> keySet; //TODO opracować kolekcję
			List<Node<T>> childList;

			#region Constructors/Destructors

			public KeyChildList(List<T> keySet, List<Node<T>> childList)
			{
				this.keySet = keySet;
				this.childList = childList;
			}

			public KeyChildList()
			{
				keySet = new List<T>();
				childList = new List<Node<T>>() { null };
			}

			#endregion

			#region Getters/Setters
			public int Count
			{
				get
				{
					return keySet.Count;
				}
			}

			#endregion

			#region Indexers
			public T this[int i]
			{
				get
				{
					return keySet[i];
				}
			}


			#endregion

			#region Methods

			internal int Insert(T v)
			{
				//TODO Poprawić wstawianie
				if (keySet.Contains(v))
				{
					return -1;
				}
				//Add new key in specific place
				//Set new key's left/right children to null
				// 1 2		->		1 * 2
				// 1 2 3	->		1 n n 3
				keySet.Add(v);
				keySet.Sort();
				int index = keySet.IndexOf(v);
				childList.Insert(index, null);
				childList[index + 1] = null;

				return index;


			}

			public void Remove(T v)
			{
				keySet.Remove(v);
			}

			public KeyChildList GetRange(int index, int count)
			{
				return new KeyChildList(keySet.GetRange(index, count), childList.GetRange(index, count + 1));
			}

			public void RemoveRange(int index, int count)
			{
				keySet.RemoveRange(index, count);
				childList.RemoveRange(index, count + 1);
			}

			internal void SetChild(T key, Node<T> left, Node<T> right)
			{
				int index = keySet.IndexOf(key);
				childList[index] = left;
				childList[++index] = right;
			}

			public Node<T> FindProperNode(T v)
			{
				int index = keySet.BinarySearch(v);
				index = ~index;
				Node<T> result = childList[index];
				return result;
			}

			#region Tymczasowo

			public KeyChildList Sever(Node<T> node, int count)
			{
				//return node.GetFirst(count);
				throw new NotImplementedException();

			}
			#endregion

			#endregion
		}
		#endregion
	}
}
