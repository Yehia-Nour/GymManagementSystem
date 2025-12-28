using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GymManagement.DAL.DataSeed
{
    public static class GymDbContextDataSeeding
    {
        public static async Task<bool> SeedData(GymDbContext dbContext)
        {
            try
            {
                var hasPlans = await dbContext.Plans.AnyAsync();
                var hasCategories = await dbContext.Categories.AnyAsync();
                if (hasPlans && hasCategories)
                    return false;

                if (!hasPlans)
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (plans.Any())
                        await dbContext.Plans.AddRangeAsync(plans);
                }

                if (!hasCategories)
                {
                    var categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (categories.Any())
                        await dbContext.Categories.AddRangeAsync(categories);
                }

                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed {ex}");
                return false;
            }
        }

        private static List<T> LoadDataFromJsonFile<T>(string fileName)
        {
            // C:\Users\DELL\Desktop\MyProjectsInVS\GymManagementSystem\GymManagement.PL\
            // wwwroot\Files\
            // categories.json
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            string data = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
        }
    }
}
