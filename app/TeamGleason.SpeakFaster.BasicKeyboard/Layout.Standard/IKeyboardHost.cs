using System;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard
{
    public interface IKeyboardHost
    {
        void SetPredictions(params string[] predictions);

        event EventHandler<string> AcceptPrediction;
    }
}
