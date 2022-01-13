using Tr3Line.Assessment.Api.Entities;
using Tr3Line.Assessment.Api.Repo;
using Tr3Line.Assessment.Api.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tr3Line.Assessment.Helpers;

namespace Tr3Line.Assessment.Api.Repository
{
    public class CompareResultRepository : DataRepository<CompareResult>, ICompareResultRepository
    {
        public CompareResultRepository(DataContext context) : base(context)
        {

        }
        public async Task<List<CompareResult>> GetCompareResultsByStudenetName(string student)
        {
            try
            {
                var response = await _context.CompareResults.Where(x => x.StudentOne == student || x.StudentTwo == student).ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong " + ex.Message);
            }

        }
        public async Task<List<CompareResult>> GetCompareResultsForFirstStudenet(string student)
        {
            try
            {
                var response = await _context.CompareResults.Where(x => x.StudentOne == student).ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong " + ex.Message);
            }

        }
        public async Task<List<CompareResult>> GetCompareResultsForSecondStudenet(string student)
        {
            try
            {
                var response = await _context.CompareResults.Where(x => x.StudentTwo == student).ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong " + ex.Message);
            }

        }
    }
}
