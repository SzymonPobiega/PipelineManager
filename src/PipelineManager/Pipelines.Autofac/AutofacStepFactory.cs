using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Autofac;
using Autofac.Core;

namespace Pipelines.Autofac
{
    public class AutofacStepFactory : IStepFactory
    {
        private readonly ILifetimeScope _container;
        private readonly List<NamedParameter> _configParameters = new List<NamedParameter>(); 

        public AutofacStepFactory(ILifetimeScope lifetimeScope)
        {
            _container = lifetimeScope;
        }


        public BaseStep CreateInstance(Type stepType, UniqueStepId stepId)
        {
            return (BaseStep) _container.Resolve(stepType, AllParameters(stepId));
        }

        private IEnumerable<Parameter> AllParameters(UniqueStepId stepId)
        {
            yield return new TypedParameter(typeof (UniqueStepId), stepId);
            foreach (var configParameter in _configParameters)
            {
                yield return configParameter;
            }
        }

        public void AddConfigurationValue(string propertyName, object propertyValue)
        {
            _configParameters.Add(new NamedParameter(propertyName, propertyName));
        }
    }
}
