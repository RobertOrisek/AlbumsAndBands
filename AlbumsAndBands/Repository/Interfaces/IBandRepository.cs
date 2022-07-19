using ZavrsniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZavrsniTest.Repository.Interfaces
{
    public interface IBandRepository
    {
        IQueryable<Band> GetAll();
        Band GetById(int id);
        void Add(Band shop);
        void Update(Band shop);
        void Delete(Band shop);
    }
}
