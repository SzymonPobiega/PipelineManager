using System;
using JetBrains.Annotations;

namespace ReleaseManager.Process.TeamCity.Events
{
    public class TeamCityBuildFinishedEvent
    {
        private readonly int _buildId;
        private readonly bool _succeeded;
        private readonly string _statusUrl;
        private readonly string _statusHtml;
        private readonly string _message;

        public TeamCityBuildFinishedEvent(int buildId, bool succeeded, [NotNull] string statusUrl,
            [NotNull] string statusHtml, [NotNull] string message)
        {
            if (statusUrl == null) throw new ArgumentNullException("statusUrl");
            if (statusHtml == null) throw new ArgumentNullException("statusHtml");
            if (message == null) throw new ArgumentNullException("message");
            _buildId = buildId;
            _succeeded = succeeded;
            _statusUrl = statusUrl;
            _statusHtml = statusHtml;
            _message = message;
        }

        public int BuildId
        {
            get { return _buildId; }
        }

        public bool Succeeded
        {
            get { return _succeeded; }
        }

        public string StatusUrl
        {
            get { return _statusUrl; }
        }

        public string StatusHtml
        {
            get { return _statusHtml; }
        }

        public string Message
        {
            get { return _message; }
        }

        protected bool Equals(TeamCityBuildFinishedEvent other)
        {
            return _buildId == other._buildId && _succeeded.Equals(other._succeeded) && string.Equals(_statusUrl, other._statusUrl) && string.Equals(_statusHtml, other._statusHtml) && string.Equals(_message, other._message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TeamCityBuildFinishedEvent) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _buildId;
                hashCode = (hashCode*397) ^ _succeeded.GetHashCode();
                hashCode = (hashCode*397) ^ (_statusUrl != null ? _statusUrl.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_statusHtml != null ? _statusHtml.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_message != null ? _message.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}