using ECampus.Shared.Comparing;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.Tests.Unit.Tests.Shared;

public class DataTransferObjectComparerTests
{
    private readonly DataTransferObjectComparer<AuditoryDto> _sut = new();

    [Fact]
    public void GetHashCode_ShouldReturnObjectId()
    {
        var auditory = new AuditoryDto{ Id = 10 };

        _sut.GetHashCode(auditory).Should().Be(auditory.Id);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenBothAreNull()
    {
        _sut.Equals(null, null).Should().BeTrue();
    }
    
    [Fact]
    public void Equals_ShouldReturnFalse_WhenOnlyXIsNullIsNull()
    {
        _sut.Equals(null, new AuditoryDto()).Should().BeFalse();
    }
    
    [Fact]
    public void Equals_ShouldReturnFalse_WhenOnlyYIsNullIsNull()
    {
        _sut.Equals(new AuditoryDto(), null).Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenIdsEqual()
    {
        _sut.Equals(new AuditoryDto { Id = 10 }, new AuditoryDto { Id = 10 }).Should().BeTrue();
    }
    
    [Fact]
    public void Equals_ShouldReturnFalse_WhenIdsNotEqual()
    {
        _sut.Equals(new AuditoryDto { Id = 11 }, new AuditoryDto { Id = 10 }).Should().BeFalse();
    }
}