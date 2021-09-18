﻿// Licensed to the .NET Foundation under one or more agreements.
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

        public override bool IsInvokable
        {
            get { return false; }
        }

        public override TimeSpan GetElementStateDelay(PointerState pointerState, TimeSpan defaultValue) => TimeSpan.Zero;

        public override TimeSpan GetElementRepeatDelay(TimeSpan defaultValue) => TimeSpan.Zero;

        public override int GetMaxDwellRepeatCount() => 0;

        public override void Invoke()
        {
        }

        protected override void ShowFeedback(DwellProgressState state, double progress)
        {
        }
    }
}