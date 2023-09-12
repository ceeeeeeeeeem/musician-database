using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Helpers;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Service
{
    public class ConcertBandService : IConcertBandService
    {
        private readonly MusicianDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ConcertBandService> _logger;
        private readonly IMemoryCache _memoryCache;

        public ConcertBandService(MusicianDbContext context, IMapper mapper, ILogger<ConcertBandService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<bool> CreateConcertBand(ConcertBandCUDto concertBandDto)
        {
            var concertBand = _mapper.Map<ConcertBand>(concertBandDto);
            _context.ConcertBands.Add(concertBand);
            int result = await _context.SaveChangesAsync();

            CacheHelper.RemoveByPattern(_memoryCache, "ConcertBand");


            return result > 0;
        }

        public async Task<bool> DeleteConcertBand(int concertId, int bandId)
        {
            var concertBand = await _context.ConcertBands.FindAsync(concertId, bandId);

            if (concertBand == null)
                return false;

            _context.ConcertBands.Remove(concertBand);

            int result = await _context.SaveChangesAsync();
            CacheHelper.RemoveByPattern(_memoryCache, "ConcertBand");


            return result > 0;
        }

        public async Task<ConcertBand> GetById(int concertId)
        {
            var cacheKey = $"ConcertBand_{concertId}";

            // Attempt to get data from cache
            if (_memoryCache.TryGetValue(cacheKey, out ConcertBand cachedCb))
            {
                _logger.LogInformation("GetById - Returning cached data for ConcertId = {ConcertId}", concertId);
                return cachedCb;
            }

            // Data is not in cache, fetch it from the database
            var concertBand = await _context.ConcertBands.SingleOrDefaultAsync(cb => cb.ConcertId == concertId);

            if (concertBand != null)
            {
                // Add the result to cache with a specific cache duration
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                // Store the fetched data in cache
                _memoryCache.Set(cacheKey, concertBand, cacheEntryOptions);

                _logger.LogInformation("GetById - Added data to cache for ConcertId = {ConcertId}", concertId);

                return concertBand;
            }

            return null;
        }


        public async Task<List<ConcertBand>> GetList()
        {
            var cacheKey = "ConcertBandsList";

            if (_memoryCache.TryGetValue(cacheKey, out List<ConcertBand> cachedConcertBands))
            {
                _logger.LogInformation("GetList - Returning cached data for ConcertBands list");
                return cachedConcertBands;
            }

            var concertBands = await _context.ConcertBands.ToListAsync();

            if (concertBands != null && concertBands.Any())
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _memoryCache.Set(cacheKey, concertBands, cacheEntryOptions);

                _logger.LogInformation("GetList - Added data to cache for ConcertBands list");
            }

            return concertBands;
        }


        public async Task<bool> UpdateConcertBand(int concertId, int bandId, ConcertBandCUDto concertBandCUDto)
        {
            // Find the existing concertBand entity
            var existingConcertBand = await _context.ConcertBands
                .SingleOrDefaultAsync(cb => cb.ConcertId == concertId && cb.BandId == bandId);

            if (existingConcertBand == null)
            {
                _logger.LogInformation("No existing concert band.");
                return false;
            }

            // Remove the existing concertBand entity from the context
            _context.ConcertBands.Remove(existingConcertBand);

            // Create a new concertBand entity with updated key values
            var newConcertBand = new ConcertBand
            {
                ConcertId = concertBandCUDto.ConcertId, // Assign the new ConcertId
                BandId = concertBandCUDto.BandId,       // Assign the new BandId
                                    
            };

            // Add the modified entity back to the context
            _context.ConcertBands.Add(newConcertBand);

            // Mark the entity as added (not modified)
            _context.Entry(newConcertBand).State = EntityState.Added;

            // Save changes to the database
            int result = await _context.SaveChangesAsync();

            // Remove cache
            CacheHelper.RemoveByPattern(_memoryCache, "ConcertBand");

            _logger.LogInformation("Result = {Result}", result);
            return result > 0;
        }



    }
}
