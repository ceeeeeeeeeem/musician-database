﻿using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
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
        private readonly List<string> _concertCountsCacheKeys = new List<string>();

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

            _memoryCache.Remove("ConcertCounts");
            _memoryCache.Remove("ConcertsCache");


            return result > 0;
        }

        public async Task<bool> DeleteConcert(int id)
        {
            var concert = await _context.Concerts.FindAsync(id);

            if (concert == null)
                return false;

            _context.Concerts.Remove(concert);

            int result = await _context.SaveChangesAsync();
            _memoryCache.Remove("ConcertCounts");
            _memoryCache.Remove("ConcertsCache");

            return result > 0;
        }

        public async Task<Concert> GetById(int id)
        {
            var concert = await _context.Concerts.SingleOrDefaultAsync(a => a.Id == id);

            return concert;
        }

        public async Task<List<ConcertCountDto>> GetConcertCountsBetweenDates(DateTime startDate, DateTime endDate)
        {
            var cacheKey = $"ConcertCounts";

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
            var concerts = await _context.Concerts.ToListAsync();

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

            concertNew.ConcertBands.Add(new ConcertBand
            {
                ConcertId = concertNew.Id,
                BandId = concertQCDto.BandId,
            });

            _context.Concerts.Add(concertNew);

            int result3 = await _context.SaveChangesAsync();
            _memoryCache.Remove("ConcertCounts");
            _memoryCache.Remove("ConcertsCache");

            return result3 > 0;
        }

        public async Task<bool> UpdateConcert(int id, ConcertCUDto concertUpdateDto)
        {
            var concert = await _context.Concerts.SingleOrDefaultAsync(a => a.Id == id);

            if (concert == null)
                return false;

            _mapper.Map(concertUpdateDto, concert);
            _context.Entry(concert).State = EntityState.Modified;

            int result = await _context.SaveChangesAsync();
            _memoryCache.Remove("ConcertCounts");
            _memoryCache.Remove("ConcertsCache");

            return result > 0;
        }
    }
}
