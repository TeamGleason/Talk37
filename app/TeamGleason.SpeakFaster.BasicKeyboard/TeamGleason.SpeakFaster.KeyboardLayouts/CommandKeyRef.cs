namespace TeamGleason.SpeakFaster.KeyboardLayouts
{
    public class CommandKeyRef : KeyRefBase<CommandKey>
    {
        internal override KeyCollection<CommandKey> IndexCollection => _layout.CommandKeys;

        public override void Create(IKeyboardControl parent) => parent.Create(this, Key);
    }
}