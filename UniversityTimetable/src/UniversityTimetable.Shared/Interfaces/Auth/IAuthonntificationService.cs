﻿namespace UniversityTimetable.Shared.Interfaces.Auth;

public interface IAuthenticationService
{
    void VerifyUser(int userId);
}