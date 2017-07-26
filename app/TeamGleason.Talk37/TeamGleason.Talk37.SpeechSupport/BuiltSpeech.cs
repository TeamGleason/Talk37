using Windows.Media.SpeechSynthesis;

namespace TeamGleason.Talk37.SpeechSupport
{
    /// <summary>
    /// Compiled speech.
    /// </summary>
    public class BuiltSpeech
    {
        internal BuiltSpeech(SpeechSynthesisStream stream)
        {
            Stream = stream;
        }

        /// <summary>
        /// The audio stream.
        /// </summary>
        public SpeechSynthesisStream Stream { get; }
    }
}
