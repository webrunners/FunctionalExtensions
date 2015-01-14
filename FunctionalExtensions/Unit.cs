namespace FunctionalExtensions
{
    /// <summary>
    /// This type represents void
    /// </summary>
    public class Unit
    {
        private Unit() { }
        public static readonly Unit Value = new Unit();
        public override string ToString()
        {
            return "unit";
        }
    }
}