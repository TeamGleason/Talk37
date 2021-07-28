using System.Xml.Serialization;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard
{
    public class Modifier
    {
        [XmlAttribute]
        public string KeyRef { get; set; }
    }
}
