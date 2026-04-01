namespace Demo.TestEntities
{
    public class B24TaskEntity
    {
        public B24TaskEntity(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
