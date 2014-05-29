using System;
using System.Collections.Generic;
using System.Linq;

namespace Pipelines
{
    public enum StepExecutionResult
    {
        Finished,
        WaitingForExternalDependency,
        Fail
    }
}