﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightClaw.Extensions;

namespace LightClaw.Engine.Graphics
{
    /// <summary>
    /// Represents a <see cref="Buffer"/>-<see cref="VertexAttributePointer"/>-association.
    /// </summary>
    public struct BufferDescription
    {
        /// <summary>
        /// The data buffer.
        /// </summary>
        public IBuffer Buffer { get; private set; }

        /// <summary>
        /// The <see cref="VertexAttributePointer"/>s describing the layout of the data in memory.
        /// </summary>
        public ImmutableList<VertexAttributePointer> VertexAttributePointers { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="BufferDescription"/> setting the buffer and the attribute pointers.
        /// </summary>
        /// <param name="vertexBuffer">The data buffer.</param>
        /// <param name="vertexAttributePointers">The <see cref="VertexAttributePointer"/>s describing the layout of the data in memory.</param>
        public BufferDescription(IBuffer vertexBuffer, IEnumerable<VertexAttributePointer> vertexAttributePointers)
            : this()
        {
            Contract.Requires<ArgumentNullException>(vertexBuffer != null);
            Contract.Requires<ArgumentNullException>(vertexAttributePointers != null);

            this.Buffer = vertexBuffer;
            this.VertexAttributePointers = vertexAttributePointers.FilterNull().ToImmutableList();
        }
    }
}
