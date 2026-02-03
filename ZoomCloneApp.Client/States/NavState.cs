namespace ZoomCloneApp.Client.States
{
    public class NavState
    {
        // set private = true only within this class
        public bool Clicked { get; private set; } = false;
        public Action? ButtonAction;
        public void ClickButton()
        {
            Clicked = true;
            ButtonAction?.Invoke();
        }
    }
}
