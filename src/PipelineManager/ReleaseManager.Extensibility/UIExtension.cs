using System;
using JetBrains.Annotations;

namespace ReleaseManager.Extensibility
{
// ReSharper disable once InconsistentNaming
    public class UIExtension
    {
        private readonly UIExtensionType _type;
        [NotNull] private readonly string _viewModelName;

        public UIExtension(UIExtensionType type, [NotNull] string viewModelName)
        {
            if (viewModelName == null) throw new ArgumentNullException("viewModelName");
            _type = type;
            _viewModelName = viewModelName;
        }

        public UIExtensionType Type
        {
            get { return _type; }
        }

        [NotNull]
        public string ViewModelName
        {
            get { return _viewModelName; }
        }
    }
}