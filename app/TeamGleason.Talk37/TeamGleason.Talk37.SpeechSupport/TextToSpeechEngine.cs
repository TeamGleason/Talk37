using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;

namespace TeamGleason.Talk37.SpeechSupport
{
    /// <summary>
    /// Text to speech logic.
    /// </summary>
    public class TextToSpeechEngine
    {
        readonly SpeechSynthesizer _synthesizer = new SpeechSynthesizer();

        /// <summary>
        /// Speak given text.
        /// </summary>
        /// <param name="text">The text.</param>
        public async Task<BuiltSpeech> SayText(string text)
        {
            for (var index = 0; index < text.Length; index++)
            {
                Debug.WriteLineIf(char.IsHighSurrogate(text, index), $"{index} is high surrogate");
                Debug.WriteLineIf(char.IsLowSurrogate(text, index), $"{index} is low surrogate");
                Debug.WriteLineIf(char.IsSurrogatePair(text, index), $"{index} is surrogate pair");
                Debug.WriteLineIf(char.IsSurrogate(text, index), $"{index} is surrogate");
            }

            var position = 0;
            while (position < text.Length)
            {
                Debug.WriteLine($"{position} is {char.ConvertToUtf32(text, position)}");
                position += char.IsSurrogatePair(text, position) ? 2 : 1;
            }

            var ssmlBuilder = new StringBuilder("<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>");
            ssmlBuilder.Append("<mark name='Start'/>");
            ssmlBuilder.Append(text);
            ssmlBuilder.Append("<mark name='Finish'/>");
            ssmlBuilder.Append("</speak>");

            var ssml = ssmlBuilder.ToString();

            var stream = await _synthesizer.SynthesizeSsmlToStreamAsync(ssml);

            foreach (var mark in stream.Markers)
            {
                Debug.WriteLine($"{mark.Text} @ {mark.Time.Milliseconds}ms");
            }

            var speech = new BuiltSpeech(stream);

            return speech;
        }
    }
}
