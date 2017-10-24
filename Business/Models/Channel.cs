namespace Business.Models
{
    class Channel : IChannel
    {
        public long? Id { get; }

        public string Name { get; }

        public Channel(long? id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
