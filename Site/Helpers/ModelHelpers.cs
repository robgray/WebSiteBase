using Domain.Entities;

namespace WebBase.Mvc.Helpers
{
    public static class ModelHelpers
    {
        public static T WithDefaultImage<T>(this T entity) where T : IHasAnImage
        {
            if (string.IsNullOrWhiteSpace(entity.ImageUrl))
            {
                entity.ImageUrl = "/Images/defaultprofile64.png";
            }
            return entity;
        }
    }
}