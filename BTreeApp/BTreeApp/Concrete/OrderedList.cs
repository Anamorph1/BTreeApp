﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTreeApp.Concrete
{
	public class OrderedList<T> : IList<T>, ICollection<T>, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
	{
		#region Fields
		readonly List<T> _list;
		readonly IComparer<T> _comparer;
		#endregion

		#region Constructors
		OrderedList(List<T> list, IComparer<T> comparer)
		{
			_list = list;
			_comparer = comparer;
		}
		public OrderedList()
			: this(new List<T>(), Comparer<T>.Default)
		{
		}
		public OrderedList(IComparer<T> comparer)
			: this(new List<T>(), comparer)
		{
		}
		public OrderedList(IEnumerable<T> collection)
			: this(collection, Comparer<T>.Default)
		{
		}

		public OrderedList(IEnumerable<T> collection, IComparer<T> comparer)
			: this(new List<T>(collection), comparer)
		{
			_list.Sort(comparer);
		}

		public OrderedList(int capacity)
			: this(new List<T>(capacity), Comparer<T>.Default)
		{
		}
		public OrderedList(int capacity, IComparer<T> comparer)
			: this(new List<T>(capacity), comparer)
		{
		}
		#endregion

		#region Properties
		public int Capacity { get => _list.Capacity; set => _list.Capacity = value; }
		public int Count { get => _list.Count; }
		object IList.this[int index] { get => _list[index]; set => _list[index] = (T)value; }
		public T this[int index] { get => _list[index]; set => _list[index] = value; }
		bool ICollection.IsSynchronized { get => false; }
		object ICollection.SyncRoot { get => _list; } 
		bool IList.IsFixedSize { get => false; }
		bool IList.IsReadOnly { get => false; }
		bool ICollection<T>.IsReadOnly { get => false; }
		#endregion


		#region Methods

		#region Add
		void ICollection<T>.Add(T item)
		{
			Add(item);
		}
		/// <summary>
		/// Adds a new item to the appropriate index of the SortedList
		/// </summary>
		/// <param name="item">The item to be inserted</param>
		/// <returns>The index at which the item was inserted (-1 if item exists in collection)</returns>
		public int Add(T item)
		{
			int index = BinarySearch(item);
			if (index < 0)
			{
				index = ~index;
			}
			else
			{
				return index;
			}
			_list.Insert(index, item);
			return index;
		}
		int IList.Add(object item)
		{
			return Add((T)item);
		}
		#endregion

		#region BinarySearch
		public int BinarySearch(T item)
		{
			return _list.BinarySearch(item, _comparer);
		}
		public int BinarySearch(int index, int count, T item)
		{
			return _list.BinarySearch(index, count, item, _comparer);
		}
		#endregion

		public void Clear() { _list.Clear(); }
		public bool Contains(T item) { return BinarySearch(item) >= 0; }
		bool IList.Contains(object item)
		{
			return Contains((T)item);
		}
		public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) { return _list.ConvertAll(converter); }
		public void CopyTo(T[] array) { _list.CopyTo(array); }
		public void CopyTo(T[] array, int arrayIndex) { _list.CopyTo(array, arrayIndex); }
		void ICollection.CopyTo(Array array, int arrayIndex) { _list.CopyTo((T[])array, arrayIndex); }
		public void CopyTo(int index, T[] array, int arrayIndex, int count) { _list.CopyTo(index, array, arrayIndex, count); }
		public void ForEach(Action<T> action)
		{
			foreach (T item in _list)
			{
				action(item);
			}
		}

		IEnumerator IEnumerable.GetEnumerator() { return _list.GetEnumerator(); }
		public IEnumerator<T> GetEnumerator() { return _list.GetEnumerator(); }
		public List<T> GetRange(int index, int count) { return _list.GetRange(index, count); }

		public bool Remove(T item)
		{
			int index = BinarySearch(item);
			if (index < 0)
			{
				return false;
			}
			_list.RemoveAt(index);
			return true;
		}
		void IList.Remove(object item)
		{
			Remove((T)item);
		}

		public void RemoveAt(int index) { _list.RemoveAt(index); }
		public void RemoveRange(int index, int count) { _list.RemoveRange(index, count); }
		public T[] ToArray() { return _list.ToArray(); }
		public void TrimExcess() { _list.TrimExcess(); }
		public int IndexOf(T item)
		{
			int index = BinarySearch(item);
			if (index < 0) return -1;

			return ++index;
		}
		int IList.IndexOf(object item)
		{
			return IndexOf((T)item);
		}
		public int LastIndexOf(T item)
		{
			int index = BinarySearch(item);
			if (index < 0) return -1;
			while (_list[++index].Equals(item)) { }
			return --index;
		}
		#endregion

		#region NotImplemented
		const string InsertExceptionMsg = "SortedList detemines position to insert automatically - use add method without an index";
		void IList.Insert(int index, object item)
		{
			throw new NotImplementedException(InsertExceptionMsg);
		}
		void IList<T>.Insert(int index, T item)
		{
			throw new NotImplementedException(InsertExceptionMsg);
		}
		#endregion
	}

}
