using AnagramSolver.BusinessLogic.ChainOfResponsibility.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.ChainOfResponsibility
{
    public class FilterPipelineFactory
    {
        public static FilterPipeline Create()
        {
            var pipeline = new FilterPipeline();

            pipeline.AddStep(new WordLengthStep());
            pipeline.AddStep(new AllowedCharactersStep());

            return pipeline;
        }
    }
}
