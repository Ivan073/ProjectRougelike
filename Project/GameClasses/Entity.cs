namespace Project.GameClasses
{
    public class Entity
    {
        public int X { set; get; } = 0; //центральные координаты
        public int Y { set; get; } = 0;

        public int Size { set; get; } = 50; //персонаж 50 (значение по умолчанию)

        public string Name { set; get; } = "NoName";

        public bool IsVisible = true;

        public Entity()
        {
        }
        public Entity(int x, int y)
        {
            X = x; Y = y;
        }

        public Entity(int x, int y,string name)
        {
            X = x; Y = y;Name= name;
        }

        public Entity(int x, int y, string name, int size)
        {
            X = x; Y = y; Name = name; Size = size;
        }
    }
}
