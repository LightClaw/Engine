﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace LightClaw.Engine.Core
{
    [DataContract, ProtoContract]
    [StructureInformation(2, 8, true)]
    public struct Rectangle : IEquatable<Rectangle>, IComparable<Rectangle>
#if SYSTEMDRAWING_INTEROP
                              , IEquatable<System.Drawing.RectangleF>
#endif
    {
        public static Rectangle One
        {
            get
            {
                return new Rectangle(0.0f, 0.0f, 1.0f, 1.0f);
            }
        }

        public static Rectangle Zero
        {
            get
            {
                return new Rectangle();
            }
        }

        public static Rectangle Random
        {
            get
            {
                return new Rectangle(RandomF.GetSingles(4));
            }
        }

        [DataMember, ProtoMember(1)]
        public Vector2 Position { get; private set; }

        [DataMember, ProtoMember(2)]
        public Size Size { get; private set; }

        public float Width
        {
            get
            {
                return this.Size.Width;
            }
        }

        public float Height
        {
            get
            {
                return this.Size.Height;
            }
        }

        public float Top
        {
            get
            {
                return this.Position.Y;
            }
        }

        public float Left
        {
            get
            {
                return this.Position.X;
            }
        }

        public float Bottom
        {
            get
            {
                return this.Top + this.Size.Height;
            }
        }

        public float Right
        {
            get
            {
                return this.Left + this.Size.Width;
            }
        }

        public float Area
        {
            get
            {
                return this.Width * this.Height;
            }
        }

#if SYSTEMDRAWING_INTEROP

        public Rectangle(System.Drawing.RectangleF rect) : this(rect.Location, rect.Size) { }

#endif

        public Rectangle(float[] values)
            : this(values[0], values[1], values[2], values[3])
        {
            Contract.Requires<ArgumentNullException>(values != null);
            Contract.Requires<ArgumentOutOfRangeException>(values.Length == 4);

            Contract.Requires<ArgumentOutOfRangeException>(values[2] >= 0.0f);
            Contract.Requires<ArgumentOutOfRangeException>(values[3] >= 0.0f);
        }

        public Rectangle(float posX, float posY, float sizeW, float sizeH)
            : this(new Vector2(posX, posY), new Size(sizeW, sizeH))
        {
            Contract.Requires<ArgumentOutOfRangeException>(sizeW >= 0.0f);
            Contract.Requires<ArgumentOutOfRangeException>(sizeH >= 0.0f);
        }

        public Rectangle(Vector2 position, Size size)
            : this()
        {
            this.Position = position;
            this.Size = size;
        }

        public int CompareTo(Rectangle other)
        {
            return this.Area.CompareTo(other.Area);
        }

        public bool ContainsX(float x)
        {
            return Rectangle.ContainsX(this, x);
        }

        public bool ContainsY(float y)
        {
            return Rectangle.ContainsY(this, y);
        }

#if SYSTEMDRAWING_INTEROP

        public bool Equals(System.Drawing.RectangleF other)
        {
            return (this.Position == (Vector2)other.Location) && (this.Size == (Size)other.Size);
        }

#endif

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

#if SYSTEMDRAWING_INTEROP
            if (obj is System.Drawing.RectangleF)
            {
                return this.Equals((System.Drawing.RectangleF)obj);
            }
#endif

            return (obj is Rectangle) ? this.Equals((Rectangle)obj) : false;
        }

        public bool Equals(Rectangle other)
        {
            return (this.Position == other.Position) && (this.Size == other.Size);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = Constants.HashStart * Constants.HashFactor + this.Position.GetHashCode();
                hash = hash * Constants.HashFactor + this.Size.GetHashCode();
                return hash;
            }
        }

        public bool Intersects(Rectangle other)
        {
            return Rectangle.Intersect(this, other);
        }

        public static bool ContainsX(Rectangle rect, float x)
        {
            return (rect.Left <= x) && (rect.Right >= x);
        }

        public static bool ContainsY(Rectangle rect, float y)
        {
            return (rect.Top <= y) && (rect.Bottom >= y);
        }

        public static bool Intersect(Rectangle left, Rectangle right)
        {
            return ((right.Position.X + right.Width > left.Position.X) &&
                   ((right.Position.X < left.Position.X) || left.ContainsX(right.Position.X))) &&
                   ((right.Position.Y + right.Height > left.Position.Y) &&
                   ((right.Position.Y > left.Position.Y) || left.ContainsY(right.Position.Y)));
        }

#if SYSTEMDRAWING_INTEROP

        public static implicit operator Rectangle(System.Drawing.RectangleF rect)
        {
            return new Rectangle(rect);
        }

        public static implicit operator System.Drawing.RectangleF(Rectangle rect)
        {
            return new System.Drawing.RectangleF(rect.Position, rect.Size);
        }

#endif
    }
}