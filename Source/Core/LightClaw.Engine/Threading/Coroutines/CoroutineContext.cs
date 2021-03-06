﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LightClaw.Engine.Core;

namespace LightClaw.Engine.Threading.Coroutines
{
    /// <summary>
    /// Represents a controller for the execution of a coroutine.
    /// </summary>
    public class CoroutineContext : Entity, ICoroutineContext, IUpdateable
    {
        /// <summary>
        /// The coroutine to execute.
        /// </summary>
        private readonly IEnumerable enumerable;

        /// <summary>
        /// The <see cref="IEnumerator"/> performing the steps.
        /// </summary>
        private IEnumerator enumerator;

        /// <summary>
        /// The <see cref="IExecutionBlockRequest"/> blocking the coroutine execution.
        /// </summary>
        private IExecutionBlockRequest blockRequest;

        /// <summary>
        /// Occurs when the coroutine was stepped.
        /// </summary>
        public event EventHandler<SteppedEventArgs> Stepped;

        /// <summary>
        /// Backing field.
        /// </summary>
        private EventHandler<UpdateEventArgs> _Updating;

        /// <summary>
        /// Occurs before the coroutine is being updated.
        /// </summary>
        event EventHandler<UpdateEventArgs> IUpdateable.Updating // Uses lock-free reference implementation of auto events
        {
            add
            {
                EventHandler<UpdateEventArgs> current;
                EventHandler<UpdateEventArgs> original;
                do
                {
                    original = _Updating;
                    EventHandler<UpdateEventArgs> updated = (EventHandler<UpdateEventArgs>)Delegate.Combine(original, value);
                    current = Interlocked.CompareExchange(ref _Updating, updated, original);
                } while (current != original);
            }
            remove
            {
                EventHandler<UpdateEventArgs> current;
                EventHandler<UpdateEventArgs> original;
                do
                {
                    original = _Updating;
                    EventHandler<UpdateEventArgs> updated = (EventHandler<UpdateEventArgs>)Delegate.Remove(original, value);
                    current = Interlocked.CompareExchange(ref _Updating, updated, original);
                } while (current != original);
            }
        }

        /// <summary>
        /// Backing field.
        /// </summary>
        private EventHandler<UpdateEventArgs> _Updated;

        /// <summary>
        /// Occurs after the coroutine is was updated.
        /// </summary>
        event EventHandler<UpdateEventArgs> IUpdateable.Updated // Uses lock-free reference implementation of auto events
        {
            add
            {
                EventHandler<UpdateEventArgs> current;
                EventHandler<UpdateEventArgs> original;
                do
                {
                    original = _Updated;
                    EventHandler<UpdateEventArgs> updated = (EventHandler<UpdateEventArgs>)Delegate.Combine(original, value);
                    current = Interlocked.CompareExchange(ref _Updated, updated, original);
                } while (current != original);
            }
            remove
            {
                EventHandler<UpdateEventArgs> current;
                EventHandler<UpdateEventArgs> original;
                do
                {
                    original = _Updated;
                    EventHandler<UpdateEventArgs> updated = (EventHandler<UpdateEventArgs>)Delegate.Remove(original, value);
                    current = Interlocked.CompareExchange(ref _Updated, updated, original);
                } while (current != original);
            }
        }

        /// <summary>
        /// Indicates whether the execution is currently blocked by an <see cref="IExecutionBlockRequest"/>.
        /// </summary>
        public bool IsBlocked { get; private set; }

        /// <summary>
        /// Indicates whether the <see cref="CoroutineContext"/> is allowed to execute.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Indicates whether the <see cref="CoroutineContext"/> is finished.
        /// </summary>
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="CoroutineContext"/> with a <see cref="Func{T}"/>.
        /// </summary>
        /// <param name="coroutine">The coroutine to be executed.</param>
        public CoroutineContext(Func<IEnumerable> coroutine)
            : this(new DeferredFuncEnumerator(coroutine))
        {
            Contract.Requires<ArgumentNullException>(coroutine != null);
        }

        /// <summary>
        /// Initializes a new <see cref="CoroutineContext"/> with a <see cref="Func{T}"/>.
        /// </summary>
        /// <param name="coroutine">The coroutine to be executed.</param>
        public CoroutineContext(IEnumerable coroutine)
        {
            Contract.Requires<ArgumentNullException>(coroutine != null);

            this.enumerable = coroutine;
            this.Reset();
        }

        /// <summary>
        /// Resets the <see cref="CoroutineContext"/>.
        /// </summary>
        public void Reset()
        {
            lock (this.enumerable)
            {
                this.enumerator = this.enumerable.GetEnumerator();
                this.IsFinished = false;
            }
        }

        /// <summary>
        /// Steps the coroutine once.
        /// </summary>
        /// <param name="current">The current object.</param>
        /// <returns><c>true</c> if the coroutine has finished execution, otherwise <c>false</c>.</returns>
        public bool Step(out object current)
        {
            lock (this.enumerable)
            {
                if (this.IsEnabled && !this.IsFinished && (this.blockRequest == null || !(this.IsBlocked = !this.blockRequest.CanExecute())))
                {
                    this.blockRequest = null;

                    bool result = !(this.IsFinished = !this.enumerator.MoveNext());
                    current = this.enumerator.Current;
                    IExecutionBlockRequest blockRequest = current as IExecutionBlockRequest;
                    if (blockRequest != null)
                    {
                        this.blockRequest = blockRequest;
                        this.IsBlocked = true;
                    }
                    this.RaiseStepped(current, result);
                    return this.IsFinished;
                }
                else
                {
                    current = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Updates the <see cref="CoroutineContext"/>.
        /// </summary>
        /// <param name="gameTime">The current <see cref="GameTime"/>.</param>
        /// <param name="pass">The current updating pass.</param>
        bool IUpdateable.Update(GameTime gameTime, int pass)
        {
            try
            {
                this.Raise(this._Updating, gameTime, pass);
                this.Step();
            }
            finally
            {
                this.Raise(this._Updated, gameTime, pass);
            }

            return true;
        }

        /// <summary>
        /// Raises the <see cref="E:Stepped"/>-event.
        /// </summary>
        /// <param name="current">The current object.</param>
        /// <param name="result"><c>true</c> if the coroutine finished execution, otherwise <c>false</c>.</param>
        private void RaiseStepped(object current, bool result)
        {
            EventHandler<SteppedEventArgs> handler = this.Stepped;
            if (handler != null)
            {
                handler(this, new SteppedEventArgs(current, result));
            }
        }

        /// <summary>
        /// Contains Contract.Invariant-definitions.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.enumerable != null);
        }

        /// <summary>
        /// A structure used to avoid immediate execution <see cref="Func{IEnumerable}"/>s on registration in the
        /// <see cref="CoroutineContext"/>.
        /// </summary>
        private class DeferredFuncEnumerator : IEnumerable, IEnumerator
        {
            /// <summary>
            /// The function returning the coroutine.
            /// </summary>
            private readonly Func<IEnumerable> coroutineFactory;

            /// <summary>
            /// The coroutine to be executed.
            /// </summary>
            private IEnumerator enumerator;

            /// <summary>
            /// The current object.
            /// </summary>
            public object Current { get; private set; }

            /// <summary>
            /// Initializes a new <see cref="DeferredFuncEnumerator"/>.
            /// </summary>
            /// <param name="coroutineFactory">The function returning the coroutine.</param>
            public DeferredFuncEnumerator(Func<IEnumerable> coroutineFactory)
            {
                Contract.Requires<ArgumentNullException>(coroutineFactory != null);

                this.coroutineFactory = coroutineFactory;
            }

            /// <summary>
            /// Gets the <see cref="IEnumerator"/>.
            /// </summary>
            /// <returns>The <see cref="IEnumerator"/>.</returns>
            public IEnumerator GetEnumerator()
            {
                return this;
            }

            /// <summary>
            /// Steps the coroutine.
            /// </summary>
            public bool MoveNext()
            {
                lock (this.coroutineFactory)
                {
                    bool result = (this.enumerator ?? (this.enumerator = this.coroutineFactory().GetEnumerator())).MoveNext();
                    this.Current = this.enumerator.Current;
                    return result;
                }
            }

            /// <summary>
            /// Resets the coroutine.
            /// </summary>
            public void Reset()
            {
                lock (this.coroutineFactory)
                {
                    this.enumerator = this.coroutineFactory().GetEnumerator();
                }
            }

            /// <summary>
            /// Contains Contract.Invariant definitions.
            /// </summary>
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(this.coroutineFactory != null);
            }
        }
    }
}
