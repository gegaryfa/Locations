using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Locations.Core.Domain.Entities;
using Locations.Infrastructure.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Locations.WebApi.TestData
{
    public class DataGenerator
    {
        private const string TestDataFileName = "locations";
        private static bool InDocker => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

        public static async Task InitializeInMemoryDb(IServiceProvider serviceProvider)
        {
            await using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var locations = GetSeedLocations(TestDataFileName);

            // Look for any location.
            if (await context.Locations.AnyAsync())
            {
                return;   // Data was already seeded
            }

            await context.Locations.AddRangeAsync(locations);

            await context.SaveChangesAsync();
        }

        private static List<CsvFileValues> GetSeedLocations(string filename)
        {
            var pathToFile = ConstructPathToFile(filename);

            var values = File.ReadAllLines(pathToFile)
                .Skip(1) // skip the line with the titles
                .Select(CsvFileValues.FromCsv)
                .ToList();

            return values;
        }

        private static string ConstructPathToFile(string filename)
        {
            if (InDocker)
            {
                return $"TestData/Data/{filename}.csv";
            }

            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePathRelativeToAssembly = Path.Combine(assemblyPath, @$".\TestData/Data/{filename}.csv");
            var normalizedPath = Path.GetFullPath(filePathRelativeToAssembly);
            return normalizedPath;
        }
    }

    internal class CsvFileValues : Location
    {
        public static CsvFileValues FromCsv(string csvLine)
        {
            var values = csvLine.Split(',');
            //var valuesCount = values.Length;

            var longitude = Convert.ToDouble(values.Last().Replace("\"", string.Empty));
            values = values.Take(values.Length - 1).ToArray();

            var latitude = Convert.ToDouble(values.Last().Replace("\"", string.Empty));
            values = values.Take(values.Length - 1).ToArray();

            var address = string.Empty;
            var totalCount = values.Length;
            for (var count = 0; count < totalCount; count++)
            {
                var addressPart = values[count];

                // Append a comma on all part but the last.
                if ((count + 1) == totalCount)
                {
                    address += addressPart;
                }
                else
                {
                    address += addressPart + ",";
                }
            }

            address = Convert.ToString(address.Replace("\"", string.Empty));

            var csvFileValues = new CsvFileValues(address, latitude, longitude);
            return csvFileValues;
        }

        public CsvFileValues(string address, double latitude, double longitude) : base(address, latitude, longitude)
        {
        }
    }
}
