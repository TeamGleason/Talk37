namespace TeamGleason.SpeakFaster.KeyboardLayouts
{
    public class PredictionKeyRef : KeyRefBase<PredictionKey>
    {
        internal override KeyCollection<PredictionKey> IndexCollection => _layout.PredictionKeys;

        public override void Create(IKeyboardControl parent) => parent.Create(this, Key);
    }
}