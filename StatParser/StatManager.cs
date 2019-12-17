using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace StatParser
{
    public class StatManager
    {
        private const string RegEx = "\\\"([a-zA-Z0-9_ ;]+)\\\"|(\\w+)";

        public List<GameEntity> Deserialize(string data)
        {
            var output = new List<GameEntity>();
            var currentEntity = new GameEntity();
            using (var reader = new System.IO.StringReader(data))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var regex = new Regex(RegEx, RegexOptions.Compiled);

                    var matches = regex.Matches(line);

                    foreach (Match match in matches)
                    {
                        switch (match.Value)
                        {
                            case "new":
                                var next = match.NextMatch();
                                var after = next.NextMatch();
                                currentEntity = new GameEntity
                                {
                                    Name = after.Value.Trim('\"')
                                };
                                output.Add(currentEntity);
                                break;
                            case "type":
                                currentEntity.Type = match.NextMatch().Value.Trim('\"');
                                break;
                            case "using":
                                currentEntity.Using = match.NextMatch().Value.Trim('\"');
                                break;
                            case "data":
                                var dataType = match.NextMatch();
                                var dataValue = dataType.NextMatch();
                                currentEntity.Data.Add(dataType.Value.Trim('\"'), dataValue.Value.Trim('\"'));
                                break;
                        }
                    }
                }
            }

            return output;
        }

        public string Serialize(IList<GameEntity> entities)
        {
            var sb = new StringBuilder();
            foreach (var gameEntity in entities)
            {
                sb.AppendLine($"new entry \"{gameEntity.Name}\"");
                sb.AppendLine($"type \"{gameEntity.Type}\"");
                sb.AppendLine($"using \"{gameEntity.Using}\"");
                foreach (var data in gameEntity.Data)
                {
                    sb.AppendLine($"data \"{data.Key}\" \"{data.Value}\"");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}