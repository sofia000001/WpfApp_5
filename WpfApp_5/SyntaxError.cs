namespace WpfApp_5
{
    public class SyntaxError
    {
        public string Fragment { get; set; }
        public int Line { get; set; }
        public int Position { get; set; }
        public string Description { get; set; }

        public string Location => $"строка {Line}, позиция {Position}";

        public SyntaxError(string fragment, int line, int position, string description)
        {
            Fragment = fragment;
            Line = line;
            Position = position;
            Description = description;
        }
    }
}