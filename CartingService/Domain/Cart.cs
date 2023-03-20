using System.Collections.Generic;

namespace CartingService.Domain
{
    public class Cart
    {
        public int Id { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();
    }
}
