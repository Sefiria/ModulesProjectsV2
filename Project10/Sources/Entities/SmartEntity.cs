using Project10.Sources.Genesis;

namespace Project10.Sources.Entities
{
    public class SmartEntity : Entity
    {
        public Genome Genome;
        public SmartEntity()
        {
            Genome = new Genome();
        }
    }
}
