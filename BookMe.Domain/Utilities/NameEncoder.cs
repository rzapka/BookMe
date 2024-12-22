namespace BookMe.Domain.Utilities
{
    public static class NameEncoder
    {
        public static string Encode(string name)
        {
            return name.Trim().ToLower().Replace(" ", "-");
        }
    }
}
