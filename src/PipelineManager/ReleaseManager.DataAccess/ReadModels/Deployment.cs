using System;

namespace ReleaseManager.DataAccess.ReadModels
{
    public class Deployment
    {
        public string UniqueId { get; set; }
        public string Environment { get; set; }
        public DateTime Date { get; set; }
        public bool Success { get; set; }
    }
}