// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    internal class NonInvokeGazeTargetItem : GazeTargetItem
    {
        internal NonInvokeGazeTargetItem()
        {
        }

        internal override bool IsInvokable
        {
            get { return false; }
        }

        internal override TimeSpan GetElementStateDelay(PointerState pointerState, TimeSpan defaultValue) => TimeSpan.Zero;

        internal override TimeSpan GetElementRepeatDelay(TimeSpan defaultValue) => TimeSpan.Zero;

        internal override int GetMaxDwellRepeatCount() => 0;

        internal override void Invoke()
        {
        }

        internal override void ShowFeedback(DwellProgressState state, double progress)
        {
        }
    }
}