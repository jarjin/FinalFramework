using System;
using System.Collections.Generic;

namespace FTRuntime.Internal {
	class SwfList<T> {
		T[] _data;
		int _size;

		static readonly T[] _emptyData       = new T[0];
		const int           _defaultCapacity = 4;

		public SwfList() {
			_data = _emptyData;
			_size = 0;
		}

		public SwfList(int capacity) {
			if ( capacity < 0 ) {
				throw new ArgumentOutOfRangeException(
					"capacity", "capacity must be >= 0");
			} else if ( capacity == 0 ) {
				_data = _emptyData;
				_size = 0;
			} else {
				_data = new T[capacity];
				_size = 0;
			}
		}

		public void Push(T value) {
			if ( _size == _data.Length ) {
				var new_capacity = _size == 0
					? _defaultCapacity : _size * 2;
				var new_data = new T[new_capacity];
				Array.Copy(_data, new_data, _size);
				_data = new_data;
			}
			_data[_size++] = value;
		}

		public T Pop() {
			if ( _size == 0 ) {
				throw new InvalidOperationException("empty list");
			}
			var last = _data[--_size];
			_data[_size] = default(T);
			return last;
		}

		public T Peek() {
			if ( _size == 0 ) {
				throw new InvalidOperationException("empty list");
			}
			return _data[_size - 1];
		}

		public void Clear() {
			Array.Clear(_data, 0, _size);
			_size = 0;
		}

		public T UnorderedRemoveAt(int index) {
			if ( (uint)index >= (uint)_size ) {
				throw new IndexOutOfRangeException();
			}
			var last = _data[_size - 1];
			_data[index] = last;
			_data[--_size] = default(T);
			return last;
		}

		public T this[int index] {
			get {
				if ( (uint)index >= (uint)_size ) {
					throw new IndexOutOfRangeException();
				}
				return _data[index];
			}
			set {
				if ( (uint)index >= (uint)_size ) {
					throw new IndexOutOfRangeException();
				}
				_data[index] = value;
			}
		}

		public int Count {
			get { return _size; }
		}

		public int Capacity {
			get { return _data.Length; }
			set {
				if ( value < _size ) {
					throw new ArgumentOutOfRangeException("capacity");
				}
				if ( value != _data.Length ) {
					if ( value > 0 ) {
						var new_data = new T[value];
						if ( _size > 0 ) {
							Array.Copy(_data, new_data, _size);
						}
						_data = new_data;
					} else {
						_data = _emptyData;
					}
				}
			}
		}

		public void AssignTo(List<T> list) {
			list.Clear();
			if ( list.Capacity < Count ) {
				list.Capacity = Count * 2;
			}
			for ( int i = 0, e = Count; i < e; ++i ) {
				list.Add(this[i]);
			}
		}

		public void AssignTo(SwfList<T> list) {
			if ( list._data.Length < _size ) {
				var new_data = new T[_size * 2];
				Array.Copy(_data, new_data, _size);
				list._data = new_data;
				list._size = _size;
			} else {
				if ( _size < list._size ) {
					Array.Clear(list._data, _size, list._size - _size);
				}
				if ( _size > 0 ) {
					Array.Copy(_data, list._data, _size);
				}
				list._size = _size;
			}
		}
	}
}