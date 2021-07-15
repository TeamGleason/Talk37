namespace TeamGleason.SpeakFaster.KeyboardLayouts
{
    public class CommandKeyRef : KeyRefBase<CommandKey>
    {
        internal override KeyCollection<CommandKey> IndexCollection => _layout.CommandKeys;
    }
}