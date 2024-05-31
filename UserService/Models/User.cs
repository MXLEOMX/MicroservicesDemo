namespace UserService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; } // Propiedad que acepta valores NULL
        public string? Email { get; set; } // Propiedad que acepta valores NULL
    }
}
