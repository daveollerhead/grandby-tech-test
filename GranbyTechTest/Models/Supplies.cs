namespace GranbyTechTest.Models
{
    public class Supplies : StockedItem
    {
        private Supplies(int id, string name)
            : base(id, name)
        {
        }

        public static Supplies Boxes()
        {
            return new Supplies(1, "Boxes");
        }

        public static Supplies BubbleWrap()
        {
            return new Supplies(2, "Bubble Wrap");
        }
    }
}