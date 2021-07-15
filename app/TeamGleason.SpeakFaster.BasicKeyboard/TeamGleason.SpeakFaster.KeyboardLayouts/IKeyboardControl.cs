namespace TeamGleason.SpeakFaster.KeyboardLayouts
{
    public interface IKeyboardControl
    {
        void Create(TextKeyRef keyRef, TextKey key);
        void Create(CommandKeyRef keyRef, CommandKey key);
        void Create(PredictionKeyRef keyRef, PredictionKey key);
    }
}