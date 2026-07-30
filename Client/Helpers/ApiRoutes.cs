namespace Client.Helpers;

public static class ApiRoutes
{
    public static class Product
    {
        public const string Base = "api/Product";

        public static string ById(int id) => $"{Base}/Product/{id}";
    }

    public static class Category
    {
        public const string Base = "api/Product/Category";

        public static string ById(int id) => $"{Base}/{id}";
    }

    public static class Description
    {
        public const string Base = "api/Product/Description";

        public static string ById(int id) => $"{Base}/{id}";
    }
}