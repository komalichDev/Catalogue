using Common.Types;
using Mapster;

namespace Common.Config
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<ProductId, ProductId>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<CategoryId, CategoryId>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<DescriptionId, DescriptionId>.NewConfig().MapWith(src => src);
            TypeAdapterConfig.GlobalSettings.Default.MapToConstructor(true);
        }
    }
}
