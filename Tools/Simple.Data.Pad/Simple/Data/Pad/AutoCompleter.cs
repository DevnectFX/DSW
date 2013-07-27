using Simple.Data.Ado;
using Simple.Data.Ado.Schema;
using Simple.Data.Interop;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Simple.Data.Pad
{
    public class AutoCompleter
    {
        private static readonly Regex NonAlphaNumeric = new Regex("[^0-9a-zA-Z]+");
        private static readonly string[] Empty = new string[0];
        private readonly ISchemaProvider _schemaProvider;
        private readonly ConcurrentDictionary<string, string[]> _cache = new ConcurrentDictionary<string, string[]>();
        private static readonly ConcurrentDictionary<string, string> PrettifiedCache = new ConcurrentDictionary<string, string>();
        public AutoCompleter(DataStrategy database)
        {
            if (database != null)
            {
                AdoAdapter adoAdapter = database.GetAdapter() as AdoAdapter;
                if (adoAdapter != null)
                {
                    this._schemaProvider = adoAdapter.SchemaProvider;
                }
            }
        }
        public AutoCompleter(ISchemaProvider schemaProvider)
        {
            this._schemaProvider = schemaProvider;
        }
        public IEnumerable<string> GetOptions(string currentText)
        {
            if (this._schemaProvider == null || string.IsNullOrWhiteSpace(currentText) || !currentText.Contains("."))
            {
                return AutoCompleter.Empty;
            }
            string[] array = this._cache.GetOrAdd(currentText, new Func<string, string[]>(this.GetOptionsImpl));
            if (array.Length == 1 && currentText.Substring(currentText.LastIndexOf('.') + 1).Equals(array[0], StringComparison.CurrentCultureIgnoreCase))
            {
                return AutoCompleter.Empty;
            }
            return array;
        }
        private string[] GetOptionsImpl(string currentText)
        {
            Token[] tokens = new Lexer(currentText).GetTokens().ToArray<Token>();
            if (tokens.Length < 2)
            {
                return AutoCompleter.Empty;
            }
            object db = tokens[0].Value;
            int current = tokens.Length - 1;
            if (tokens[current].Type != TokenType.Dot && tokens[current].Type != TokenType.Identifier)
            {
                return AutoCompleter.Empty;
            }
            string partial = string.Empty;
            if (tokens[current].Type == TokenType.Identifier)
            {
                partial = tokens[current].Value.ToString();
                current--;
            }
            if (tokens[current].Type == TokenType.Dot)
            {
                current--;
            }
            if (tokens[current].Type == TokenType.CloseParen)
            {
                return this.GetOptionsForMethodReturnType(tokens, current, false, partial).ToArray<string>();
            }
            if (tokens[current].Type != TokenType.Identifier)
            {
                return AutoCompleter.Empty;
            }
            string[] array = this.GetOptionsImpl(partial, tokens[current], db).ToArray<string>();
            if (array.Length == 1 && array[0].Equals(partial, StringComparison.CurrentCultureIgnoreCase))
            {
                return AutoCompleter.Empty;
            }
            return array;
        }
        private IEnumerable<string> GetOptionsForMethodReturnType(Token[] tokens, int current, bool methodChainIncludesOrderBy, string partial)
        {
            current = Lexer.FindIndexOfOpeningToken(tokens, current, TokenType.OpenParen);
            if (--current < 0)
            {
                return AutoCompleter.Empty;
            }
            if (tokens[current].Type != TokenType.Identifier)
            {
                return AutoCompleter.Empty;
            }
            if (AutoCompleter.IsAMethodThatReturnsAQuery(tokens[current].Value.ToString()))
            {
                current -= 2;
                if (tokens[current].Type != TokenType.Identifier)
                {
                    return AutoCompleter.Empty;
                }
                IEnumerable<string> options = this.QueryOptions(tokens[current].Value.ToString(), methodChainIncludesOrderBy);
                if (!string.IsNullOrWhiteSpace(partial))
                {
                    options =
                        from s in options
                        where s.StartsWith(partial, StringComparison.CurrentCultureIgnoreCase)
                        select s;
                }
                return options;
            }
            else
            {
                string methodName = tokens[current].Value.ToString();
                if (methodName.Equals("join", StringComparison.CurrentCultureIgnoreCase))
                {
                    return new string[]
					{
						"On"
					};
                }
                methodChainIncludesOrderBy = (methodChainIncludesOrderBy || methodName.StartsWith("OrderBy", StringComparison.CurrentCultureIgnoreCase));
                current--;
                if (tokens[current].Type == TokenType.Dot)
                {
                    current--;
                }
                if (tokens[current].Type == TokenType.CloseParen)
                {
                    return this.GetOptionsForMethodReturnType(tokens, current, methodChainIncludesOrderBy, partial);
                }
                return AutoCompleter.Empty;
            }
        }
        private static bool IsAMethodThatReturnsAQuery(string identifier)
        {
            return identifier.StartsWith("FindAll", StringComparison.CurrentCultureIgnoreCase) || identifier.Equals("All", StringComparison.CurrentCultureIgnoreCase) || identifier.StartsWith("Query", StringComparison.CurrentCultureIgnoreCase);
        }
        private IEnumerable<string> GetOptionsImpl(string partial, Token token, object db)
        {
            if (token.Value == db)
            {
                return
                    from s in this.DatabaseOptions().Select(new Func<string, string>(AutoCompleter.Prettify))
                    where s.StartsWith(partial, StringComparison.CurrentCultureIgnoreCase)
                    orderby s
                    select s;
            }
            return
                from s in this.TableOptions(token.Value.ToString())
                where s.StartsWith(partial, StringComparison.CurrentCultureIgnoreCase)
                orderby s
                select s;
        }
        private IEnumerable<string> TableOptions(string tableName)
        {
            Table table = (
                from t in this._schemaProvider.GetTables()
                where AutoCompleter.Prettify(t.ActualName) == AutoCompleter.Prettify(tableName)
                select t).SingleOrDefault<Table>();
            if (table != null)
            {
                yield return "All";
                yield return "Query";
                foreach (string current in
                    from c in this._schemaProvider.GetColumns(table)
                    select AutoCompleter.Prettify(c.ActualName))
                {
                    yield return current;
                    yield return "FindBy" + current;
                    yield return "FindAllBy" + current;
                }
            }
            yield break;
        }
        private IEnumerable<string> QueryOptions(string tableName, bool includeThenBy)
        {
            yield return "Select";
            yield return "Where";
            yield return "ReplaceWhere";
            if (includeThenBy)
            {
                yield return "ThenBy";
                yield return "ThenByDescending";
            }
            else
            {
                yield return "OrderBy";
                yield return "OrderByDescending";
            }
            yield return "Skip";
            yield return "Take";
            yield return "Join";
            Table table = (
                from t in this._schemaProvider.GetTables()
                where AutoCompleter.Prettify(t.ActualName) == AutoCompleter.Prettify(tableName)
                select t).SingleOrDefault<Table>();
            if (table != null)
            {
                foreach (string current in
                    from c in this._schemaProvider.GetColumns(table)
                    select AutoCompleter.Prettify(c.ActualName))
                {
                    if (includeThenBy)
                    {
                        yield return "ThenBy" + current;
                        yield return "ThenBy" + current + "Descending";
                    }
                    else
                    {
                        yield return "OrderBy" + current;
                        yield return "OrderBy" + current + "Descending";
                    }
                }
            }
            yield break;
        }
        private IEnumerable<string> DatabaseOptions()
        {
            foreach (Table current in this._schemaProvider.GetTables())
            {
                yield return current.ActualName;
            }
            foreach (Procedure current2 in this._schemaProvider.GetStoredProcedures())
            {
                yield return current2.Name;
            }
            yield break;
        }
        private static string Prettify(string source)
        {
            return AutoCompleter.PrettifiedCache.GetOrAdd(source, new Func<string, string>(AutoCompleter.PrettifyImpl));
        }
        private static string PrettifyImpl(string source)
        {
            if (!AutoCompleter.NonAlphaNumeric.IsMatch(source))
            {
                return source;
            }
            StringBuilder builder = new StringBuilder();
            string[] array = AutoCompleter.NonAlphaNumeric.Replace(source, " ").Split(new char[]
			{
				' '
			});
            for (int i = 0; i < array.Length; i++)
            {
                string word = array[i];
                builder.Append(char.ToUpper(word[0]));
                builder.Append(word.Substring(1).ToLower());
            }
            return builder.ToString();
        }
    }
}
