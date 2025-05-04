namespace ECommerce.Models
{
    public class Usuario
    {
        public int Id {  get; set; }
        public string? Username {  get; set; }
        public string? Email { get; set; }
        public string? Pass {  get; set; }
        public List<string> Roles { get; set; } = []; // ADMIN, USER, EDITOR
    }
}