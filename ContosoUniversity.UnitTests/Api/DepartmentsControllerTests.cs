﻿using Xunit;
using ContosoUniversity.Api.Controllers;
using Moq;
using ContosoUniversity.Data.Interfaces;
using ContosoUniversity.Data.Entities;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.UnitTests.Api
{
    public class DepartmentsControllerTests
    {
        private readonly DepartmentsController _sut;
        private readonly Mock<IRepository<Department>> _mockDepartmentRepo;

        public DepartmentsControllerTests()
        {
            _mockDepartmentRepo = Departments.AsMockRepository<Department>();
            _sut = new DepartmentsController(_mockDepartmentRepo.Object);
        }

        [Fact]
        public void HttpGet_ReturnsAListOfDepartments()
        {
            var result = _sut.GetAll();

            Assert.IsType(typeof(List<Department>), result);
            Assert.Equal(4, ((List<Department>)result).Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void HttpGet_ReturnsANotFoundResult(int id)
        {
            var result = _sut.GetById(id);

            Assert.Equal(404, ((NotFoundResult)result).StatusCode);
        }

        [Theory]
        [InlineData(1, "English", 350000)]
        public void HttpGet_ReturnsADepartmentEntity(int id, string departmentName, int budget)
        {
            var result = _sut.GetById(id);

            Assert.IsType(typeof(ObjectResult), result);
            var department = (Department)((ObjectResult)result).Value;
            Assert.Equal(departmentName, department.Name);
            Assert.Equal(budget, department.Budget);
        }

        private List<Department> Departments { get; } = new List<Department>
        {
            new Department { ID = 1, Name = "English",     Budget = 350000, AddedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 1 },
            new Department { ID = 2, Name = "Mathematics", Budget = 100000, AddedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 2 },
            new Department { ID = 3, Name = "Engineering", Budget = 350000, AddedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 3 },
            new Department { ID = 4, Name = "Economics",   Budget = 100000, AddedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 4 }
        };
    }
}
