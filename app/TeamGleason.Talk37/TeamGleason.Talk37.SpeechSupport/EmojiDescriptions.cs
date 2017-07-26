using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TeamGleason.Talk37.SpeechSupport
{
    /// <summary>
    /// Programmed emojis.
    /// </summary>
    public static class EmojiDescriptions
    {
        static readonly string Prefix = Path.GetFileNameWithoutExtension(typeof(EmojiDescriptions).GetTypeInfo().Assembly.ManifestModule.Name);

        static readonly Dictionary<int, EmojiDescription> _emojis = new Dictionary<int, EmojiDescription>();

        static void AddEmoji(EmojiDescription description)
        {
            foreach (var utf32 in description.Utf32s)
            {
                _emojis.Add(utf32, description);
            }
        }

        static void AddEmoji(int utf32, string audioFileName, string visualString)
        {
            var description = new EmojiDescription(utf32, Path.Combine(Prefix, audioFileName), visualString);
            AddEmoji(description);
        }

        static void AddEmoji(string emoji, string audioFileName, string visualString)
        {
            if (emoji.Length != (char.IsSurrogatePair(emoji, 0) ? 2 : 1))
            {
                throw new ArgumentOutOfRangeException("emoji");
            }

            var utf32 = char.ConvertToUtf32(emoji, 0);

            AddEmoji(utf32, audioFileName, visualString);
        }
        static EmojiDescriptions()
        {
            AddEmoji("😀", "poop_mono.wav", "");
        }

        /// <summary>
        /// Get the emoji description for a UTF-32 character.
        /// </summary>
        /// <param name="utf32">The character to decode.</param>
        /// <returns>The Emoji description or null if no mapping.</returns>
        public static EmojiDescription Get(int utf32)
        {
            EmojiDescription description;

            _emojis.TryGetValue(utf32, out description);

            return description;
        }
    }
}
