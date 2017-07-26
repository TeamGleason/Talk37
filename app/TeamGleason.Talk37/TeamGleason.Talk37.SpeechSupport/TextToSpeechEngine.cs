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

        static int NextPosition(string text, int previousPosition)
        {
            var increment = char.IsSurrogatePair(text, previousPosition) ? 2 : 1;
            var nextPosition = previousPosition + increment;
            return nextPosition;
        }

        /// <summary>
        /// Speak given text.
        /// </summary>
        /// <param name="text">The text.</param>
        public async Task<BuiltSpeech> SayText(string text)
        {
            var ssmlBuilder = new StringBuilder("<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>");
            ssmlBuilder.Append("<mark name='Start'/>");

            var start = 0;
            var index = 0;
            while (index < text.Length)
            {
                var utf32 = char.ConvertToUtf32(text, index);
                var description = EmojiDescriptions.Get(utf32);
                var foundEmoji = description != null;

                if (foundEmoji)
                {
                    ssmlBuilder.Append(text.Substring(start, index - start));
                    index = NextPosition(text, index);
                    start = index;
                    ssmlBuilder.Append($"<audio src='{description.AudioFileName}'/>");
                }
                else
                {
                    index = NextPosition(text, index);
                }
            }
            ssmlBuilder.Append(text.Substring(start));

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
