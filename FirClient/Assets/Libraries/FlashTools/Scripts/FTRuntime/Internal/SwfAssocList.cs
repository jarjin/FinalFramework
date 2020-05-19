using System.Collections.Generic;

namespace FTRuntime.Internal {
	class SwfAssocList<T> {
		SwfList<T>           _list;
		Dictionary<T, int>   _dict;
		IEqualityComparer<T> _comp;

		public SwfAssocList() {
			_list = new SwfList<T>();
			_dict = new Dictionary<T, int>();
			_comp = EqualityComparer<T>.Default;
		}

		public SwfAssocList(int capacity) {
			_list = new SwfList<T>(capacity);
			_dict = new Dictionary<T, int>(capacity);
			_comp = EqualityComparer<T>.Default;
		}

		public T this[int index] {
			get {
				return _list[index];
			}
		}

		public int this[T item] {
			get {
				return _dict[item];
			}
		}

		public int Count {
			get {
				return _list.Count;
			}
		}

		public bool Contains(T value) {
			return _dict.ContainsKey(value);
		}

		public void Add(T item) {
			if ( !_dict.ContainsKey(item) ) {
				_dict.Add(item, _list.Count);
				_list.Push(item);
			}
		}

		public void Remove(T item) {
			int index;
			if ( _dict.TryGetValue(item, out index) ) {
				_dict.Remove(item);
				var reordered =_list.UnorderedRemoveAt(index);
				if ( !_comp.Equals(reordered, item) ) {
					_dict[reordered] = index;
				}
			}
		}

		public void Clear() {
			_list.Clear();
			_dict.Clear();
		}

		public void AssignTo(List<T> list) {
			_list.AssignTo(list);
		}

		public void AssignTo(SwfList<T> list) {
			_list.AssignTo(list);
		}
	}
}