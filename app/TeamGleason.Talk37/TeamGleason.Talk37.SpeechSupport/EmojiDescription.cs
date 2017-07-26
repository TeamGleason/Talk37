using System;
using System.Collections.Generic;

namespace TeamGleason.Talk37.SpeechSupport
{
    /// <summary>
    /// Description of Emoji.
    /// </summary>
    public class EmojiDescription
    {
        /// <summary>
        /// The UTF-32 characters that this represents.
        /// </summary>
        public IEnumerable<int> Utf32s { get; }

        /// <summary>
        /// File containing the audio.
        /// </summary>
        public string AudioFile { get; }

        /// <summary>
        /// Duration of the audio file.
        /// </summary>
        public TimeSpan AudioDuration { get; }

        /// <summary>
        /// The string for programming the visual device.
        /// </summary>
        public string VisualString { get; }

        /// <summary>
        /// Duration of the visual.
        /// </summary>
        public TimeSpan VisualDuration { get; }
    }
}
