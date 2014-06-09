namespace ReleaseManager.Host.Controllers.Admin
{
    public class ProjectAdminController : RestfulController<ProjectAdminRepresentation>
    {

    }

    public class UserRoleController : RestfulController<UserRoleRepresentation>
    {
        
    }

    public class UserRoleRepresentation
    {
        public string Login { get; set; }
        public string Project { get; set; }
        public string Role { get; set; }
    }

    public class ProjectAdminRepresentation
    {
        public string Name { get; set; }
    }
}
