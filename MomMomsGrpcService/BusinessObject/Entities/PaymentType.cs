﻿namespace BusinessObject.Entities
{
    public class PaymentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get;}
    }
}
