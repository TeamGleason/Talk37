﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace TeamGleason.SpeakFaster.BasicKeyboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly KeyValuePair<string, Func<XmlReader, KeyRefBase>>[] _types = new KeyValuePair<string, Func<XmlReader, KeyRefBase>>[]
        {
            new KeyValuePair<string, Func<XmlReader, KeyRefBase>>(nameof(PredictionKeyRef), r => new PredictionKeyRef(r)),
            new KeyValuePair<string, Func<XmlReader, KeyRefBase>>(nameof(CommandKeyRef), r => new CommandKeyRef(r)),
            new KeyValuePair<string, Func<XmlReader, KeyRefBase>>(nameof(TextKeyRef), r => new TextKeyRef(r))
        };
        private static readonly Dictionary<string, Func<XmlReader, KeyRefBase>> _constructors = new Dictionary<string, Func<XmlReader, KeyRefBase>>(_types);

        public MainWindow()
        {
            InitializeComponent();

            var keyRefs = ReadLayout();

            var rows = 0;
            var columns = 0;

            foreach (var keyRef in keyRefs)
            {
                if (rows < keyRef.Row + keyRef.RowSpan)
                {
                    rows = keyRef.Row + keyRef.RowSpan;
                }
                if (columns < keyRef.Column + keyRef.ColumnSpan)
                {
                    columns = keyRef.Column + keyRef.ColumnSpan;
                }

                var control = keyRef.CreateControl();
                TheGrid.Children.Add(control);
            }

            for (var i = 0; i < rows; i++)
            {
                TheGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (var i = 0; i < columns; i++)
            {
                TheGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private IEnumerable<KeyRefBase> ReadLayout()
        {
            var keyRefs = new List<KeyRefBase>();

            var xmlLayout = Properties.Resources.Layout;
            using (var input = new StringReader(xmlLayout))
            {
                var settings = new XmlReaderSettings
                {
                    ConformanceLevel = ConformanceLevel.Fragment
                };

                using (var reader = XmlReader.Create(input, settings))
                {
                    while (reader.Read() && reader.NodeType == XmlNodeType.Whitespace) { }

                    while (reader.NodeType == XmlNodeType.Element)
                    {
                        if (!_constructors.TryGetValue(reader.Name, out var constructor))
                        {
                            throw new InvalidDataException();
                        }
                        var item = constructor(reader);
                        keyRefs.Add(item);

                        while (reader.Read() && reader.NodeType == XmlNodeType.Whitespace) { }
                    }

                    if (reader.NodeType != XmlNodeType.None)
                    {
                        throw new InvalidDataException();
                    }
                }
            }

            return keyRefs;
        }
    }
}