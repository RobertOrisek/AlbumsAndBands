using ZavrsniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZavrsniTest.Repository.Interfaces
{
    public interface IAlbumRepository
    {
        IQueryable<Album> GetAll();
        Album GetById(int id);
        void Add(Album seller);
        void Update(Album seller);
        void Delete(Album seller);
        IQueryable<Album> GetAllByParameters(int minimum, int maximum);
    }
}
