using System;

namespace EDW.Domain.Models
{
    public class Entity
    {
        public Guid Id { get; set; }
        public Entity()
        {
            Id = new Guid();
        }
    }
}