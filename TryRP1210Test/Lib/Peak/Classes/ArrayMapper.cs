using System;
using System.Collections;
using System.Collections.Generic;

namespace Peak.Classes
{
    public class ArrayMapper<type> : IEnumerable<type>, IEnumerable
    {
        private int length;
        private int offset;
        private List<type> origin;

        public ArrayMapper(List<type> source, int startIndex)
          : this(source, startIndex, source.Count - startIndex)
        {
        }

        public ArrayMapper(List<type> source, int startIndex, int count)
        {
            this.origin = source;
            this.Initialize(startIndex, count);
        }

        private void Initialize(int startIndex, int count)
        {
            if (startIndex < 0 || count < 0)
                throw new ArgumentOutOfRangeException();
            if (startIndex + count > this.origin.Count)
                throw new IndexOutOfRangeException();
            this.offset = startIndex;
            this.length = count;
        }

        public IEnumerator<type> GetEnumerator()
        {
            for (int i = this.offset; i < this.offset + this.length; ++i)
                yield return this.origin[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.Mapped;
        }

        public void Remap(int startIndex, int count) => this.Initialize(startIndex, count);

        public void Remap(int startIndex) => this.Initialize(startIndex, this.origin.Count - startIndex);

        public bool NextBlock()
        {
            if (this.offset + this.length > this.origin.Count - this.length)
                return false;
            this.Initialize(this.offset + this.length, this.length);
            return true;
        }

        public static explicit operator ArrayMapper<type>(List<type> array) => new ArrayMapper<type>(array, 0);

        public static explicit operator List<type>(ArrayMapper<type> mapper) => mapper.origin;

        public type this[int index]
        {
            get
            {
                if (index > this.length - 1 || index < 0)
                    throw new IndexOutOfRangeException();
                return this.origin[index + this.offset];
            }
            set
            {
                if (index > this.length - 1 || index < 0)
                    throw new IndexOutOfRangeException();
                this.origin[index + this.offset] = value;
            }
        }

        public int Length
        {
            get => this.length;
            internal set => this.Remap(this.offset, value);
        }

        public int StartIndex
        {
            get => this.offset;
            internal set => this.Remap(value, this.length);
        }

        public List<type> Mapped => SubArray.GetArray<type>(this.origin, this.offset, this.length);

        public List<type> Source => this.origin;
    }
}
