using System.Collections.Generic;
using ReleaseManager.Host.Controllers;

namespace ReleaseManager.Host.Models
{
    public class ProjectsModel : Representation<ProjectsController>
    {
        public List<ProjectState> Projects { get; set; }
    }
}