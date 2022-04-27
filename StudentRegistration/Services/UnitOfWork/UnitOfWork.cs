using Microsoft.EntityFrameworkCore;
using StudentRegistration.Models;
using StudentRegistration.Repositories.StudentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;

        public UnitOfWork()
        {
            _dbContext = new StudentRegistrationContext();
        }

        private IStudentRepository _StudentRepository;

        public IStudentRepository StudentRepository
        {
            get
            {
                if (_StudentRepository == null)
                    _StudentRepository = new StudentRepository(_dbContext);

                return _StudentRepository;
            }
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
