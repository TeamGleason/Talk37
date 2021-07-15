using System.Xml.Serialization;

namespace TeamGleason.SpeakFaster.KeyboardLayouts
{
    public class PredictionKey : IndexObject
    {
        [XmlAttribute]
        public string Style { get; set; }


        [XmlAttribute]
        public int Order { get; set; }
    }
}