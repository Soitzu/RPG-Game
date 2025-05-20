using Raylib_cs;

namespace Game.Models
{
    public enum ItemType
    {
        Weapon,
        Consumable,
        QuestItem,
        Armor,
        Misc
    }



    public class Item
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public ItemType Type { get; set; }
        public Texture2D Icon { get; set; }



        public Item(int id, string name, int count, string description, int value, ItemType type)
        {
            Id = id;
            Name = name;
            Count = count;
            Description = description;
            Value = value;
            Type = type;
        }





    }
}
