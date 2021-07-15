using System.Xml.Serialization;

namespace TeamGleason.SpeakFaster.KeyboardLayouts
{
    public class View : IndexObject
    {
        [XmlElement(nameof(PredictionKeyRef), typeof(PredictionKeyRef))]
        [XmlElement(nameof(CommandKeyRef), typeof(CommandKeyRef))]
        [XmlElement(nameof(TextKeyRef), typeof(TextKeyRef))]
        public KeyRefBase[] KeyRefs { get; set; }
    }
}