using System.Xml.Serialization;

namespace TeamGleason.SpeakFaster.KeyboardLayouts
{
    public class CommandKey : IndexObject
    {
        [XmlAttribute]
        public string Label { get; set; }

        [XmlAttribute]
        public string Icon { get; set; }

        [XmlAttribute]
        public string Style { get; set; }

        [XmlAttribute]
        public bool Toggles { get; set; }

        [XmlAttribute]
        public string Command { get; set; }
    }
}