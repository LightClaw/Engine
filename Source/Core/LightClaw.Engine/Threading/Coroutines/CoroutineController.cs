﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LightClaw.Engine.Core;

namespace LightClaw.Engine.Threading.Coroutines
{
    /// <summary>
    /// Represents a lightweight <see cref="Component"/> that controls execution of coroutines.
    /// </summary>
    [DataContract]
    [Solitary(typeof(CoroutineController), "More than one controller induces unnecessary overhead.")]
    public class CoroutineController : Component
    {
        /// <summary>
        /// The list of managed coroutines.
        /// </summary>
        [IgnoreDataMember]
        private readonly List<ICoroutineContext> contexts = new List<ICoroutineContext>();

        /// <summary>
        /// Notifies when a new coroutine was added to the <see cref="CoroutineController"/>.
        /// </summary>
        public event EventHandler<ParameterEventArgs> CoroutineAdded;

        /// <summary>
        /// Initializes a new <see cref="CoroutineController"/>.
        /// </summary>
        public CoroutineController() { }

        /// <summary>
        /// Initializes a new <see cref="CoroutineController"/> from a <see cref="Func{T}"/>.
        /// </summary>
        /// <param name="coroutine">The coroutine function.</param>
        public CoroutineController(Func<IEnumerable> coroutine)
        {
            Contract.Requires<ArgumentNullException>(coroutine != null);

            this.Add(coroutine);
        }

        /// <summary>
        /// Initializes a new <see cref="CoroutineController"/> from a coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to be executed.</param>
        public CoroutineController(IEnumerable coroutine)
        {
            Contract.Requires<ArgumentNullException>(coroutine != null);

            this.Add(coroutine);
        }

        /// <summary>
        /// Queries a new coroutine for execution.
        /// </summary>
        /// <param name="coroutine">The coroutine to execute.</param>
        public void Add(IEnumerable coroutine)
        {
            Contract.Requires<ArgumentNullException>(coroutine != null);

            lock (this.contexts)
            {
                this.contexts.Add(new CoroutineContext(coroutine));
            }
            this.Raise(this.CoroutineAdded);
        }

        /// <summary>
        /// Queries a new coroutine for execution.
        /// </summary>
        /// <param name="coroutine">The coroutine to execute.</param>
        public void Add(Func<IEnumerable> coroutine)
        {
            Contract.Requires<ArgumentNullException>(coroutine != null);

            lock (this.contexts)
            {
                this.contexts.Add(new CoroutineContext(coroutine));
            }
            this.Raise(this.CoroutineAdded);
        }

        /// <summary>
        /// Queries coroutines for execution.
        /// </summary>
        /// <param name="coroutines">The coroutines to execute.</param>
        public void AddRange(IEnumerable<IEnumerable> coroutines)
        {
            Contract.Requires<ArgumentNullException>(coroutines != null);

            foreach (IEnumerable coroutine in coroutines)
            {
                this.Add(coroutine);
            }
        }

        /// <summary>
        /// Queries coroutines for execution.
        /// </summary>
        /// <param name="coroutines">The coroutines to execute.</param>
        public void AddRange(IEnumerable<Func<IEnumerable>> coroutines)
        {
            Contract.Requires<ArgumentNullException>(coroutines != null);

            foreach (Func<IEnumerable> coroutine in coroutines)
            {
                this.Add(coroutine);
            }
        }

        /// <summary>
        /// Updates all coroutines and removes the ones that have finished execution.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        /// <param name="pass">The current updating pass.</param>
        protected override bool OnUpdate(GameTime gameTime, int pass)
        {
            IEnumerable<ICoroutineContext> contexts = this.contexts;
            if (contexts != null && (pass == 0))
            {
                lock (contexts)
                {
#pragma warning disable 0728
                    contexts = contexts.ToList();
#pragma warning restore 0728
                }

                foreach (ICoroutineContext context in contexts)
                {
                    if (context.Step())
                    {
                        lock (this.contexts)
                        {
                            this.contexts.Remove(context);
                        }
                    }
                }
            }

            return true;
        }
    }
}
