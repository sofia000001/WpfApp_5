using System.Collections.Generic;

namespace WpfApp_5
{
    public class Parser
    {
        private List<Token> tokens;
        private int currentPos;
        private Token currentToken;

        private SymbolTable symbolTable = new SymbolTable();

        public List<SyntaxError> syntaxErrors = new List<SyntaxError>();
        public List<SemanticError> semanticErrors = new List<SemanticError>();

        public AstNode Root { get; private set; }

        private const int CODE_STRING = 1;
        private const int CODE_IDENTIFIER = 3;
        private const int CODE_KEYWORD = 4;
        private const int CODE_ASSIGN = 5;
        private const int CODE_SEMICOLON = 6;
        private const int CODE_SPACE = 7;


        public void Parse(List<Token> tokens)
        {
            this.tokens = tokens;
            currentPos = 0;

            syntaxErrors.Clear();
            semanticErrors.Clear();
            symbolTable = new SymbolTable();

            var root = new ProgramNode();

            while (currentPos < tokens.Count)
            {
                int start = currentPos;

                SkipSpaces();

                if (currentToken == null)
                    break;

                var node = ParseDeclaration();

                if (node != null)
                    root.Children.Add(node);

                if (currentPos == start)
                    currentPos++;
            }

            Root = root;
        }


        private void SkipSpaces()
        {
            while (currentPos < tokens.Count &&
                   tokens[currentPos].Code == CODE_SPACE)
            {
                currentPos++;
            }

            currentToken = currentPos < tokens.Count
                ? tokens[currentPos]
                : null;
        }

        private void NextToken()
        {
            currentPos++;
            SkipSpaces();
        }

        private void SkipToSemicolon()
        {
            while (currentToken != null &&
                   currentToken.Code != CODE_SEMICOLON)
            {
                currentPos++;
                SkipSpaces();
            }

            if (currentToken != null &&
                currentToken.Code == CODE_SEMICOLON)
            {
                currentPos++;
                SkipSpaces();
            }
        }


        private AstNode ParseDeclaration()
        {
            var node = new StringConstDeclNode();

            // --- String ---
            if (currentToken != null &&
                currentToken.Code == CODE_KEYWORD &&
                currentToken.Value == "String")
            {
                NextToken();
            }
            else
            {
                AddSyntaxError("Ожидалось 'String'");
                SkipToSemicolon();
                return null;
            }

            // имя 
            if (currentToken != null &&
                currentToken.Code == CODE_IDENTIFIER)
            {
                node.Name = currentToken.Value;

                if (!symbolTable.Declare(node.Name, "String"))
                {
                    semanticErrors.Add(new SemanticError
                    {
                        Message = $"Ошибка: идентификатор \"{node.Name}\" уже объявлен",
                        Line = currentToken.Line,
                        Position = currentToken.StartPos
                    });

                    SkipToSemicolon();
                    return null;
                }

                NextToken();
            }
            else
            {
                AddSyntaxError("Ожидался идентификатор");
                SkipToSemicolon();
                return null;
            }

            // = 
            if (currentToken != null &&
                currentToken.Code == CODE_ASSIGN)
            {
                NextToken();
            }
            else
            {
                AddSyntaxError("Ожидался '='");
                SkipToSemicolon();
                return null;
            }

            // значение 
            if (currentToken != null &&
                currentToken.Code == CODE_STRING)
            {
                node.Value = currentToken.Value;
                NextToken();
            }
            else
            {
                AddSyntaxError("Ожидалась строка");
                SkipToSemicolon();
                return null;
            }

            // ; 
            if (currentToken != null &&
                currentToken.Code == CODE_SEMICOLON)
            {
                NextToken();
            }
            else
            {
                AddSyntaxError("Ожидался ';'");
                SkipToSemicolon();
                return null;
            }

            return node;
        }


        private void AddSyntaxError(string message)
        {
            syntaxErrors.Add(new SyntaxError(
                currentToken?.Value ?? "",
                currentToken?.Line ?? 1,
                currentToken?.StartPos ?? 1,
                message));
        }
    }
}