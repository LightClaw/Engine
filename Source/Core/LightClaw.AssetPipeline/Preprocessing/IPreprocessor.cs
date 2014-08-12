﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightClaw.AssetPipeline.Preprocessing
{
    public interface IPreprocessor
    {
        Task PreprocessAsync(Stream data, object parameter);
    }
}
