using SchoolProject.Data;
using SchoolProject.Enums;
using SchoolProject.Models;
using SchoolProject.Services;

namespace Project.Tests.Services;

public class EmployeeTests
{
    private readonly List<EmployeeDTO> _employees = SeedData.GetEmployees();
    private readonly EmployeeService _service = new();

    [Fact]
    public void BirthDate_FuturDate_ThrowsArgumentException()
    {
        var future = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        Assert.Throws<ArgumentException>(() => new BirthDate(future));
    }

    [Fact]
    public void BirthDate_Age_IsCorrect()
    {
        var date = DateOnly.FromDateTime(DateTime.Today.AddYears(-30));
        var bd = new BirthDate(date);
        Assert.Equal(30, bd.Age);
    }

    [Fact]
    public void EmployeeDTO_SameData_AreEqual()
    {
        var a = new EmployeeDTO("Anna", "Jensen", new BirthDate(new DateOnly(1990, 3, 15)), Gender.F);
        var b = new EmployeeDTO("Anna", "Jensen", new BirthDate(new DateOnly(1990, 3, 15)), Gender.F);
        Assert.True(a.Equals(b));
    }

    [Fact]
    public void EmployeeDTO_DifferentName_AreNotEqual()
    {
        var a = new EmployeeDTO("Anna", "Jensen", new BirthDate(new DateOnly(1990, 3, 15)), Gender.F);
        var b = new EmployeeDTO("Maria", "Jensen", new BirthDate(new DateOnly(1990, 3, 15)), Gender.F);
        Assert.False(a.Equals(b));
    }

    [Fact]
    public void EmployeeDTO_Sort_OrdersByLastName()
    {
        var list = new List<EmployeeDTO>
        {
            new("B", "Ø", new BirthDate(new DateOnly(1980, 1, 1)), Gender.M),
            new("A", "A", new BirthDate(new DateOnly(1980, 1, 1)), Gender.F),
        };
        list.Sort();
        Assert.Equal("A", list[0].EfterNavn);
    }

 
    [Fact]
    public void EmployeeRecord_SameData_AreEqual()
    {
        var r1 = new EmployeeRecord("Anna", "Jensen", 34, Gender.F);
        var r2 = new EmployeeRecord("Anna", "Jensen", 34, Gender.F);
        Assert.Equal(r1, r2); 
    }

    [Fact]
    public void EmployeeRecord_Sort_OrdersByLastName()
    {
        var list = new List<EmployeeRecord>
        {
            new("B", "Sørensen", 40, Gender.M),
            new("A", "Andersen", 35, Gender.F),
        };
        list.Sort();
        Assert.Equal("Andersen", list[0].EfterNavn);
    }

    [Fact]
    public void GetByGender_Female_ReturnsOnlyFemales()
    {
        var result = _service.GetByGender(_employees, Gender.F);
        Assert.All(result, r => Assert.Equal(Gender.F, r.Køn));
    }

    [Fact]
    public void GetByGender_Male_ReturnsOnlyMales()
    {
        var result = _service.GetByGender(_employees, Gender.M);
        Assert.All(result, r => Assert.Equal(Gender.M, r.Køn));
    }

    [Fact]
    public void GetByGender_SeedData_Has6FemalesAnd4Males()
    {
        Assert.Equal(6, _service.GetByGender(_employees, Gender.F).Count);
        Assert.Equal(4, _service.GetByGender(_employees, Gender.M).Count);
    }

    [Fact]
    public void GetNearingRetirement_SeedData_AtLeastTwo()
    {
        var result = _service.GetNearingRetirement(_employees);
        Assert.True(result.Count >= 2);
    }

    [Fact]
    public void GetNearingRetirement_AllWithin5Years()
    {
        var result = _service.GetNearingRetirement(_employees);
        Assert.All(result, r => Assert.True(67 - r.Alder <= 5));
    }
}
