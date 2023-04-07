using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.UnitTests.Api;
public class UnitTestBase {
    protected Mock<IMapper> _mapper;

    public UnitTestBase() {
        _mapper = new Mock<IMapper>();
    }

    public Mock<IMapper> GetMapper() => _mapper;

    public void SetupMapper<T, S>(T target, S source) {
        _mapper.Setup(m => m.Map<T>(source)).Returns(target);
    }

    public void AssertHttpStatusisValid<T>(IActionResult result) {
        Assert.IsInstanceOfType(result, typeof(T));
        Assert.IsNotNull((T)result);
    }
    

}
