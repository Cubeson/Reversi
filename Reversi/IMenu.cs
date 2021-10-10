namespace Reversi
{
    public class MenuComponent
    {
        public string Name { get; private set; }
        public MenuComponent(string Name)
        {
            this.Name = Name;
        }
    }
    public static class MenuComponents{
        public static MenuComponent NewGame = new MenuComponent("New Game");
        public static MenuComponent Options = new MenuComponent("Options");
        public static MenuComponent Exit    = new MenuComponent("Exit");
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
