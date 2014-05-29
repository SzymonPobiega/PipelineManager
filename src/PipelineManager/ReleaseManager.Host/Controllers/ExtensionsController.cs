using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ReleaseManager.Extensibility;

namespace ReleaseManager.Host.Controllers
{
    public class ExtensionsController : ApiController
    {
        private readonly IEnumerable<ExtensionDescriptor> _extensions;

        public ExtensionsController(IEnumerable<ExtensionDescriptor> extensions)
        {
            _extensions = extensions;
        }

        public ExtensionsModel Get()
        {
            var deploymentDetailsExtensions = FindExtensions(UIExtensionType.DeploymentDetails);
            var releaseCandidateExtensions = FindExtensions(UIExtensionType.ReleaseCandidate);

            return new ExtensionsModel
            {
                DeploymentDetailsExtensions = deploymentDetailsExtensions,
                ReleaseCandidateExtensions = releaseCandidateExtensions
            };
        }

        private string[] FindExtensions(UIExtensionType extensionType)
        {
            var deploymentDetailsExtensions = _extensions
                .SelectMany(x => x.UIExtensions)
                .Where(x => x.Type == extensionType)
                .Select(x => x.ViewModelName).ToArray();
            return deploymentDetailsExtensions;
        }
    }
}