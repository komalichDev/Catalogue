using DatabaseAccess.RepositoryModel;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DatabaseAccess.Helpers
{
    public static class ProductFilterBuilder
    {
        public static IQueryable<Entity.Product> ApplyFilter(this IQueryable<Entity.Product> query, ProductFilter filter)
        {
            if(filter == null)
            {
                return query;
            }

            if (filter.SearchTitle && !string.IsNullOrEmpty(filter.SearchText))
            {
                query = query.Where(p => p.Name.Contains(filter.SearchText));
            }

            if(filter.SearchLongDescription && !string.IsNullOrEmpty(filter.SearchText))
            {
                query = query.Where(p => p.Description.DetailedText.Contains(filter.SearchText));
            }

            if (filter.SearchShortDescription && !string.IsNullOrEmpty(filter.SearchText))
            {
                query = query.Where(p => p.Description.ShortSummary.Contains(filter.SearchText));
            }

            if (filter.SearchByCategory && filter.SelectedCategoryIds != null && filter.SelectedCategoryIds.Any())
            {
                query = query.Where(p => filter.SelectedCategoryIds.Contains(p.CategoryId));
            }

            if (filter.SearchByPrice)
            {
                query = query.Where(p => p.Price > filter.MinPrice && p.Price < filter.MaxPrice);
            }

            if (filter.SearchByWeight)
            {
                query = query.Where(p => p.Description.WeightInGrams > filter.MinWeight && p.Description.WeightInGrams < filter.MaxWeight);
            }

            return query;
        }
    }
}
