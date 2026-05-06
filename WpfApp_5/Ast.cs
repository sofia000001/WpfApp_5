using System.Collections.Generic;

namespace WpfApp_5   
{
    public abstract class AstNode
    {
        public List<AstNode> Children { get; set; } = new List<AstNode>();
    }

    public class ProgramNode : AstNode { }

    public class StringConstDeclNode : AstNode
    {
        public string Name { get; set; }
        public string Type { get; set; } = "String";
        public string Value { get; set; }
    }

    public static class AstPrinter
    {
        public static string Print(AstNode node, string indent = "")
        {
            var sb = new System.Text.StringBuilder();

            if (node is ProgramNode)
                sb.AppendLine("Program");

            if (node is StringConstDeclNode str)
            {
                sb.AppendLine($"{indent}StringConstDeclNode");
                sb.AppendLine($"{indent}├── name: {str.Name}");
                sb.AppendLine($"{indent}├── type: {str.Type}");
                sb.AppendLine($"{indent}└── value: {str.Value}");
            }

            foreach (var child in node.Children)
                sb.Append(Print(child, indent + "  "));

            return sb.ToString();
        }
    }
}