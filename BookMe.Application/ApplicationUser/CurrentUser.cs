namespace BookMe.Application.ApplicationUser
{
    public class CurrentUser
    {
        public CurrentUser(string id, string email, string firstName, string lastName, bool isAdmin)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            IsAdmin = isAdmin;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
