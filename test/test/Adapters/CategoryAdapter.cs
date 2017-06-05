
namespace EncePence.Adapters
{
    /// <summary>
    /// Static class used to translate Category class to entity framework class and vice versa.
    /// </summary>
    static class CategoryAdapter
    {
        /// <summary>
        /// Static method to translate Category to entity framework class.
        /// </summary>
        /// <param name="category">Category class object</param>
        /// <returns>Entity framework Category class object</returns>
        public static EncePence.Adapters.Category GetCategoryEntity(EncePence.Category category)
        {
            EncePence.Adapters.Category categoryEntity = new Category()
            {
                CategoryId = category.Id,
                Name = category.Name,
                Describtion = category.Describtion
            };
            return categoryEntity;
        }

        /// <summary>
        /// Static method to translate entity framework Category class to EncePence.Category class 
        /// </summary>
        /// <param name="categoryEntity">Entity framework Category class object</param>
        /// <returns>EncePence.Category class object</returns>
        public static EncePence.Category GetCategory(EncePence.Adapters.Category categoryEntity)
        {
            EncePence.Category category = new EncePence.Category(categoryEntity.CategoryId,
                categoryEntity.Name, categoryEntity.Describtion, 
                CategoryVisibility.Instance.IsEnable(categoryEntity.CategoryId));
            return category;
        }
    }
}
