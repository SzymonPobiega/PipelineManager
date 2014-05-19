using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Pipelines.Infrastructure.Records;
using Ploeh.AutoFixture;

namespace Pipelines.Infrastructure.UnitTests
{
    [TestFixture]
    public class CommandRecordTests
    {
        [Test]
        public void It_can_serialize_and_deserialize_payload_with_private_readonly_fields()
        {
            var fixture = new Fixture();
            var encapsulatedValue = fixture.Create<string>();
            var enqueueDate = fixture.Create<DateTime>();

            var payload = new TestCommand(new CommandArgument()
            {
                SomeValue = encapsulatedValue
            });

            var record = new CommandRecord(payload, enqueueDate);

            var deserializedPayload = (TestCommand)record.DeserializePayload();

            Assert.AreEqual(encapsulatedValue, ((CommandArgument)deserializedPayload.Argument).SomeValue);
        }

        public class TestCommand : Command
        {
            [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
            private readonly object _argument;

            public TestCommand(object argument)
            {
                _argument = argument;
            }

            public object Argument
            {
                get { return _argument; }
            }

            public override void Execute(IPipelineHost pipelineHost)
            {
            }
        }

        public class CommandArgument
        {
            public string SomeValue { get; set; }
        }
    }
}