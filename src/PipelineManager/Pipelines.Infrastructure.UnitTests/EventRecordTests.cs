using System;
using NUnit.Framework;
using Pipelines.Infrastructure.Records;
using Ploeh.AutoFixture;

namespace Pipelines.Infrastructure.UnitTests
{
    [TestFixture]
    public class EventRecordTests
    {
        [Test]
        public void It_can_serialize_and_deserialize_payload_with_private_readonly_fields()
        {
            var fixture = new Fixture();
            var pipelineId = fixture.Create<string>();
            var sequence = fixture.Create<int>();
            var encapsulatedValue = fixture.Create<string>();

            var payload = new EncapsulatedFieldPayload(encapsulatedValue);

            var evnt = new EventRecord(pipelineId, sequence, payload, DateTime.UtcNow);

            var deserializedPayload = (EncapsulatedFieldPayload) evnt.DeserializePayload();

            Assert.AreEqual(encapsulatedValue, deserializedPayload.EncapsulatedValue);
        }

        public class EncapsulatedFieldPayload
        {
            private readonly string _encapsulatedValue;

            public EncapsulatedFieldPayload(string encapsulatedValue)
            {
                _encapsulatedValue = encapsulatedValue;
            }

            public string EncapsulatedValue
            {
                get { return _encapsulatedValue; }
            }
        }
    }
}
