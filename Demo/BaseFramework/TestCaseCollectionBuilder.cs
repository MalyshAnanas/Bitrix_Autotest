using Demo.BaseFramework.LogTools;
using System.Reflection;

namespace Demo.BaseFramework
{
    public abstract class TestCaseCollectionBuilder
    {
        List<ExecutableTestCase> CaseCollection { get; } = new List<ExecutableTestCase>();

        public TestCaseCollectionBuilder()
        {
            CaseCollection.AddRange(GetCases());
        }

        abstract protected List<ExecutableTestCase> GetCases();

        public static void ActivateProvidersInstances(List<ExecutableTestCase> resultCaseCollection)
        {
            IEnumerable<Type> subclassTypes = Assembly
                .GetAssembly(typeof(TestCaseCollectionBuilder))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(TestCaseCollectionBuilder)));

            foreach (var subClassType in subclassTypes)
            {
                try
                {
                    var instance = Activator.CreateInstance(subClassType) as TestCaseCollectionBuilder;
                    resultCaseCollection.AddRange(instance.CaseCollection);
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            }
        }
    }
}
