using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EncePence
{
    class MsSqlData : DataController
    {

        public Word[] GetAllWords()
        {
            Word[] words = new Word[] { };
            EncePence.Adapters.Word[] wordsEntity = new Adapters.Word[] { };
            using (var ctx = new Adapters.EncePenceEntities3())
            {
                wordsEntity = ctx.Words.ToArray();
                words = TransformObjects(wordsEntity);
            }
            return words;
        }

        private static Word[] TransformObjects(EncePence.Adapters.Word[] wordsEntity)
        {
            List<Word> words = new List<Word>();
            foreach (Adapters.Word item in wordsEntity)
            {
                EncePence.Word word = Adapters.WordAdapter.GetWord(item);
                words.Add(word);
            }
            return words.ToArray();
        }

        public Word[] GetAllWordsByCategory(Category category)
        {
            Word[] words = new Word[] { };
            Adapters.Word[] wordsEntity = new Adapters.Word[] { };
            Adapters.Category categoryEntity = Adapters.CategoryAdapter.GetCategoryEntity(category);
            using (var ctx = new Adapters.EncePenceEntities3())
            {
                wordsEntity = (from w in ctx.Words
                               where w.CategoryId == categoryEntity.CategoryId
                               select w).ToArray();
                words = TransformObjects(wordsEntity);
            }
            return words;
        }

        public Word[] GetAllWordsByFirstChar(char firstChar)
        {
            Word[] words = new Word[] { };
            Adapters.Word[] wordsEntity = new Adapters.Word[] { };
            using (var ctx = new Adapters.EncePenceEntities3())
            {
                wordsEntity = (from w in ctx.Words
                               where w.Name.StartsWith("w")
                               select w).ToArray();
                words = TransformObjects(wordsEntity);
            }
            return words;

        }

        public Category[] GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            using (var ctx = new Adapters.EncePenceEntities3())
            {
                Adapters.Category[] categoriesEntity = ctx.Categories.ToArray();
                foreach (var item in categoriesEntity)
                {
                    Category category = Adapters.CategoryAdapter.GetCategory(item);
                    categories.Add(category);
                }
            }
            return categories.ToArray();
        }

        public Word GetWord(int wordId)
        {
            Word resultWord;
            using (var ctx = new Adapters.EncePenceEntities3())
            {
                Adapters.Word word = (from w in ctx.Words
                                      where w.WordId == wordId
                                      select w).SingleOrDefault();
                resultWord = Adapters.WordAdapter.GetWord(word);
            }
            return resultWord;
        }

        public Word GetWord(string wordName)
        {
            Word resultWord;
            using (var ctx = new Adapters.EncePenceEntities3())
            {
                Adapters.Word word = (from w in ctx.Words
                                      where w.Name == wordName
                                      select w).SingleOrDefault();
                resultWord = Adapters.WordAdapter.GetWord(word);
            }
            return resultWord;
        }

        public Word[] SearchWord(string searchString)
        {
            Word[] words = new Word[] { };
            Adapters.Word[] wordsEntity = new Adapters.Word[] { };
            using (var ctx = new Adapters.EncePenceEntities3())
            {
                wordsEntity = (from w in ctx.Words
                               where w.Name.Contains(searchString)
                               select w).ToArray();
                words = TransformObjects(wordsEntity);
            }
            return words;
        }

        public Word[] GetFilteredWords(WordsFilter wordsFilter)
        {
            Word[] wordsInCategory;
            if (wordsFilter.Category is Category.AllCategories)
            {
                wordsInCategory = this.GetAllWords();
            }
            else
            {
                wordsInCategory = this.GetAllWordsByCategory(wordsFilter.Category);
            }
            Word[] wordsWithSearchText = (from w in wordsInCategory
                                          where w.Name.Contains(wordsFilter.SearchText)
                                          select w).ToArray();
            Word[] wordsWithFirstLetter = (from w in wordsWithSearchText
                                           where w.Name.StartsWith(wordsFilter.FirstLetter)
                                           select w).ToArray();
            return wordsWithFirstLetter;
        }

        public Word GetRandomWord()
        {
            Random r = new Random();
            Word[] words = this.GetAllWords();
            return GetRandomWord(words);
        }

        private static Word GetRandomWord(Word[] words)
        {
            Word resultWord;
            int idx = new Random().Next(words.Length);
            resultWord = words[idx];
            return resultWord;
        }

        public Word GetRandomWord(int[] disabledId)
        {
            Word resultWord;
            using (var ctx = new Adapters.EncePenceEntities3())
            {
                Word[] words = (from w in this.GetAllWords()
                                where !disabledId.Contains(w.Id)
                                select w).ToArray();

                resultWord = GetRandomWord(words);
            }
            return resultWord;

        }

        public Word GetRandomWord(int[] disabledId, int[] idsWithMostShows, int[] disabledCategoryIds)
        {
            Word resultWord;
            using (var ctx = new Adapters.EncePenceEntities3())
            {
                Word[] allWords = this.GetAllWords();
                Word[] words = (from w in allWords
                                where !disabledCategoryIds.Contains(w.Category.Id) &&
                                !disabledId.Contains(w.Id) &&
                                !idsWithMostShows.Contains(w.Id)
                                select w).ToArray();
                resultWord = GetRandomWord(words);
            }
            return resultWord;
        }

        public int GetWordsCount()
        {
            int count;
            using (var ctx = new Adapters.EncePenceEntities3())
            {
                count = ctx.Words.Count();
            }
            return count;
        }

        public int GetWordsCount(WordsFilter wordFilter)
        {
            int count = -1;
            using (var ctx = new Adapters.EncePenceEntities3())
            {
                if (wordFilter.Category is Category.AllCategories)
                {
                    count = (from w in ctx.Words
                             where w.Name.StartsWith(wordFilter.FirstLetter) &&
                                    w.Name.Contains(wordFilter.SearchText)
                             select w).Count();
                }
                else
                {
                    count = (from w in ctx.Words
                             where w.CategoryId == wordFilter.Category.Id &&
                             w.Name.StartsWith(wordFilter.FirstLetter) &&
                             w.Name.Contains(wordFilter.SearchText)
                             select w).Count();
                }
            }
            return count;

        }


        public Word[] GetRange(int startIdx, int count, WordsFilter wordFilter)
        {
            List<Word> words = new List<Word>();
            
            using (var ctx = new EncePence.Adapters.EncePenceEntities3())
            {
                List<Adapters.Word> wordsEntity = new List<Adapters.Word>();
                if (wordFilter.Category is Category.AllCategories)
                {
                    wordsEntity = (from w in ctx.Words
                                   orderby w.Name
                                   where w.Name.StartsWith(wordFilter.FirstLetter) &&
                                         w.Name.Contains(wordFilter.SearchText)
                                   select w).Skip(startIdx).Take(count).ToList();
                }
                else
                {
                    wordsEntity = (from w in ctx.Words
                                   orderby w.Name
                                   where w.CategoryId == wordFilter.Category.Id &&
                                         w.Name.StartsWith(wordFilter.FirstLetter) &&
                                         w.Name.Contains(wordFilter.SearchText)
                                   select w).Skip(startIdx).Take(count).ToList();
                }
                foreach (EncePence.Adapters.Word item in wordsEntity)
                {
                    Word word = EncePence.Adapters.WordAdapter.GetWord(item);
                    words.Add(word);
                }
            }
            return words.ToArray();
        }



        public int GetNextCategoryId()
        {
            throw new NotImplementedException();
        }

        public void AddCategory(int id, string name, string describtion)
        {
            throw new NotImplementedException();
        }


        public int GetNextWordId()
        {
            throw new NotImplementedException();
        }

        public void AddWord(int id, string name, string description, int p)
        {
            throw new NotImplementedException();
        }
    }
}
