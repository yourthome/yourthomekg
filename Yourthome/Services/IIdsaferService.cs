using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yourthome.Data;
using Yourthome.Models;

namespace Yourthome.Services
{
    public interface IIdsaferService
    {
        void CreateIdSafer();
        void TakeUserID(int userid);
        int GetUserID();
    }

    public class IdsaferService : IIdsaferService
    {
        private YourthomeContext _context;
        public IdsaferService(YourthomeContext context)
        {
            _context = context;
            CreateIdSafer();
        }
        public void CreateIdSafer()
        {
            var safer = _context.Idsafer.SingleOrDefault(s => s.ID == 1);
            if (safer == null)
            {
                Idsafer safer1 = new Idsafer();
                _context.Idsafer.Add(safer1);
                _context.SaveChanges();
            }
        }
        public void TakeUserID(int userid)
        {
            var safer = _context.Idsafer.SingleOrDefault(s => s.ID == 1);
            safer.SafedID = userid;
            _context.Idsafer.Update(safer);
            _context.SaveChanges();
        }
        public int GetUserID()
        {
            var safer = _context.Idsafer.SingleOrDefault(s => s.ID == 1);
            return safer.SafedID;
        }
    }
}
