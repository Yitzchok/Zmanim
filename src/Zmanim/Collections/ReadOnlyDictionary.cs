using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

//This was taken from http://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=29
namespace PublicDomain
{
    /// <summary>
    /// Provides the base class for a generic read-only dictionary.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of keys in the dictionary.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The type of values in the dictionary.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// An instance of the <b>ReadOnlyDictionary</b> generic class is
    /// always read-only. A dictionary that is read-only is simply a
    /// dictionary with a wrapper that prevents modifying the
    /// dictionary; therefore, if changes are made to the underlying
    /// dictionary, the read-only dictionary reflects those changes. 
    /// See <see cref="Dictionary`2"/> for a modifiable version of 
    /// this class.
    /// </para>
    /// <para>
    /// <b>Notes to Implementers</b> This base class is provided to 
    /// make it easier for implementers to create a generic read-only
    /// custom dictionary. Implementers are encouraged to extend this
    /// base class instead of creating their own. 
    /// </para>
    /// </remarks>
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    [ComVisible(false)]
    [DebuggerTypeProxy(typeof(ReadOnlyDictionaryDebugView<,>))]
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
                                                    IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection,
                                                    IEnumerable
    {
        private readonly IDictionary<TKey, TValue> _source;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:ReadOnlyDictionary`2" /> class that contains
        /// elements copied from the specified 
        /// <see cref="T:IDictionary`2"></see> and uses the default equality
        /// comparer for the key type.
        /// </summary>
        /// <param name="dictionaryToWrap">The <see cref="T:IDictionary`2" />
        /// that will be wrapped.</param>
        /// <exception cref="T:System.ArgumentException">
        /// Thrown when dictionary contains one or more duplicate keys.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown when the dictionary is null.
        /// </exception>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionaryToWrap)
        {
            if (dictionaryToWrap == null)
                throw new ArgumentNullException("dictionaryToWrap");

            this._source = dictionaryToWrap;
        }

        /// <summary>This method is not supported by the 
        /// <see cref="T:ReadOnlyDictionary`2"/></summary>
        /// <param name="key">
        /// The object to use as the key of the element to add.</param>
        /// <param name="value">
        /// The object to use as the value of the element to add.</param>
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            ThrowNotSupportedException();
        }

        /// <summary>Gets the number of key/value pairs contained in the 
        /// <see cref="T:ReadOnlyDictionary`2"></see>.</summary>
        /// <returns>The number of key/value pairs contained in the
        /// <see cref="T:ReadOnlyDictionary`2"></see>.</returns>
        public int Count { get { return this._source.Count; } }

        /// <summary>Determines whether the <see cref="T:ReadOnlyDictionary`2" />
        /// contains the specified key.</summary>
        /// <returns>
        /// true if the <see cref="T:ReadOnlyDictionary`2" /> contains
        /// an element with the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate in the
        /// <see cref="T:ReadOnlyDictionary`2"></see>.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown when the key is null.
        /// </exception>
        public bool ContainsKey(TKey key)
        {
            return this._source.ContainsKey(key);
        }

        /// <summary>Gets a collection containing the keys in the
        /// <see cref="T:ReadOnlyDictionary`2"></see>.</summary><returns>A
        /// <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />
        /// containing the keys in the
        /// <see cref="T:System.Collections.Generic.Dictionary`2"></see>.
        /// </returns>
        public ICollection<TKey> Keys
        {
            get { return this._source.Keys; }
        }

        /// <summary>
        /// This method is not supported by the <see cref="T:ReadOnlyDictionary`2"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.
        /// </returns>
        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            ThrowNotSupportedException();
            return false;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value
        /// associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the value parameter.
        /// This parameter is passed uninitialized.</param>
        /// <returns>
        /// <b>true</b> if the <see cref="T:ReadOnlyDictionary`2" /> contains
        /// an element with the specified key; otherwise, <b>false</b>.
        /// </returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this._source.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets a collection containing the values of the
        /// <see cref="T:ReadOnlyDictionary`2" />.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return this._source.Values; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <returns>
        /// The value associated with the specified key. If the specified key
        /// is not found, a get operation throws a 
        /// <see cref="T:System.Collections.Generic.KeyNotFoundException" />,
        /// and a set operation creates a new element with the specified key.
        /// </returns>
        /// <param name="key">The key of the value to get or set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown when the key is null.
        /// </exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
        /// The property is retrieved and key does not exist in the collection.
        /// </exception>
        public TValue this[TKey key]
        {
            get { return this._source[key]; }
            set { ThrowNotSupportedException(); }
        }

        /// <summary>This method is not supported by the
        /// <see cref="T:ReadOnlyDictionary`2"/></summary>
        /// <param name="item">
        /// The object to add to the <see cref="T:ICollection`1"/>.
        /// </param>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(
            KeyValuePair<TKey, TValue> item)
        {
            ThrowNotSupportedException();
        }

        /// <summary>This method is not supported by the 
        /// <see cref="T:ReadOnlyDictionary`2"/></summary>
        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            ThrowNotSupportedException();
        }

        /// <summary>
        /// Determines whether the <see cref="T:ICollection`1"/> contains a
        /// specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="T:ICollection`1"/>.
        /// </param>
        /// <returns>
        /// <b>true</b> if item is found in the <b>ICollection</b>; 
        /// otherwise, <b>false</b>.
        /// </returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(
            KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this._source)
                .Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ICollection to an Array, starting at a
        /// particular Array index. 
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the
        /// destination of the elements copied from ICollection.
        /// The Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in array at which copying begins.
        /// </param>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(
            KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this._source)
                .CopyTo(array, arrayIndex);
        }

        /// <summary>Returns true </summary>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return true; }
        }

        /// <summary>This method is not supported by the
        /// <see cref="T:ReadOnlyDictionary`2"/></summary>
        /// <param name="item">
        /// The object to remove from the ICollection.
        /// </param>
        /// <returns>Will never return a value.</returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            ThrowNotSupportedException();
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A IEnumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<
            KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)this._source)
                .GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An IEnumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this._source).GetEnumerator();
        }

        /// <summary>
        /// This method is not supported by the <see cref="T:ReadOnlyDictionary`2"/>.
        /// </summary>
        /// <param name="key">
        /// The System.Object to use as the key of the element to add.
        /// </param>
        /// <param name="value">
        /// The System.Object to use as the value of the element to add.
        /// </param>
        void IDictionary.Add(object key, object value)
        {
            ThrowNotSupportedException();
        }

        /// <summary>
        /// This method is not supported by the <see cref="T:ReadOnlyDictionary`2"/>.
        /// </summary>
        void IDictionary.Clear()
        {
            ThrowNotSupportedException();
        }

        /// <summary>
        /// Determines whether the IDictionary object contains an element
        /// with the specified key.
        /// </summary>
        /// <param name="key">
        /// The key to locate in the IDictionary object.
        /// </param>
        /// <returns>
        /// <b>true</b> if the IDictionary contains an element with the key;
        /// otherwise, <b>false</b>.
        /// </returns>
        bool IDictionary.Contains(object key)
        {
            return ((IDictionary)this._source).Contains(key);
        }

        /// <summary>
        /// Returns an <see cref="IDictionaryEnumerator"/> for the
        /// <see cref="IDictionary{TKey,TValue}"/>. 
        /// </summary>
        /// <returns>
        /// An IDictionaryEnumerator for the IDictionary.
        /// </returns>
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)this._source).GetEnumerator();
        }

        /// <summary>Returns true</summary>
        bool IDictionary.IsFixedSize { get { return true; } }

        /// <summary>Returns true</summary>
        bool IDictionary.IsReadOnly { get { return true; } }

        /// <summary></summary>
        ICollection IDictionary.Keys
        {
            get { return ((IDictionary)this._source).Keys; }
        }

        /// <summary>
        /// This method is not supported by the <see cref="T:ReadOnlyDictionary`2"/>.
        /// </summary>
        /// <param name="key">
        /// Gets an <see cref="ICollection{T}"/> object containing the keys of the 
        /// <see cref="IDictionary{TKey,TValue}"/> object.
        /// </param>
        void IDictionary.Remove(object key)
        {
            ThrowNotSupportedException();
        }

        /// <summary>
        /// Gets an ICollection object containing the values in the DictionaryBase object.
        /// </summary>
        ICollection IDictionary.Values
        {
            get { return ((IDictionary)this._source).Values; }
        }

        /// <summary>
        /// Gets or sets the element with the specified key. 
        /// </summary>
        /// <param name="key">The key of the element to get or set.</param>
        /// <returns>The element with the specified key. </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown when a value is set.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the key is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>
        object IDictionary.this[object key]
        {
            get { return ((IDictionary)this._source)[key]; }
            set { ThrowNotSupportedException(); }
        }

        /// <summary>
        /// For a description of this member, see <see cref="ICollection{T}.CopyTo"/>. 
        /// </summary>
        /// <param name="array">
        /// The one-dimensional Array that is the destination of the elements copied from 
        /// ICollection. The Array must have zero-based indexing.
        /// </param>
        /// <param name="index">
        /// The zero-based index in Array at which copying begins.
        /// </param>
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this._source).CopyTo(array, index);
        }

        /// <summary>
        /// For a description of this member, see <see cref="ICollection{T}.IsSynchronized"/>.
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)this._source).IsSynchronized; }
        }

        /// <summary>
        /// For a description of this member, see <see cref="ICollection{T}.SyncRoot"/>.
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return ((ICollection)this._source).SyncRoot; }
        }

        private static void ThrowNotSupportedException()
        {
            throw new NotSupportedException("This Dictionary is read-only");
        }
    }

    internal sealed class ReadOnlyDictionaryDebugView<TKey, TValue>
    {
        private IDictionary<TKey, TValue> dict;

        public ReadOnlyDictionaryDebugView(
            ReadOnlyDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            this.dict = dictionary;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<TKey, TValue>[] Items
        {
            get
            {
                KeyValuePair<TKey, TValue>[] array =
                    new KeyValuePair<TKey, TValue>[this.dict.Count];
                this.dict.CopyTo(array, 0);
                return array;
            }
        }
    }
}