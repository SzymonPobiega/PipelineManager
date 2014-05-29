namespace ReleaseManager.Extensibility
{
// ReSharper disable once InconsistentNaming

    public class ExtensionDescriptor
    {
        private readonly UIExtension[] _uiExtensions;

        public ExtensionDescriptor(params UIExtension[] uiExtensions)
        {
            _uiExtensions = uiExtensions;
        }

        public UIExtension[] UIExtensions
        {
            get { return _uiExtensions; }
        }
    }
}