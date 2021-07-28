namespace TeamGleason.SpeakFaster.BasicKeyboard.Layout.Standard
{
    public interface IKeyboardControl
    {
        void Create(TextKeyRef keyRef, TextKey key);
        void Create(CommandKeyRef keyRef, CommandKey key);
        void Create(PredictionKeyRef keyRef, PredictionKey key);
    }
}