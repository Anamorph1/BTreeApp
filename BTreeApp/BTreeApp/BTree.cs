using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTreeApp
{
	public enum NodeType
	{
		Inner,
		Leaf,
		Root
	}

	
	public class BTree<T> // where T: class ??????
	{
		Node root;
		int height = 0;
		public readonly int size;


		public BTree(int n)
		{
			size = 2*n-1; //Stopień B-drzewa
			root = AllocateNode();
			height = 0; //TODO zrobić żeby przy wstawieniu pierwszego elementu wysokość drzewa przeskoczyła na 1
		}

		private Node AllocateNode(bool isInner = false)
		{
			if(isInner)
			{
				return new Node(size, false);
			}
			return new Node(size);	
		}

		public bool Insert(T v)
		{
			if(IsExist(v))
			{
				return false;
			}
			
			if (root.IsFull)
			{
				bool isRoot = true;
				++height;
				Node r = root;
				Node s = AllocateNode(isRoot);
				root = s;
				r.SetAsRoot(root);
				
				r.SplitChild(v);
				return s.InsertNonFull(v);
			}
			else
			{
				return root.InsertNonFull(v);
			}
		}

		public bool IsExist(T v)
		{
			return Search(v) != null ? true : false;
		}

		private object Search(T v)
		{
			return null;
			//throw new NotImplementedException();
		}

		#region Node
		private class Node
		{
			Node root = null;
			KeyChildList entry;

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
			public Node(int n, bool isLeaf = true)
			{
				size = n;
				this.isLeaf = isLeaf;
				entry = new KeyChildList();
			}

			public Node()
			{
				entry = new KeyChildList();
			}
			#endregion

			#region Methods

			#region SplitChild()
			//Zakładamy, że węzeł ma już tymczasowy element w sobie 
			public void SplitChild(T v)
			{
				Node leftSibling = AllocateNewNodeBasedOn();
				T key = ExtractMidKey();
				leftSibling.RewriteLeftHalfFrom(this);
				SendUpKeyToRoot(key, leftSibling, this);
			}

			private Node AllocateNewNodeBasedOn()
			{
				Node result = new Node(size);
				result.isLeaf = this.IsLeaf;
				return result;
			}

			private T ExtractMidKey()
			{
				//Zmienić is full na false
				int index = entry.Count / 2;
				T key = entry[index];
				//root.entry.Insert(key);
				entry.Remove(key);
				this.isFull = false;
				return key;
			}

			private void RewriteLeftHalfFrom(Node node)
			{
				int count = node.Count / 2;
				entry = node.entry.GetRange(0, count);
				node.entry.RemoveRange(0, count);
			}

			private void SendUpKeyToRoot(T key, Node left, Node right)
			{
				int iOk = root.entry.Insert(key);
				root.entry.SetLeftChild(key, left);
				root.entry.SetRightChild(key, right);
			}
			#endregion

			#region InsertNonFull()
			public bool InsertNonFull(T v)
			{
				if (isLeaf)
				{
					int iOk = entry.Insert(v);
					if(iOk != -1)
					{
						if(entry.Count == size)
						{
							isFull = true;
						}
						return true;
					}
					return false;
				}
				else
				{
					Node n = entry.FindProperNode(v);
					if (n.isFull)
					{
						n.SplitChild(v);
					}
					return n.InsertNonFull(v);
				}
			}
			#endregion

			internal void SetAsRoot(Node root)
			{
				this.root = root;
			}

			#endregion

			#region KeyChildList
			class KeyChildList
			{
				List<T> keySet; //TODO opracować kolekcję
				List<Node> childList;

				#region Constructors/Destructors

				public KeyChildList(List<T> keySet, List<Node> childList)
				{
					this.keySet = keySet;
					this.childList = childList;
				}

				public KeyChildList()
				{
					keySet = new List<T>();
					childList = new List<Node>() { null };
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
					return new KeyChildList(keySet.GetRange(index, count), childList.GetRange(index, count+1));
				}

				public void RemoveRange(int index, int count)
				{
					keySet.RemoveRange(index, count);
					childList.RemoveRange(index, count+1);
				}

				internal void SetRightChild(T key, Node node)
				{
					int index = keySet.IndexOf(key);
					childList[++index] = node;

				}

				internal void SetLeftChild(T key, Node node)
				{
					int index = keySet.IndexOf(key);
					childList[index] = node;
				}

				public Node FindProperNode(T v)
				{
					int index = keySet.BinarySearch(v);
					index = ~index;
					Node result = childList[index];
					return result;
				}

				#region Tymczasowo

				public KeyChildList Sever(Node node, int count)
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
	#endregion
}
