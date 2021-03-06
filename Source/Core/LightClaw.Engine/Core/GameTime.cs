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
    /// Represents the game's time in fixed- or variable-step game loops.
    /// </summary>
    [DataContract]
    public struct GameTime : ICloneable, IEquatable<GameTime>
    {
        /// <summary>
        /// Gets a <see cref="GameTime"/> with time zero.
        /// </summary>
        public static GameTime Null
        {
            get
            {
                return new GameTime();
            }
        }

        /// <summary>
        /// The time that passed since the last call to <see cref="IUpdateable.Update"/>.
        /// </summary>
        [DataMember]
        public TimeSpan ElapsedSinceLastUpdate { get; private set; }

        /// <summary>
        /// The game's total running time.
        /// </summary>
        [DataMember]
        public TimeSpan TotalGameTime { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="GameTime"/> setting the delta time and the total game time.
        /// </summary>
        /// <param name="elapsedSinceUpdate">
        /// The time in seconds that passed since the last call to <see cref="IUpdateable.Update"/>.
        /// </param>
        /// <param name="totalTime">The game's total running time in seconds.</param>
        public GameTime(TimeSpan elapsedSinceUpdate, TimeSpan totalTime)
            : this()
        {
            Contract.Requires<ArgumentOutOfRangeException>(elapsedSinceUpdate >= TimeSpan.Zero);
            Contract.Requires<ArgumentOutOfRangeException>(totalTime >= TimeSpan.Zero);

            this.ElapsedSinceLastUpdate = elapsedSinceUpdate;
            this.TotalGameTime = totalTime;
        }

        /// <summary>
        /// Clones the <see cref="GameTime"/> creating a deep copy.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public object Clone()
        {
            return new GameTime(this.ElapsedSinceLastUpdate, this.TotalGameTime);
        }

        /// <summary>
        /// Tests whether the current instance and the specified object are the same.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to test against.</param>
        /// <returns><c>true</c> if both instances are the same, otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return (obj is GameTime) ? this.Equals((GameTime)obj) : false;
        }

        /// <summary>
        /// Checks whether the two <see cref="GameTime"/>s are equal.
        /// </summary>
        /// <param name="other">The <see cref="GameTime"/> to test against.</param>
        /// <returns><c>true</c> if both instances are the same, otherwise <c>false</c>.</returns>
        public bool Equals(GameTime other)
        {
            return (this.ElapsedSinceLastUpdate == other.ElapsedSinceLastUpdate) && (this.TotalGameTime == other.TotalGameTime);
        }

        /// <summary>
        /// Obtains the <see cref="GameTime"/>s hash code.
        /// </summary>
        /// <returns>The <see cref="GameTime"/>s hash code.</returns>
        public override int GetHashCode()
        {
            return HashF.GetHashCode(this.ElapsedSinceLastUpdate, this.TotalGameTime);
        }

        /// <summary>
        /// Adds the specified time that <paramref name="elapsedSinceLastUpdate"/> to the <see cref="GameTime"/>.
        /// </summary>
        /// <param name="gameTime">The <see cref="GameTime"/> to add to.</param>
        /// <param name="elapsedSinceLastUpdate">The time (in seconds) that elapsed since the last update call.</param>
        /// <returns>A newly created game time with the new values.</returns>
        public static GameTime operator +(GameTime gameTime, double elapsedSinceLastUpdate)
        {
            Contract.Requires<ArgumentOutOfRangeException>(elapsedSinceLastUpdate >= 0.0);

            TimeSpan elapsed = TimeSpan.FromSeconds(elapsedSinceLastUpdate);
            return new GameTime(
                elapsed,
                gameTime.TotalGameTime + elapsed
            );
        }

        /// <summary>
        /// Adds the specified time that <paramref name="elapsedSinceLastUpdate"/> to the <see cref="GameTime"/>.
        /// </summary>
        /// <param name="gameTime">The <see cref="GameTime"/> to add to.</param>
        /// <param name="elapsedSinceLastUpdate">The time that elapsed since the last update call.</param>
        /// <returns>A newly created game time with the new values.</returns>
        public static GameTime operator +(GameTime gameTime, TimeSpan elapsedSinceLastUpdate)
        {
            Contract.Requires<ArgumentOutOfRangeException>(elapsedSinceLastUpdate >= TimeSpan.Zero);

            return new GameTime(
                elapsedSinceLastUpdate,
                gameTime.TotalGameTime + elapsedSinceLastUpdate
            );
        }

        /// <summary>
        /// Checks whether two <see cref="GameTime"/>s are equal.
        /// </summary>
        /// <param name="left">The first operand.</param>
        /// <param name="right">The second operand.</param>
        /// <returns><c>true</c> if both instances are the same, otherwise <c>false</c>.</returns>
        public static bool operator ==(GameTime left, GameTime right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks whether two <see cref="GameTime"/>s are equal.
        /// </summary>
        /// <param name="left">The first operand.</param>
        /// <param name="right">The second operand.</param>
        /// <returns><c>true</c> if both instances are the same, otherwise <c>false</c>.</returns>
        public static bool operator !=(GameTime left, GameTime right)
        {
            return !(left == right);
        }
    }
}
