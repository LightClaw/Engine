﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace LightClaw.Engine.Core
{
    [ProtoContract(IgnoreListHandling = true)]
    [ProtoInclude(100, typeof(GameObject))]
    public class ListChildManager<T> : ChildManager<T>, IList<T>
        where T : IControllable
    {
        [ProtoIgnore]
        public virtual T this[int index]
        {
            get
            {
                Contract.Assert(index < this.Count);

                return this.Items[index];
            }
            set
            {
                Contract.Assert(index < this.Count);

                this.Items[index] = value;
            }
        }

        public int Count
        {
            get 
            {
                return this.Items.Count;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ListChildManager() { }

        public ListChildManager(IEnumerable<T> items)
        {
            Contract.Requires<ArgumentNullException>(items != null);

            this.AddRange(items);
        }

        public virtual void Add(T item)
        {
            this.Items.Add(item);
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            Contract.Requires<ArgumentNullException>(items != null);

            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        public virtual void Clear()
        {
            this.Items.Clear();
        }

        public virtual bool Contains(T item)
        {
            return this.Items.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            Contract.Assert(arrayIndex <= (array.Length - this.Count));

            this.Items.CopyTo(array, arrayIndex);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public virtual int IndexOf(T item)
        {
            return this.Items.IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            this.Items.Insert(index, item);
        }

        public virtual void InsertRange(int index, IEnumerable<T> items)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Requires<ArgumentNullException>(items != null);
            Contract.Assert(index <= this.Count);

            foreach (T item in items)
            {
                this.Insert(index++, item);
            }
        }

        public virtual bool Remove(T item)
        {
            return this.Items.Remove(item);
        }

        public virtual void RemoveAt(int index)
        {
            Contract.Assert(index <= this.Count);

            this.Items.RemoveAt(index);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.Items != null);
        }
    }
}
