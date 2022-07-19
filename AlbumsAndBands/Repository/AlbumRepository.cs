using Microsoft.EntityFrameworkCore;
using ZavrsniTest.Repository.Interfaces;
using ZavrsniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZavrsniTest.Repository
{
    public class AlbumRepository: IAlbumRepository
    {
        private readonly AppDbContext _context;

        public AlbumRepository(AppDbContext context)
        {
            this._context = context;
        }

        public IQueryable<Album> GetAll()
        {
            return _context.Albums;
        }

        public Album GetById(int id)
        {
            return _context.Albums.FirstOrDefault(a => a.Id == id);
        }

        public void Add(Album album)
        {
            _context.Albums.Add(album);
            _context.SaveChanges();
        }

        public void Update(Album album)
        {
            _context.Entry(album).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public void Delete(Album album)
        {
            _context.Albums.Remove(album);
            _context.SaveChanges();
        }

        public IQueryable<Album> GetAllByParameters(int minimum, int maximum)
        {
            return _context.Albums.Include(b => b.Band).Where(a => a.PublishingYear >= minimum && a.PublishingYear <= maximum);
        }
    }
}
