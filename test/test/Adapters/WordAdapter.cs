namespace EncePence.Adapters
{
    /// <summary>
    /// WordAdapter is a static class to translate Word class object to entity framework Word class object and vice versa
    /// </summary>
    static class WordAdapter
    {
        /// <summary>
        /// Method to translate entity framework Word object to EncePence.Word object.
        /// </summary>
        /// <param name="wordEntity">Entity framework Word object class</param>
        /// <returns>EncePence.Word object class</returns>
        public static EncePence.Word GetWord(EncePence.Adapters.Word wordEntity)
        {
            EncePence.Category category = Adapters.CategoryAdapter.GetCategory(wordEntity.Category);
            EncePence.Word word = new EncePence.Word(wordEntity.WordId, wordEntity.Name,
                wordEntity.Describtion, category, WordsVisibility.Instance.IsEnable(wordEntity.WordId));
            return word;
        }


        /// <summary>
        /// Method to translate EncePence.Word object class to entity framework Word class.
        /// </summary>
        /// <param name="word">EncePence.Word object class</param>
        /// <returns>Entity framework Word class.</returns>
        public static EncePence.Adapters.Word GetWordEntity(EncePence.Word word)
        {
            EncePence.Adapters.Category categoryEntity = CategoryAdapter.GetCategoryEntity(word.Category);
            EncePence.Adapters.Word wordEntity = new Word()
            {
                WordId = word.Id,
                Name = word.Name,
                Describtion =word.Description,
                Category = categoryEntity,
                CategoryId = word.Category.Id
            };
            return wordEntity;
        }
    }
}
