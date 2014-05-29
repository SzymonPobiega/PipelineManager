using System;
using System.Collections.Generic;
using System.Globalization;
using Pipelines;
using Pipelines.Infrastructure;
using ReleaseManager.Events;
using ReleaseManager.Process.Octopus;

namespace ReleaseManager.Host.Models
{
    public class ReleaseCandidateHistoryModel
    {
        public List<HistoryItem> Items { get; set; }

        public ReleaseCandidateHistoryModel()
        {
            Items = new List<HistoryItem>();
        }

        public void On(EventEnvelope<ReleaseCandidateCreatedEvent> evnt)
        {
            AddItem(evnt, "Created");
        }

        public void On(EventEnvelope<ReleaseCandidateDeployedEvent> evnt)
        {
            AddItem(evnt, string.Format("Deployed to {0}", evnt.Payload.Environment));
        }

        public void On(EventEnvelope<TestSuiteFinishedEvent> evnt)
        {
            var itemType = HistoryItemType.Success;
            switch (evnt.Payload.Result)
            {
                case TestResult.Success:
                    itemType = HistoryItemType.Success;
                    break;
                case TestResult.Inconclusive:
                    itemType = HistoryItemType.Warning;
                    break;
                case TestResult.Failed:
                    itemType = HistoryItemType.Error;
                    break;
            }
            AddItem(evnt, string.Format("Finished executing {0} test suite", evnt.Payload.SuiteType), itemType);
        }

        public void On(EventEnvelope<StageFinishedEvent> evnt)
        {
            AddItem(evnt, string.Format("Finished stage {0}", evnt.Payload.StageId.StageId));
        }

        public void On(EventEnvelope<DeploymentRequestedEvent> evnt)
        {
            AddItem(evnt, string.Format("Scheduled deployment to {0}", evnt.Payload.Environment));
        }

        private void AddItem<T>(EventEnvelope<T> evnt, string message, HistoryItemType type = HistoryItemType.Success)
            where T : class 
        {
            Items.Add(new HistoryItem()
            {
                Date = FormatDate(evnt.OccurenceDateUtc),
                Message = message,
                Type = type
            });
        }

        private string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy'-'MM'-'dd HH':'mm':'ss",CultureInfo.InvariantCulture);
        }
    }
}