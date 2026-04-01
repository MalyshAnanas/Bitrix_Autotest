namespace Demo.BaseFramework
{
    public class ExecutableTestCaseTreeNode
    {
        public ExecutableTestCaseTreeNode(string title)
        {
            Title = title;
        }

        public string Title { get; set; }
        public bool IsChecked { get; set; }
    }
}
