﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LightClaw.Engine.Core;

namespace LightClaw.Engine.Graphics
{
    [DataContract]
    public sealed class EffectPassData : ICloneable, IEquatable<EffectPassData>
    {
        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public EffectStageSources Sources { get; private set; }

        [DataMember]
        public ImmutableArray<string> Uniforms { get; private set; }

        private EffectPassData() { }

        public EffectPassData(EffectStageSources sources, IEnumerable<string> uniforms)
            : this(null, sources, uniforms)
        {
            Contract.Requires<ArgumentNullException>(sources != null);
            Contract.Requires<ArgumentNullException>(uniforms != null);
        }

        public EffectPassData(string name, EffectStageSources sources, IEnumerable<string> uniforms)
        {
            Contract.Requires<ArgumentNullException>(sources != null);
            Contract.Requires<ArgumentNullException>(uniforms != null);

            this.Name = name;
            this.Sources = sources;
            this.Uniforms = uniforms.ToImmutableArray();
        }

        public object Clone()
        {
            return new EffectPassData(this.Name, this.Sources, this.Uniforms);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(obj, this))
                return true;

            EffectPassData data = obj as EffectPassData;
            return (data != null) ? this.Equals(data) : false;
        }

        public bool Equals(EffectPassData other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(other, this))
                return true;

            return (this.Name == other.Name) && (this.Sources == other.Sources) && (this.Uniforms.SequenceEqual(other.Uniforms));
        }

        public override int GetHashCode()
        {
            return HashF.GetHashCode(
                this.Sources,
                this.Name,
                HashF.GetHashCode(this.Uniforms)
            );
        }

        public static bool operator ==(EffectPassData left, EffectPassData right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(EffectPassData left, EffectPassData right)
        {
            return !(left == right);
        }
    }
}
