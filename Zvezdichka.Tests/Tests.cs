using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Tests
{
    public class Tests
    {
        private static bool testsInitialized = false;

        public static void Initialize()
        {
            if (!testsInitialized)
            {
                Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
                testsInitialized = true;
            }
        }

        public static ZvezdichkaDbContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<ZvezdichkaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ZvezdichkaDbContext(dbOptions);
        }
    }
}
