using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StatParser
{
    public interface IStatManager
    {
        List<GameEntity> Deserialize(string data);
        string Serialize(IList<GameEntity> entities);
    }

    public class StatManager : IStatManager
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
                                var entityName = after.Value.Trim('\"');
                                currentEntity = new GameEntity
                                {
                                    Name = entityName
                                };
                                output.Add(currentEntity);
                                break;
                            case "type":
                                currentEntity.Type = match.NextMatch().Value.Trim('\"');
                                break;
                            case "using":
                                var entity = match.NextMatch().Value.Trim('\"');
                                currentEntity.Using = entity;
                                var parent = output.FirstOrDefault(o => o.Name == entity);
                                if (parent != null)
                                {
                                    currentEntity.Data = new Dictionary<string, string>(parent.Data);
                                }

                                break;
                            case "data":
                                var dataType = match.NextMatch();
                                var dataValue = dataType.NextMatch();
                                var key = dataType.Value.Trim('\"');
                                var value = dataValue.Value.Trim('\"');
                                currentEntity.Data[key] = value;
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
                GameEntity parentEntity = null;
                if (!string.IsNullOrWhiteSpace(gameEntity.Using))
                {
                    parentEntity = entities.SingleOrDefault(e => e.Name == gameEntity.Using);
                }

                foreach (var data in gameEntity.Data)
                {
                    if (parentEntity == null
                        || !parentEntity.Data.ContainsKey(data.Key)
                        || parentEntity.Data[data.Key] != data.Value)
                    {
                        sb.AppendLine($"data \"{data.Key}\" \"{data.Value}\"");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString().Trim();
        }
    }
}