# rpg_stat_editor

This is a basic editor for editing stats on game entities.
The StatManager uses regular expressions to parse the entities, alternatively we could use tokenization if performance becomes a problem. For now we build aganst an abstraction by using IStatManager within the editor.
The PersistenceManager uses a file storage, again we use an interface in our implementation so this can easily be switched out to a REST api or similar.

# TODO List
- Setup IoC container (PRISM/Unity)
- Add data validation
- Increase unit test coverage
- Look into multiple inheritence (_Orc > Red Ork > Big Red Ork)
- Look into tokenized parser: https://en.wikipedia.org/wiki/Lexical_analysis#Tokenization
