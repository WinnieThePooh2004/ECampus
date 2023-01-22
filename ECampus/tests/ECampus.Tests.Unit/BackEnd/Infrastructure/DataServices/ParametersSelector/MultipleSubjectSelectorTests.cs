﻿using ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleSubjectSelectorTests
{
    private readonly MultipleSubjectSelector _sut;
    private readonly DbSet<Subject> _dataSet;
    private readonly List<Subject> _data;

    public MultipleSubjectSelectorTests()
    {
        _sut = new MultipleSubjectSelector();
        _data = new List<Subject>
        {
            new() { Name = "name1" },
            new() { Name = "name2" },
            new() { Name = "nam" }
        };
        _dataSet = new DbSetMock<Subject>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new SubjectParameters { Name = "name" };

        var selectedData = _sut.SelectData(_dataSet, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[0]);
        selectedData.Count.Should().Be(2);
    }
}