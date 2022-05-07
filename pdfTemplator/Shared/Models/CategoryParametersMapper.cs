using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public static class CategoryParametersMapper
    {
        public static Category ToCategory(this CategoryParameters categoryParams)
        {
            return new Category
            {
                Name = categoryParams.Name,
                Id = categoryParams.Id,
            };
        }

        public static CategoryParameters ToCategoryParameters(this Category category)
        {
            return new CategoryParameters
            {
                Name = category.Name,
                Id = category.Id,
            };
        }
    }
}
