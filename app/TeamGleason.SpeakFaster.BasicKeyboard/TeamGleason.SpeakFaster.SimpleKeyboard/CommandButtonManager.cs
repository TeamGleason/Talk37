﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using TeamGleason.SpeakFaster.KeyboardLayouts;

namespace TeamGleason.SpeakFaster.SimpleKeyboard
{
    internal abstract class CommandButtonManager : ButtonManager<CommandKey>
    {
        internal CommandButtonManager(KeyboardControl parent, CommandKey key, ButtonBase button)
            : base(parent, key, button)
        {
            if (key.Label != null)
            {
                button.Content = key.Label;
            }
            else
            {
                var template = parent.Resources[key.Icon];
                if (template != null)
                {
                    button.ContentTemplate = (DataTemplate)template;
                }
                else
                {
                    Debug.Fail($"No template for {key.Icon}");
                    button.Content = key.Icon;
                }
            }
        }
    }
}