using System.Text.RegularExpressions;

namespace Karakatsiya.Services
{
    public class HtmlValidator
    {
        private static readonly HashSet<string> AllowedTags = new HashSet<string> { "a", "code", "i", "strong", "p", "b" };

        private static readonly HashSet<string> AllowedAttributes = new HashSet<string> { "href", "title" };

        public ValidationResult ValidateHtml(string input)
        {
            var tagPattern = @"<(/?)(\w+)([^>]*)>";
            var matches = Regex.Matches(input, tagPattern);

            foreach (Match match in matches)
            {
                string tagName = match.Groups[2].Value.ToLower();

                if (!AllowedTags.Contains(tagName))
                {
                    return new ValidationResult(false, $"Недопустимый тег: <{tagName}>");
                }

                string attributes = match.Groups[3].Value;
                var attributePattern = "(\\w+)=\"[^\"]*\"";
                var attributeMatches = Regex.Matches(attributes, attributePattern);

                foreach (Match attributeMatch in attributeMatches)
                {
                    string attributeName = attributeMatch.Groups[1].Value.ToLower();

                    if (!AllowedAttributes.Contains(attributeName))
                    {
                        return new ValidationResult(false, $"Недопустимый атрибут: {attributeName}");
                    }

                    if (attributeName == "onclick" || attributeName == "onload")
                    {
                        return new ValidationResult(false, $"Запрещенный атрибут: {attributeName}");
                    }
                }
            }

            var openTags = new Stack<string>();
            foreach (Match match in matches)
            {
                string tagName = match.Groups[2].Value.ToLower();
                if (match.Groups[1].Value == "/")
                {
                    if (openTags.Count == 0 || openTags.Pop() != tagName)
                    {
                        return new ValidationResult(false, "Неправильная вложенность тегов");
                    }
                }
                else
                {
                    openTags.Push(tagName);
                }
            }

            if (openTags.Count > 0)
            {
                return new ValidationResult(false, "Не закрыты все теги");
            }

            return new ValidationResult(true, "HTML корректен");
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; }
        public string ErrorMessage { get; }

        public ValidationResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }
    }
}