﻿using ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleGroupSelectorTests
{
    private readonly MultipleGroupSelector _sut;
    private readonly DbSet<Group> _dataSet;
    private readonly List<Group> _data;

    public MultipleGroupSelectorTests()
    {
        _sut = new MultipleGroupSelector();
        _data = new List<Group>
        {
            new() { Name = "b", DepartmentId = 10 },
            new() { Name = "ab", DepartmentId = 10 },
            new() { Name = "ab", DepartmentId = 11 }
        };
        _dataSet = new DbSetMock<Group>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new GroupParameters { DepartmentId = 10, Name = "a" };

        var selectedData = _sut.SelectData(_dataSet, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Count.Should().Be(1);
    }
    
    [Fact]
    public void SelectData_ShouldNotFilterByFacultyId_WhenFacultyIdIs0()
    {
        var parameters = new GroupParameters { DepartmentId = 0, Name = "a" };

        var selectedData = _sut.SelectData(_dataSet, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[2]);
        selectedData.Count.Should().Be(2);
    }
}