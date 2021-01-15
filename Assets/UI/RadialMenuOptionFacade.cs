namespace Assets.UI
{
    public class RadialMenuOptionFacade
    {
        public string Text { get; }
        public RadialMenuDelegates.MenuItemClicked OnClick { get; }
        public RadialMenuDelegates.MenuItemConfirmed OnConfirm { get; }
        public bool Enabled { get; }

        public RadialMenuOptionFacade(string text, RadialMenuDelegates.MenuItemClicked onClick, RadialMenuDelegates.MenuItemConfirmed onConfirm, bool enabled = true)
        {
            Text = text;
            OnClick = onClick;
            OnConfirm = onConfirm;
            Enabled = enabled;
        }
    }
}