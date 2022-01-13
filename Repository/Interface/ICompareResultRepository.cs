using Tr3Line.Assessment.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tr3Line.Assessment.Api.Repository.Interface
{
    public interface ICompareResultRepository : IDataRepository<CompareResult>
    {
        Task<List<CompareResult>> GetCompareResultsByStudenetName(string student);
        Task<List<CompareResult>> GetCompareResultsForFirstStudenet(string student);
        Task<List<CompareResult>> GetCompareResultsForSecondStudenet(string student);
    }
}
