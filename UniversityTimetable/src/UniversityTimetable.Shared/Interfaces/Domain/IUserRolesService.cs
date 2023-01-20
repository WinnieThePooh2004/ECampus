﻿using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Shared.Interfaces.Domain;

public interface IUserRolesService
{
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> UpdateAsync(UserDto user);
    Task<UserDto> CreateAsync(UserDto user);
}