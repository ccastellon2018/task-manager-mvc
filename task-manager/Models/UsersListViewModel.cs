namespace task_manager.Models;

public class UsersListViewModel
{
    public List<UsuarioViewModel> Users { get; set; } = [];
    public string Message { get; set; } = string.Empty;
}
