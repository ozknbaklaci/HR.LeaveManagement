﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly LeaveManagementDbContext _context;

        public GenericRepository(LeaveManagementDbContext context)
        {
            _context = context;
        }

        public async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await Get(id);

            return entity != null;
        }

        public async Task<T> Add(T entity)
        {
            await _context.AddAsync(entity);
            //TODO : UnitOfWork Tarafında SaveChangesAsync() eklendi.
            //await _context.SaveChangesAsync();

            return entity;
        }

        public async Task Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            //TODO : UnitOfWork Tarafında SaveChangesAsync() eklendi.
            //await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            //TODO : UnitOfWork Tarafında SaveChangesAsync() eklendi.
            // await _context.SaveChangesAsync();
        }
    }
}
