﻿using Microsoft.AspNetCore.Mvc;
using webapi.Login.Models;
using webapi.Models;
using webapi.MyProfile.Models;
using webapi.ProjectSearch.Models;

namespace webapi.Service
{
    public interface ICosmosService
    {
        Task<ProjectData> GetProjectById(string id);
        Task<bool> GetUploadStatus(string id);
        Task<bool> AddProject(ProjectData data);
        Task<bool> UpdateProject(ProjectData data);
        Task<bool> DeleteProject(string projectId);
        Task<List<string>> DeleteProjects(List<string> projectIds);
        Task<CountProjects> GetItems(int pageSize, int pageNumber, FilterData filter, SortData sort);
        //Task<int> GetNumberOfProjects();
        Task<Profile> GetBacklog(int pageSize, int pageNumber);

        Task<bool> AddProjectReport(ProjectReportData data);
        Task<ProjectReportData> GetProjectReport(string projectId);
        Task<PagedDbResults<List<FindingResponse>>> GetPagedProjectReportFindings(string? projectName, string? details, string? impact, string? repeatability, string? references, string? cWE, string? findingName, int page);
        Task<bool> DeleteProjectReports(List<string> projectReportIds);
        Task<List<Tuple<string, int, int>>> GetCriticalityData();
        Task<List<Tuple<string, int, int>>> GetVulnerabilityData();

        Task<List<Tuple<int, int>>> GetCWEData();
        Task<List<string>> DeleteAllReportsAsync();

        //Task<Profile> ProfileItems(string email);

        Task DeleteUsers();
        Task<UserRole> GetUserRole(string email);
        Task<List<Tuple<float, string, string>>> GetCVSSData();
    }
}
