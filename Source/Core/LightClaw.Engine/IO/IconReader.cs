﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightClaw.Engine.IO
{
    /// <summary>
    /// Represents an <see cref="IContentReader"/> reading <see cref="Icon"/>s.
    /// </summary>
    public class IconReader : IContentReader
    {
        /// <summary>
        /// Checks whether the <see cref="IconReader"/> can read assets of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="assetType">The type of the asset that is about to be read.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="IconReader"/> can read assets of the specified <see cref="Type"/>, otherwise
        /// <c>false</c>.
        /// </returns>
        public bool CanRead(Type assetType, object parameter)
        {
            return (assetType == typeof(Icon));
        }

        /// <summary>
        /// Asynchronously reads the icon.
        /// </summary>
        /// <param name="parameters">
        /// <see cref="ContentReadParameters"/> containing information about the asset to be loaded.
        /// </param>
        /// <returns>
        /// The deserialized asset.
        /// </returns>
        public Task<object> ReadAsync(ContentReadParameters parameters)
        {
            return Task.FromResult<object>(new Icon(parameters.AssetStream));
        }
    }
}
