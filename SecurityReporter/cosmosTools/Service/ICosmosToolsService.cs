using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmosTools.Service
{
    public interface ICosmosToolsService
    {
        Task<bool> AddProjects(int amount);
        Task<bool> DeleteAllProjects();
    }
}
