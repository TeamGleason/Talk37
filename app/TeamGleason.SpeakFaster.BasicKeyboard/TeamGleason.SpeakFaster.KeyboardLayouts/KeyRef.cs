using System.Xml.Serialization;

namespace TeamGleason.SpeakFaster.KeyboardLayouts
{
    public class KeyRefBase
    {
    }

    public class KeyRefBase<T> : KeyRefBase
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
    }
}