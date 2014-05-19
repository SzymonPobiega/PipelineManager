using System;
using System.Collections.Generic;
using System.Linq;
using Pipelines.Schema;

namespace Pipelines
{
    public class CodeBasedTypeResolver : IPipelineTypeResolver
    {
        private readonly List<Tuple<Func<string, bool>,PipelineSchema>> _schemas = new List<Tuple<Func<string, bool>, PipelineSchema>>();  

        public CodeBasedTypeResolver RegisterType(Func<string, bool> pipelineIdMatcher, PipelineSchema schema)
        {
            if (pipelineIdMatcher == null) throw new ArgumentNullException("pipelineIdMatcher");
            if (schema == null) throw new ArgumentNullException("schema");

            _schemas.Add(Tuple.Create(pipelineIdMatcher, schema));
            return this;
        }

        public PipelineSchema ResolveType(string pipelineId)
        {
            return _schemas.First(x => x.Item1(pipelineId)).Item2;

        }
    }
}