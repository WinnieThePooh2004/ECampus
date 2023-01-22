﻿using ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleTeacherSelectorTests
{
    private readonly MultipleTeacherSelector _sut;
    private readonly DbSet<Teacher> _dataSet;
    private readonly List<Teacher> _data;

    public MultipleTeacherSelectorTests()
    {
        _sut = new MultipleTeacherSelector();
        _data = new List<Teacher>
        {
            new() { LastName = "b", DepartmentId = 10 },
            new() { LastName = "ab", DepartmentId = 10 },
            new() { LastName = "ab", DepartmentId = 11 }
        };
        _dataSet = new DbSetMock<Teacher>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new TeacherParameters { DepartmentId = 10, LastName = "a", FirstName = ""};

        var selectedData = _sut.SelectData(_dataSet, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Count.Should().Be(1);
    }
    
    [Fact]
    public void SelectData_ShouldNotFilterByFacultyId_WhenFacultyIdIs0()
    {
        var parameters = new TeacherParameters { DepartmentId = 0, LastName = "a", FirstName = "" };

        var selectedData = _sut.SelectData(_dataSet, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[2]);
        selectedData.Count.Should().Be(2);
    }
}