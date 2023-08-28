using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;
using Snickler.EFCore;

namespace MusicianDatabase.Service
{
    public class BandService : IBandService
    {
        private readonly MusicianDbContext _context;
        private readonly IMapper _mapper;
        public BandService(MusicianDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            return results;
        }

        public async Task<bool> CreateBand(BandCUDto bandDto)
        {
            //var band = new Band
            //{
            //    Name = bandDto.Name,
            //    Genre = bandDto.Genre,
            //    Description = bandDto.Description
            //};
            var band = _mapper.Map<Band>(bandDto);
            _context.Bands.Add(band);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteBand(int id)
        {
            var band = await _context.Bands.FindAsync(id);

            if (band == null)
                return false;

            _context.Bands.Remove(band);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<List<BandAvailableDto>> GetAvailable(string? genre, DateTime date)
        {
            // önceki tarihlere istek atılmasını engellemek istersek burada yaparız.

            //var bands = await _context.Bands.Include(b => b.ConcertBands).ThenInclude(cb => cb.Concert)
            //    .Where(b => b.Genre.Contains(genre) && !b.ConcertBands.Any(cb => cb.Concert.Date == date))
            //    .Select(b => new BandAvailableDto
            //    {
            //        Id = b.Id,
            //        Name = b.Name,
            //        Genre = b.Genre,
            //        Description = b.Description
            //    })
            //    .ToListAsync();
            var bands = await _context.Bands.Include(b => b.ConcertBands).ThenInclude(cb => cb.Concert)
                .Where(b =>
                (!string.IsNullOrEmpty(genre) ? b.Genre.Contains(genre) : true) &&
                !b.ConcertBands.Any(cb => cb.Concert.Date == date))
                .ToListAsync();
            var availableBands = _mapper.Map<List<BandAvailableDto>>(bands); // Added reverse map at profile for this to work.

            return availableBands;
        }

        public async Task<List<BandConcertsDto>> GetBandConcertsBetweenDates(int bandId, DateTime startDate, DateTime endDate)
        {
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

            return result;
        }

        // TODO: Use DTOs
        public async Task<Band> GetById(int id)
        {
            var band = await _context.Bands.SingleOrDefaultAsync(a => a.Id == id);

            return band;
        }

        public async Task<List<Band>> GetList()
        {
            var bands = await _context.Bands.ToListAsync();

            return bands;
        }

        public async Task<List<ArtistRoleDto>> GetMembers(int bandId)
        {
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

            return bandMembers;
        }

        public async Task<List<RoleInBandDto>> GetRoles(int bandId)
        {
            var roleInstruments = await _context.RoleInstruments
                .Where(ri => ri.Role.Band.Id == bandId)
                .Include(ri => ri.Instrument)
                .Include(ri => ri.Role).ThenInclude(r => r.Artist)
                .Include(ri => ri.Role).ThenInclude(r => r.Band).ToListAsync();
            var roles = _mapper.Map<List<RoleInBandDto>>(roleInstruments);

            return roles;
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
                //foreach (var roleInstrument in role.RoleInstruments.ToList())
                //{
                //    //_context.RoleInstruments.Remove(roleInstrument);
                //}

                //_context.Roles.Remove(role);
                _context.RoleInstruments.RemoveRange(role.RoleInstruments);
            }
            _context.Roles.RemoveRange(rolesToRemove);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }



        public async Task<bool> UpdateBand(int id, BandCUDto bandUpdateDto)
        {
            var band = await _context.Bands.SingleOrDefaultAsync(a => a.Id == id);

            if (band == null)
                return false;

            //// Update properties from the DTO
            //band.Name = bandUpdateDto.Name;
            //band.Genre = bandUpdateDto.Genre;
            //band.Description = bandUpdateDto.Description;

            _mapper.Map(bandUpdateDto, band);

            _context.Entry(band).State = EntityState.Modified;

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }


    }
}
