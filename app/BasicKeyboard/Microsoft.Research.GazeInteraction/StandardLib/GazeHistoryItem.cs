// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    struct GazeHistoryItem<TElement>
    {
        internal GazeTargetItem<TElement> HitTarget { get; set; }

        internal TimeSpan Timestamp { get; set; }

        internal TimeSpan Duration { get; set; }
    }
}