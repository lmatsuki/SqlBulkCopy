namespace Demo.Domain.Entities
{
    public class Skill
    {
        public Skill(int playerId, string name, int correlationId)
        {
            PlayerId = playerId;
            Name = name;
            CorrelationId = correlationId;
        }

        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public decimal Cooldown { get; set; }
        public int Level { get; set; }
        public bool IsLearned { get; set; }
        public int CorrelationId { get; set; }
    }
}
