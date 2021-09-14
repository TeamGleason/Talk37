// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace StandardLib
{
    // Every filter must provide an Update method which transforms sample data
    // and returns filtered output
    public interface IGazeFilter
    {
        GazeFilterArgs Update(GazeFilterArgs args);

        void LoadSettings(IDictionary<string, object> settings);
    }
}