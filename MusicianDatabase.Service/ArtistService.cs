using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Helpers;
using MusicianDatabase.Service.Interfaces;
using System;

namespace MusicianDatabase.Service
{
    public class ArtistService : IArtistService
    {
        private readonly MusicianDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ArtistService> _logger;
        private readonly IMemoryCache _memoryCache;
        public ArtistService(MusicianDbContext context, IMapper mapper, ILogger<ArtistService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;

        }

        public async Task<bool> CreateArtist(ArtistCUDto artistDto)
        {
            var artist = _mapper.Map<Artist>(artistDto);

            _context.Artists.Add(artist);
            int result = await _context.SaveChangesAsync();

            CacheHelper.RemoveByPattern(_memoryCache, "Artist");

            return result > 0;
        }

        public async Task<bool> DeleteArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);

            if (artist == null)
                return false;

            _context.Artists.Remove(artist);

            int result = await _context.SaveChangesAsync();

            CacheHelper.RemoveByPattern(_memoryCache, "Artist");

            return result > 0;
        }

        // SQL SP
        public async Task<List<Artist>> GetArtistsWithoutBands()
        {
            var cacheKey = "ArtistsWithoutBands";

            // Attempt to get data from cache
            if (_memoryCache.TryGetValue(cacheKey, out List<Artist> cachedData))
            {
                _logger.LogInformation("GetArtistsWithoutBands - Returning cached data");
                return cachedData;
            }

            // Data is not in cache, fetch it from the database
            var result = await _context.Artists.FromSqlRaw("GetArtistsWithoutBands").ToListAsync();

            if (result != null && result.Any())
            {
                // Add the result to cache with a specific cache duration
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Set an appropriate cache duration
                };

                // Store the fetched data in cache
                _memoryCache.Set(cacheKey, result, cacheEntryOptions);

                return result;
            }

            return null;
        }


        public async Task<Artist> GetById(int id)
        {
            var cacheKey = $"Artist_{id}";

            // Attempt to get data from cache
            if (_memoryCache.TryGetValue(cacheKey, out Artist cachedData))
            {
                _logger.LogInformation("GetById (Artist) - Returning cached data for Id = {Id}", id);
                return cachedData;
            }

            // Data is not in cache, fetch it from the database
            var artist = await _context.Artists.SingleOrDefaultAsync(a => a.Id == id);

            if (artist != null)
            {
                // Add the artist to cache with a specific cache duration
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Set an appropriate cache duration
                };

                // Store the fetched data in cache
                _memoryCache.Set(cacheKey, artist, cacheEntryOptions);

                return artist;
            }

            return null;
        }


        public async Task<List<Artist>> GetList()
        {
            var cacheKey = $"Artists";
            if (_memoryCache.TryGetValue(cacheKey, out List<Artist> cachedData))
                return cachedData;

            var artists = await _context.Artists.ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Set an appropriate cache duration
            };

            _memoryCache.Set(cacheKey, artists, cacheEntryOptions);
            _logger.LogInformation("Added CacheKey Artists");

            return artists;
        }

        public async Task<List<ArtistRoleDto>> GetRolesById(int id)
        {
            var cacheKey = $"ArtistRoles_{id}";

            if (_memoryCache.TryGetValue(cacheKey, out List<ArtistRoleDto> cachedRoles))
            {
                _logger.LogInformation("GetRolesById - Returning cached data for ArtistId = {ArtistId}", id);
                return cachedRoles;
            }

            var artistRoles = await _context.Artists
                .Where(a => a.Id == id)
                .SelectMany(a => a.Roles.Select(r => new ArtistRoleDto
                {
                    ArtistId = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    BandId = r.BandId,
                    BandName = r.Band.Name, // Assuming Band has a Name property
                    BandGenre = r.Band.Genre, // Assuming Band has a Genre property
                    Instrument = r.RoleInstruments.Single().Instrument.Name // Get the instrument's name directly
                }))
                .ToListAsync();

            var groupedArtistRoles = artistRoles
                .GroupBy(ar => new { ar.ArtistId, ar.BandId })
                .Select(group => new ArtistRoleDto
                {
                    ArtistId = group.Key.ArtistId,
                    FirstName = group.First().FirstName,
                    LastName = group.First().LastName,
                    BandId = group.Key.BandId,
                    BandName = group.First().BandName,
                    BandGenre = group.First().BandGenre,
                    Instrument = string.Join(", ", group.Select(ar => ar.Instrument)) // Combine instruments
                })
                .ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            _memoryCache.Set(cacheKey, groupedArtistRoles, cacheEntryOptions);
            _logger.LogInformation("GetRolesById - Added data to cache for ArtistId = {ArtistId}", id);

            return groupedArtistRoles;
        }




        public async Task<List<ArtistRoleDto>> GetRolesOfArtistByInstrument(int id, int instrumentId)
        {
            var cacheKey = $"ArtistRoles_Artist_{id}_Instrument_{instrumentId}";

            if (_memoryCache.TryGetValue(cacheKey, out List<ArtistRoleDto> cachedRoles))
            {
                _logger.LogInformation("GetRolesOfArtistByInstrument - Returning cached data for ArtistId = {ArtistId} and InstrumentId = {InstrumentId}", id, instrumentId);
                return cachedRoles;
            }

            var artistRoles = await _context.Artists
                .Where(a => a.Id == id)
                .SelectMany(a => a.Roles)
                .Where(role => role.RoleInstruments.Any(ri => ri.InstrumentId == instrumentId))
                .Select(role => new ArtistRoleDto
                {
                    ArtistId = role.ArtistId, // Use the parameter 'id' passed to the method
                    FirstName = role.Artist.FirstName, // Assuming 'a' is the artist
                    LastName = role.Artist.LastName,
                    BandId = role.BandId, // Use 'role' for BandId
                    BandName = role.Band.Name, // Assuming Band has a Name property
                    BandGenre = role.Band.Genre, // Assuming Band has a Genre property
                    Instrument = role.RoleInstruments.Single(ri => ri.InstrumentId == instrumentId).Instrument.Name // Get the instrument's name directly
                })
                .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            _memoryCache.Set(cacheKey, artistRoles, cacheEntryOptions);
            _logger.LogInformation("GetRolesOfArtistByInstrument - Added data to cache for ArtistId = {ArtistId} and InstrumentId = {InstrumentId}", id, instrumentId);

            return artistRoles;
        }




        public async Task<bool> UpdateArtist(int id, ArtistCUDto artistUpdateDto)
        {
            var artist = await _context.Artists.SingleOrDefaultAsync(a => a.Id == id);

            if (artist == null)
                return false;

            _mapper.Map(artistUpdateDto, artist);

            _context.Entry(artist).State = EntityState.Modified;

            int result = await _context.SaveChangesAsync();

            CacheHelper.RemoveByPattern(_memoryCache, "Artist");

            return result > 0;
        }
    }
}
