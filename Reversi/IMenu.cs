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
    public class Window
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Message { set; get; }
        public Window(int X, int Y, int Width, int Height, string Message)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Message = Message;
        }
    }

    interface IMenu
    {
        public void IndexUp();
        public void IndexDown();
        public int getIndex();
        public MenuComponent getCurrentComponent();
        public MenuComponent[] getMenuComponents();
        public Window getWindow();
        public void setWindow(Window window);
        public void closeWindow();
    }
}
