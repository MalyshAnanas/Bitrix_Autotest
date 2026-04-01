namespace Demo.BaseFramework
{
    class CaseCollectionCreator
    {
        public List<ExecutableTestCase> AllCaseCollection { get; } = new List<ExecutableTestCase>();

        public CaseCollectionCreator()
        {
            TestCaseCollectionBuilder.ActivateProvidersInstances(AllCaseCollection);
        }
    }
}
