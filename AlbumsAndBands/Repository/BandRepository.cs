using Microsoft.EntityFrameworkCore;
using ZavrsniTest.Repository.Interfaces;
using ZavrsniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZavrsniTest.Repository
{
    public class BandRepository: IBandRepository
    {
        private readonly AppDbContext _context;

        public BandRepository(AppDbContext context)
        {
            this._context = context;
        }

        public IQueryable<Band> GetAll()
        {
            return _context.Bands;
        }

        public Band GetById(int id)
        {
            return _context.Bands.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Band band)
        {
            _context.Bands.Add(band);
            _context.SaveChanges();
        }

        public void Update(Band band)
        {
            _context.Entry(band).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public void Delete(Band band)
        {
            _context.Bands.Remove(band);
            _context.SaveChanges();
        }
    }
}
