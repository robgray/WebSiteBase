using System.Reflection;
using Domain.Entities;

namespace Tests.Common
{
    public static class TestHelpers
    {
        public static TModel WithId<TModel>(this TModel model, int id) where TModel : IHasId
        {
            var idProp = model.GetType().GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);

            idProp.SetValue(model, id);

            return model;
        }
    }
}
