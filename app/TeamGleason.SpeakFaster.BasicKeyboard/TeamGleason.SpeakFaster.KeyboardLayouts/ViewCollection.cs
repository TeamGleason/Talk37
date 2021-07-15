namespace TeamGleason.SpeakFaster.KeyboardLayouts
{
    public class ViewCollection : KeyCollection<View>
    {
        private readonly KeyboardLayout _layout;

        internal ViewCollection(KeyboardLayout layout)
        {
            _layout = layout;
        }

        public override void Add(View item)
        {
            base.Add(item);

            foreach (var keyRef in item.KeyRefs)
            {
                keyRef.Attach(_layout);
            }
        }
    }
}