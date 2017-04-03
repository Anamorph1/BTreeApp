using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTreeApp.Abstract;

namespace BTreeApp.Concrete
{
	class KeyChildSet<T,V> : IKeyChildSet<T,V>
	{
		SortedSet<T> keySet; //TODO opracować kolekcję
		List<V> childList;

		#region Constructors/Destructors

		public KeyChildSet(SortedSet<T> keySet, List<V> childList)
		{
			this.keySet = keySet;
			this.childList = childList;
		}

		public KeyChildSet()
		{
			keySet = new SortedSet<T>();
			childList = new List<V>() { default(V) };
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
				return keySet.ElementAt(i);
			}
		}
		#endregion

		#region Methods

		public int Insert(T v)
		{
			//Add new key in specific place
			//Set new key's left/right children to null
			// 1 2		->		1 * 2
			// 1 2 3	->		1 n n 3
			bool bOk = keySet.Add(v);
			if(!bOk)
			{
				return -1;
			}
			int index = keySet.IndexOf(v);
			childList.Insert(index, null);
			childList[index + 1] = null;

			return index;


		}

		public void Remove(T v)
		{
			keySet.Remove(v);
		}

		public KeyChildSet GetRange(int index, int count)
		{
			return new KeyChildSet(keySet.GetRange(index, count), childList.GetRange(index, count + 1));
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

		#region IEnumerable<T>
		public IEnumerator<T> GetEnumerator()
		{
			return keySet.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion


		#endregion
	}
}
