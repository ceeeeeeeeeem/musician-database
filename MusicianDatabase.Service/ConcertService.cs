using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Logging;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Helpers;
using MusicianDatabase.Service.Interfaces;
using Snickler.EFCore;

namespace MusicianDatabase.Service
{
    public class ConcertService : IConcertService
    {
        private readonly MusicianDbContext _context;
        private readonly IMapper _mapper;

        // Has console logger, memory cache implemented. (console logger, memorycache defined at Program.cs)
        private readonly ILogger<ConcertService>  _logger;
        private readonly IMemoryCache _memoryCache;

        public ConcertService(MusicianDbContext context, IMapper mapper, ILogger<ConcertService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<bool> CreateConcert(ConcertCUDto concertDto)
        {
            var concert = _mapper.Map<Concert>(concertDto);
            _context.Concerts.Add(concert);
            int result = await _context.SaveChangesAsync();

            CacheHelper.RemoveByPattern(_memoryCache, "Concert");


            return result > 0;
        }

        public async Task<bool> DeleteConcert(int id)
        {
            var concert = await _context.Concerts.FindAsync(id);

            if (concert == null)
                return false;

            _context.Concerts.Remove(concert);

            int result = await _context.SaveChangesAsync();

            CacheHelper.RemoveByPattern(_memoryCache, "Concert");

            return result > 0;
        }

        public async Task<Concert> GetById(int id)
        {
            var cacheKey = $"Concert_{id}";

            if (_memoryCache.TryGetValue(cacheKey, out Concert cachedConcert))
            {
                _logger.LogInformation("GetById (concert) - Returning cached data for Id = {Id}", id);
                return cachedConcert;
            }

            var concert = await _context.Concerts.SingleOrDefaultAsync(a => a.Id == id);

            if (concert != null)
            {
                // Add the concert to cache with a specific cache duration
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _memoryCache.Set(cacheKey, concert, cacheEntryOptions);
                _logger.LogInformation("GetById (concert) - Added data to cache for Id = {Id}", id);

                return concert;
            }

            return null;
        }

        public async Task<List<ConcertCountDto>> GetConcertCountsBetweenDates(DateTime startDate, DateTime endDate)
        {
            // cacheKey starts with Concerts (plural) for ease of removal from memory.
            var cacheKey = $"ConcertCounts_{startDate}_{endDate}";

            if (_memoryCache.TryGetValue(cacheKey, out List<ConcertCountDto> cachedData))
            {
                _logger.LogInformation("GetConcertCountsBetweenDates - Returning cached data for StartDate: {StartDate} and EndDate: {EndDate}", startDate, endDate);
                return cachedData;
            }
            _logger.LogInformation("GetConcertCountsBetweenDates - Data not found in cache. Querying the database for StartDate: {StartDate} and EndDate: {EndDate}", startDate, endDate);


            var result = new List<ConcertCountDto>();

            _logger.LogInformation("GetConcertCountsBetweenDates called with StartDate: {StartDate} and EndDate: {EndDate}", startDate, endDate);
            

            await Task.Run(() =>
            {
                _context.LoadStoredProc("GetConcertCountsBetweenDates", false)
                    .WithSqlParam("StartDate", startDate)
                    .WithSqlParam("EndDate", endDate)
                    .ExecuteStoredProc((handler) =>
                    {
                        result = handler.ReadToList<ConcertCountDto>().ToList();
                    });
            });


            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2) // Set an appropriate cache duration
            };

            _memoryCache.Set(cacheKey, result, cacheEntryOptions);
            _logger.LogInformation("Added CacheKey ConcertCounts using: {StartDate} and EndDate: {EndDate}", startDate, endDate);



            return result;
        }

        public async Task<List<Concert>> GetList()
        {
            var cacheKey = $"ConcertsCache";
            if (_memoryCache.TryGetValue(cacheKey, out List<Concert> concerts))
            {
                _logger.LogInformation("Returning cached Concerts data");
                return concerts;  
            };

            concerts = await _context.Concerts.ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Set an appropriate cache duration
            };

            _memoryCache.Set(cacheKey, concerts, cacheEntryOptions);
            _logger.LogInformation("Added CacheKey ConcertsCache");

            return concerts;
        }

        public async Task<bool> QuickCreateConcert(ConcertQuickCreateDto concertQCDto)
        {
            var concertNew = _mapper.Map<Concert>(concertQCDto);

            // Ensure that ConcertBands is initialized as a collection
            concertNew.ConcertBands = new List<ConcertBand>();

            _context.Concerts.Add(concertNew);

            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                // Now that the Concert entity is saved and has an Id, you can create ConcertBand
                concertNew.ConcertBands.Add(new ConcertBand
                {
                    ConcertId = concertNew.Id,
                    BandId = concertQCDto.BandId,
                });

                // Save changes again to add the ConcertBand
                int result2 = await _context.SaveChangesAsync();

                if (result2 > 0)
                {
                    CacheHelper.RemoveByPattern(_memoryCache, "Concert"); // Will also remove ConcertBand caches, as intended.
                    return true;
                }
            }

            return false;
        }



        public async Task<bool> UpdateConcert(int id, ConcertCUDto concertUpdateDto)
        {
            var concert = await _context.Concerts.SingleOrDefaultAsync(a => a.Id == id);

            if (concert == null)
                return false;

            // Check if the provided VenueId exists in the Venues table
            var existingVenue = await _context.Venues.SingleOrDefaultAsync(v => v.Id == concertUpdateDto.VenueId);

            if (existingVenue == null)
            {
                // Handle the case where the VenueId doesn't exist
                // You can return an error message or throw an exception
                return false;
            }

            // Map and update the Concert entity
            _mapper.Map(concertUpdateDto, concert);
            _context.Entry(concert).State = EntityState.Modified;

            int result = await _context.SaveChangesAsync();
            CacheHelper.RemoveByPattern(_memoryCache, "Concert");

            return result > 0;
        }

    }
}
