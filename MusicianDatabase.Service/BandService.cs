using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Extensions;
using MusicianDatabase.Service.Helpers;
using MusicianDatabase.Service.Interfaces;
using Snickler.EFCore;

namespace MusicianDatabase.Service
{
    public class BandService : IBandService
    {
        private readonly MusicianDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BandService> _logger;
        private readonly IMemoryCache _memoryCache;
        public BandService(MusicianDbContext context, IMapper mapper, ILogger<BandService> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<bool> AddMember(int artistId, int bandId, int instrumentId, string? roleDescription = null)
        {
            var artist = await _context.Artists.FindAsync(artistId);
            var band = await _context.Artists.FindAsync(bandId);
            var instrument = await _context.Instruments.FindAsync(instrumentId);

            // TODO: Come back after Role mapper definition.
            var newRole = new Role
            {
                ArtistId = artistId,
                BandId = bandId,
                Description = roleDescription,
            };
            _context.Roles.Add(newRole);
            int result1 = await _context.SaveChangesAsync();

            var newRoleInstrument = new RoleInstruments
            {
                RoleId = newRole.Id,
                InstrumentId = instrumentId
            };
            _context.RoleInstruments.Add(newRoleInstrument);
            int result2 = await _context.SaveChangesAsync();
            bool results = result1 > 0 && result2 > 0;

            CacheHelper.RemoveByPattern(_memoryCache, "Band");
            CacheHelper.RemoveByPattern(_memoryCache, "Role");



            return results;
        }

        public async Task<bool> CreateBand(BandCUDto bandDto)
        {
            bandDto.Name = bandDto.Name.StylizeBandName();
            var band = _mapper.Map<Band>(bandDto);
            _context.Bands.Add(band);
            int result = await _context.SaveChangesAsync();

            CacheHelper.RemoveByPattern(_memoryCache, "Band");

            return result > 0;
        }

        public async Task<bool> DeleteBand(int id)
        {
            var band = await _context.Bands.FindAsync(id);

            if (band == null)
                return false;

            _context.Bands.Remove(band);

            int result = await _context.SaveChangesAsync();

            CacheHelper.RemoveByPattern(_memoryCache, "Band");
            CacheHelper.RemoveByPattern(_memoryCache, "Role");


            return result > 0;
        }

        public async Task<List<BandAvailableDto>> GetAvailable(string? genre, DateTime date)
        {
            var cacheKey = $"BandsAvailable_{genre}_{date}";

            if (_memoryCache.TryGetValue(cacheKey, out List<BandAvailableDto> cachedAvailableBands))
            {
                _logger.LogInformation($"GetAvailable - Returning cached data for Genre = '{genre}' and Date = '{date}'");
                return cachedAvailableBands;
            }

            var bands = await _context.Bands.Include(b => b.ConcertBands).ThenInclude(cb => cb.Concert)
                .Where(b =>
                (!string.IsNullOrEmpty(genre) ? b.Genre.Contains(genre) : true) &&
                !b.ConcertBands.Any(cb => cb.Concert.Date == date))
                .ToListAsync();

            var availableBands = _mapper.Map<List<BandAvailableDto>>(bands); // Added reverse map at profile for this to work.

            if (availableBands.Count > 0)
            {
                // Add the available bands to cache with a specific cache duration
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _memoryCache.Set(cacheKey, availableBands, cacheEntryOptions);

                return availableBands;
            }

            return null;
        }


        public async Task<List<BandConcertsDto>> GetBandConcertsBetweenDates(int bandId, DateTime startDate, DateTime endDate)
        {
            var cacheKey = $"BandConcerts_{bandId}_{startDate}_{endDate}";

            if (_memoryCache.TryGetValue(cacheKey, out List<BandConcertsDto> cachedBandConcerts))
            {
                _logger.LogInformation($"GetBandConcertsBetweenDates - Returning cached data for BandId = '{bandId}', StartDate = '{startDate}', EndDate = '{endDate}'");
                return cachedBandConcerts;
            }

            var result = new List<BandConcertsDto>();

            await Task.Run(() =>
            {
                _context.LoadStoredProc("GetBandConcertsBetweenDates", false)
                    .WithSqlParam("BandId", bandId)
                    .WithSqlParam("StartDate", startDate)
                    .WithSqlParam("EndDate", endDate)
                    .ExecuteStoredProc((handler) =>
                    {
                        result = handler.ReadToList<BandConcertsDto>().ToList();
                    });
            });

            if (result.Count > 0)
            {
                // Add the result to cache with a specific cache duration
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _memoryCache.Set(cacheKey, result, cacheEntryOptions);

                return result;
            }

            return null;
        }

        public async Task<Band> GetById(int id)
        {
            var cacheKey = $"Band_{id}";

            if (_memoryCache.TryGetValue(cacheKey, out Band cachedBand))
            {
                _logger.LogInformation($"GetById (Band) - Returning cached data for Id = {id}");
                return cachedBand;
            }

            var band = await _context.Bands.SingleOrDefaultAsync(a => a.Id == id);

            if (band != null)
            {
                // Add the band to cache with a specific cache duration
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _memoryCache.Set(cacheKey, band, cacheEntryOptions);

                return band;
            }

            return null;
        }

        public async Task<List<Band>> GetList()
        {
            var cacheKey = "Bands";

            if (_memoryCache.TryGetValue(cacheKey, out List<Band> cachedBands))
            {
                _logger.LogInformation("GetList (Bands) - Returning cached data");
                return cachedBands;
            }

            var bands = await _context.Bands.ToListAsync();

            if (bands.Count > 0)
            {
                // Add the bands list to cache with a specific cache duration
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _memoryCache.Set(cacheKey, bands, cacheEntryOptions);

                return bands;
            }

            return null;
        }


        public async Task<List<ArtistRoleDto>> GetMembers(int bandId)
        {
            var cacheKey = $"BandMembers_{bandId}";

            if (_memoryCache.TryGetValue(cacheKey, out List<ArtistRoleDto> cachedBandMembers))
            {
                _logger.LogInformation($"GetMembers - Returning cached data for BandId = '{bandId}'");
                return cachedBandMembers;
            }

            var bandMembers = await _context.Bands
                .Where(b => b.Id == bandId)
                .SelectMany(b => b.Roles.Select(r => new
                {
                    BandId = b.Id,
                    BandName = b.Name,
                    ArtistId = r.Artist.Id,
                    FirstName = r.Artist.FirstName,
                    LastName = r.Artist.LastName,
                    ArtistGenre = r.Artist.Genre, // Added property
                    Instrument = r.RoleInstruments.Single().Instrument.Name
                }))
                .GroupBy(a => new { a.ArtistId, a.FirstName, a.LastName, a.ArtistGenre })
                .Select(g => new ArtistRoleDto
                {
                    BandId = g.First().BandId,
                    BandName = g.First().BandName,
                    ArtistId = g.Key.ArtistId,
                    FirstName = g.Key.FirstName,
                    LastName = g.Key.LastName,
                    ArtistGenre = g.Key.ArtistGenre,
                    Instrument = string.Join(", ", g.Select(a => a.Instrument))
                })
                .ToListAsync();

            if (bandMembers.Count > 0)
            {
                // Add the bandMembers to cache with a specific cache duration
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _memoryCache.Set(cacheKey, bandMembers, cacheEntryOptions);

                return bandMembers;
            }

            return null;
        }

        public async Task<List<RoleInBandDto>> GetRoles(int bandId)
        {
            var cacheKey = $"BandRoles_{bandId}";

            if (_memoryCache.TryGetValue(cacheKey, out List<RoleInBandDto> cachedBandRoles))
            {
                _logger.LogInformation($"GetRoles - Returning cached data for BandId = '{bandId}'");
                return cachedBandRoles;
            }

            var roleInstruments = await _context.RoleInstruments
                .Where(ri => ri.Role.Band.Id == bandId)
                .Include(ri => ri.Instrument)
                .Include(ri => ri.Role).ThenInclude(r => r.Artist)
                .Include(ri => ri.Role).ThenInclude(r => r.Band)
                .ToListAsync();
            var roles = _mapper.Map<List<RoleInBandDto>>(roleInstruments);

            if (roles.Count > 0)
            {
                // Add the roles to cache with a specific cache duration
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                _memoryCache.Set(cacheKey, roles, cacheEntryOptions);

                return roles;
            }

            return null;
        }


        public async Task<bool> RemoveMember(int artistId, int bandId)
        {
            var rolesToRemove = await _context.Roles
                .Include(r => r.RoleInstruments)
                .Where(r => r.ArtistId == artistId && r.BandId == bandId)
                .ToListAsync();

            if (!rolesToRemove.Any())
                return false; // Member not found in the specified band

            foreach (var role in rolesToRemove)
            {
                _context.RoleInstruments.RemoveRange(role.RoleInstruments);
            }
            _context.Roles.RemoveRange(rolesToRemove);

            int result = await _context.SaveChangesAsync();
            CacheHelper.RemoveByPattern(_memoryCache, "Band");
            CacheHelper.RemoveByPattern(_memoryCache, "Role");

            return result > 0;
        }

        public async Task<bool> UpdateBand(int id, BandCUDto bandUpdateDto)
        {
            var band = await _context.Bands.SingleOrDefaultAsync(a => a.Id == id);

            if (band == null)
                return false;

            bandUpdateDto.Name = bandUpdateDto.Name.StylizeBandName();

            _mapper.Map(bandUpdateDto, band);

            _context.Entry(band).State = EntityState.Modified;

            int result = await _context.SaveChangesAsync();
            CacheHelper.RemoveByPattern(_memoryCache, "Band");

            return result > 0;
        }

    }
}
