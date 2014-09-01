﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LightClaw.Engine.Core
{
    /// <summary>
    /// An error describing the condition when the current thread is not a specified thread.
    /// </summary>
    [Serializable]
    public class WrongThreadException : Exception
    {
        /// <summary>
        /// The <see cref="Thread.ManagedThreadId"/> of the current thread.
        /// </summary>
        public int CurrentThreadId { get; private set; }

        /// <summary>
        /// The <see cref="Thread.ManagedThreadId"/> of the target thread.
        /// </summary>
        public int TargetThreadId { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="WrongThreadException"/>.
        /// </summary>
        public WrongThreadException() { }

        /// <summary>
        /// Initializes a new <see cref="WrongThreadException"/> setting <see cref="P:CurrentThreadId"/> and <see cref="P:TargetThreadId"/>.
        /// </summary>
        /// <param name="currentThreadId">The <see cref="Thread.ManagedThreadId"/> of the current thread.</param>
        /// <param name="targetThreadId">The <see cref="Thread.ManagedThreadId"/> of the target thread.</param>
        public WrongThreadException(int currentThreadId, int targetThreadId)
        {
            this.CurrentThreadId = currentThreadId;
            this.TargetThreadId = targetThreadId;
        }

        /// <summary>
        /// Initializes a new <see cref="WrongThreadException"/> setting the exception message.
        /// </summary>
        /// <param name="message">A description of the error.</param>
        public WrongThreadException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new <see cref="WrongThreadException"/> setting the exception message, <see cref="P:CurrentThreadId"/> and <see cref="P:TargetThreadId"/>.
        /// </summary>
        /// <param name="currentThreadId">The <see cref="Thread.ManagedThreadId"/> of the current thread.</param>
        /// <param name="targetThreadId">The <see cref="Thread.ManagedThreadId"/> of the target thread.</param>
        /// <param name="message">A description of the error.</param>
        public WrongThreadException(string message, int currentThreadId, int targetThreadId)
            : base(message)
        {
            this.CurrentThreadId = currentThreadId;
            this.TargetThreadId = targetThreadId;
        }

        /// <summary>
        /// Initializes a new <see cref="WrongThreadException"/> setting the exception message and the inner exception that lead to
        /// this exception being thrown.
        /// </summary>
        /// <param name="message">A description of the error.</param>
        /// <param name="inner">The <see cref="Exception"/> that lead up to this <see cref="Exception"/>.</param>
        public WrongThreadException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new <see cref="WrongThreadException"/> from the specified <see cref="SerializationInfo"/> and <see cref="StreamingContext"/>.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/>.</param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected WrongThreadException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        {
            Contract.Requires<ArgumentNullException>(info != null);

            this.CurrentThreadId = info.GetInt32("CurrentThreadId");
            this.TargetThreadId = info.GetInt32("TargetThreadId");
        }
    }
}