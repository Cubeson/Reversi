namespace Reversi
{
    public class MenuComponent
    {
        public string Message { get; set; }
        public string AdditionalMessage { get; set; }
        public MenuComponent(string Message)
        {
            this.Message = Message;
        }
        public string getMessage()
        {
            return Message + AdditionalMessage;
        }
    }
    public static class MenuComponents{
        public static MenuComponent NewGame     = new MenuComponent("New Game");
        public static MenuComponent Options     = new MenuComponent("Options");
        public static MenuComponent Exit        = new MenuComponent("Exit");
        public static MenuComponent BoardSize   = new MenuComponent("Board Size");

    }

    interface IMenu
    {
        public void IndexUp();
        public void IndexDown();
        public int getIndex();
        public MenuComponent getCurrentComponent();
        public MenuComponent[] getMenuComponents();
    }
}
