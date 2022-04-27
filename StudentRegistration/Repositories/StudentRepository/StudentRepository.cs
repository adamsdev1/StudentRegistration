using Microsoft.EntityFrameworkCore;
using StudentRegistration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.Repositories.StudentRepository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private StudentRegistrationContext context
        {
            get
            {
                return _dbContext as StudentRegistrationContext;
            }
        }
        
        public StudentRepository(DbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

    }
}
