﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using LightClaw.Engine.Core;
using LightClaw.Engine.Threading;
using OpenTK.Graphics.OpenGL4;

namespace LightClaw.Engine.Graphics.OpenGL
{
    /// <summary>
    /// Represents the base class for all OpenGL wrapper objects.
    /// </summary>
    [DataContract]
    public abstract class GLObject : DispatcherEntity, IGLObject
    {
        /// <summary>
        /// A lock used to restrict access to the supported extensions-method.
        /// </summary>
        private static object extensionQueryLock = new object();

        /// <summary>
        /// Contains all supported OpenGL extensions.
        /// </summary>
        private static volatile List<string> supportedExtensions; // Volatile needed for double checked locking

        private static readonly Lazy<Version> _MaxOpenGLVersion = new Lazy<Version>(
            () => new Version(GL.GetInteger(GetPName.MajorVersion), GL.GetInteger(GetPName.MinorVersion)), 
            true
        );

        /// <summary>
        /// Gets the maximum supported OpenGL version.
        /// </summary>
        public static Version MaxOpenGLVersion
        {
            get
            {
                return _MaxOpenGLVersion.Value;
            }
        }

        private int _Handle;

        /// <summary>
        /// The OpenGL-handle.
        /// </summary>
        [IgnoreDataMember]
        public virtual int Handle
        {
            get
            {
                return _Handle;
            }
            protected set
            {
                this.SetProperty(ref _Handle, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="GLObject"/>.
        /// </summary>
        protected GLObject() : this(null, 0) { }

        /// <summary>
        /// Initializes a new <see cref="GLObject"/> setting the object's handle.
        /// </summary>
        /// <param name="handle">The object's associated OpenGL-handle.</param>
        protected GLObject(int handle) : this(null, handle) { }

        /// <summary>
        /// Initializes a new <see cref="GLObject"/> setting the object's name.
        /// </summary>
        /// <param name="name">The object's name.</param>
        protected GLObject(string name) : this(name, 0) { }

        /// <summary>
        /// Initializes a new <see cref="GLObject"/> setting the object's name and handle.
        /// </summary>
        /// <param name="handle">The object's associated OpenGL-handle.</param>
        /// <param name="name">The object's name.</param>
        protected GLObject(string name, int handle)
            : base(name) 
        {
            this.Handle = handle;
        }

        /// <summary>
        /// Static constructor of <see cref="GLObject"/>.
        /// </summary>
        static GLObject()
        {
            // Assume GLContext is present
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
        }

        /// <summary>
        /// Protected dispose-callback freeing all unmanaged and optionally managed resources as well.
        /// </summary>
        /// <param name="disposing">Indicates whether to free managed resources as well.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    this.Handle = 0;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets the last OpenGL error.
        /// </summary>
        /// <returns>The last error.</returns>
        public static ErrorCode GetLastError()
        {
            return GL.GetError();
        }

        /// <summary>
        /// Checks whether the specified <see cref="Version"/> is supported.
        /// </summary>
        /// <param name="version">The <see cref="Version"/> to check for.</param>
        /// <returns><c>true</c> if the specified OpenGL Version is supported, otherwise <c>false</c>.</returns>
        public static bool IsOpenGLVersionSupported(Version version)
        {
            return (MaxOpenGLVersion >= version);
        }

        /// <summary>
        /// Checks whether the specified OpenGL extension is supported.
        /// </summary>
        /// <param name="extensionName">The extension to check for.</param>
        /// <returns><c>true</c> if the extension can be used, otherwise <c>false</c>.</returns>
        public static bool SupportsExtension(string extensionName)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(extensionName));

            if (supportedExtensions == null)
            {
                lock (extensionQueryLock)
                {
                    if (supportedExtensions == null)
                    {
                        supportedExtensions = new List<string>();
                        int extensionCount = GL.GetInteger(GetPName.NumExtensions);
                        for (int i = 0; i < extensionCount; i++)
                        {
                            supportedExtensions.Add(GL.GetString(StringNameIndexed.Extensions, i));
                        }
                    }
                }
            }

            return supportedExtensions.Contains(extensionName, StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Retreives the <see cref="GLObject"/>'s handle via implicit conversion.
        /// </summary>
        /// <param name="glObject">The <see cref="GLObject"/> to obtain the handle of.</param>
        /// <returns>The <see cref="GLObject"/>'s handle.</returns>
        public static implicit operator int(GLObject glObject)
        {
            Contract.Requires<ArgumentNullException>(glObject != null);

            return glObject.Handle;
        }
    }
}
