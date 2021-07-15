using System.Xml.Serialization;

namespace TeamGleason.SpeakFaster.KeyboardLayouts
{
    public class Modifier
    {
        [XmlAttribute]
        public string KeyRef { get; set; }
    }
}
