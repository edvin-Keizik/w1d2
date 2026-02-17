using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic.ChainOfResponsibility
{
    public class FilterPipeline
    {
        private readonly List<IFilterStep> _steps = new();

        public void AddStep(IFilterStep step) => _steps.Add(step);

        public bool Execute(FilterContext context, string signature)
        {
            int index = 0;

            bool Next()
            {

                if (index >= _steps.Count)
                {
                    return true;
                }

                var step = _steps[index];
                index++;

                return step.Handle(context, signature, Next);
            }

            return Next();
        }
    }
}
