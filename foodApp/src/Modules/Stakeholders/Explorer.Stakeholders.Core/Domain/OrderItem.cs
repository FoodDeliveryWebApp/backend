using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain
{
    public class OrderItem : Entity
    {
        public int OrderId { get; private set; }
        public int FoodId { get; private set; }
        public int Quantity { get; private set; }
        public Food Food { get; private set; }

        protected OrderItem() { }

        public OrderItem(int foodId, int quantity, Food food)
        {
            FoodId = foodId;
            Quantity = quantity;
            Food = food;
        }
    }
}
