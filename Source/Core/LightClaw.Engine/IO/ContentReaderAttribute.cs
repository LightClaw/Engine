﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightClaw.Engine.IO
{
    /// <summary>
    /// Instructs the <see cref="ContentManager"/> to use the specified <see cref="IContentReader"/> to read an asset.
    /// </summary>
    /// <remarks>
    /// The <see cref="IContentReader"/> specified via the attribute has precedence over any registered <see cref="IContentReader"/>s.
    /// If the specified <see cref="IContentReader"/> fails to read the asset, the <see cref="ContentManager"/> will fall back to the
    /// registered readers to read the asset.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class ContentReaderAttribute : Attribute
    {
        /// <summary>
        /// The <see cref="Type"/> of <see cref="IContentReader"/> to use.
        /// </summary>
        public Type ContentReaderType { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="ContentReaderAttribute"/> and sets the <see cref="Type"/> of <see cref="IContentReader"/>
        /// to read the asset with.
        /// </summary>
        /// <param name="contentReaderType">The <see cref="Type"/> of of <see cref="IContentReader"/> to read the asset with.</param>
        public ContentReaderAttribute(Type contentReaderType)
        {
            Contract.Requires<ArgumentNullException>(contentReaderType != null);
            Contract.Requires<ArgumentException>(typeof(IContentReader).IsAssignableFrom(contentReaderType));

            this.ContentReaderType = contentReaderType;
        }

        /// <summary>
        /// Contains Contract.Invariant definitions.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.ContentReaderType != null);
        }
    }
}
