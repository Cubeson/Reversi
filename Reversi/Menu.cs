namespace Reversi
{

    class Menu : IMenu
    {
        int index = 0;
        MenuComponent[] menuComponents;
        public Menu(MenuComponent[] components)
        {
            menuComponents = components;
        }
        public MenuComponent[] getMenuComponents()
        {
            return menuComponents;
        }
        public MenuComponent getCurrentComponent()
        {
            return menuComponents[index];
        }
        public int getIndex()
        {
            return index;
        }
        public void IndexDown()
        {
            if (index <= 0) return;
            index--;
        }
        public void IndexUp()
        {
            if (index >= menuComponents.Length - 1) return;
            index++;
        }
    }
}
