namespace EncePence
{
    interface DataController
    {
        Word[] GetAllWords();
        Word[] GetAllWordsByCategory(Category category);
        Word[] GetAllWordsByFirstChar(char firstChar);
        Category[] GetAllCategories();
        Word GetWord(int wordId);
        Word GetWord(string wordName);
        Word[] SearchWord(string searchString);
        Word[] GetFilteredWords(WordsFilter wordsFilter);
        Word GetRandomWord();
        Word GetRandomWord(int[] disabledId);
        Word GetRandomWord(int[] disabledWordIds, int[] idsWithMostShows, int[] disabledCategoryIds);
        int GetWordsCount();
        int GetWordsCount(WordsFilter wordsFilter);
        Word[] GetRange(int startIdx, int count, WordsFilter wordFilter);

        int GetNextCategoryId();
        void AddCategory(int id, string name, string describtion);

        int GetNextWordId();

        void AddWord(int id, string name, string description, int p);
    }
}
