// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

#if WINDOWS_UWP
namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
#else
namespace FrameworkLib
#endif
{
    internal struct GazeHistoryItem
    {
        public GazeTargetItem HitTarget { get; set; }

        public TimeSpan Timestamp { get; set; }

        public TimeSpan Duration { get; set; }
    }
}