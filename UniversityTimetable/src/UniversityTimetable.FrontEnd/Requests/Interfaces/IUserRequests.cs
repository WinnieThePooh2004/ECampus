﻿using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.FrontEnd.Requests.Interfaces;

public interface IUserRequests : IBaseRequests<UserDto>
{
    Task<UserDto> GetCurrentUserAsync();
    Task<Dictionary<string, string>> ValidateCreateAsync(UserDto user);
    Task<Dictionary<string, string>> ValidateUpdateAsync(UserDto user);
    Task SaveAuditory(int auditoryId);
    Task RemoveSavedAuditory(int auditoryId);
    Task SaveGroup(int groupId);
    Task RemoveSavedGroup(int groupId);
    Task SaveTeacher(int teacherId);
    Task RemoveSavedTeacher(int teacherId);
}