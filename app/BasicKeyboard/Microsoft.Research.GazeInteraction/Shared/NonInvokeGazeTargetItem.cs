// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
#endif

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    internal class NonInvokeGazeTargetItem : GazeTargetItem
    {
        internal NonInvokeGazeTargetItem()
            : base(new Page())
        {
        }

        internal override bool IsInvokable
        {
            get { return false; }
        }

        internal override void Invoke(UIElement element)
        {
        }
    }
}