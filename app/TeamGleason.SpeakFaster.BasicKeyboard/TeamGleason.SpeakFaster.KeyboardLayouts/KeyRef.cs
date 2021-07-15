using System.Xml.Serialization;

namespace TeamGleason.SpeakFaster.KeyboardLayouts
{
    public class KeyRefBase
    {
        protected KeyboardLayout _layout;

        internal void Attach(KeyboardLayout layout)
        {
            _layout = layout;
        }
    }

    public abstract class KeyRefBase<T> : KeyRefBase
        where T : IndexObject
    {
        [XmlAttribute]
        public string KeyRef { get; set; }

        [XmlAttribute]
        public int Row { get; set; }

        [XmlAttribute]
        public int RowSpan { get; set; }

        [XmlAttribute]
        public int Column { get; set; }

        [XmlAttribute]
        public int ColumnSpan { get; set; }

        internal T Key => IndexCollection[KeyRef];

        internal abstract KeyCollection<T> IndexCollection { get; }
    }
}