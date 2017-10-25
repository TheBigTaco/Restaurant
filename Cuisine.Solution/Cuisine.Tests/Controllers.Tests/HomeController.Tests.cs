using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Cuisine.Controllers;
using Cuisine.Models;

namespace Cuisine.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
      [TestMethod]
      public void Index_ReturnsCorrectView_True()
      {
        HomeController controller = new HomeController();
        IActionResult indexView = controller.Index();
        ViewResult result = indexView as ViewResult;
        Assert.IsInstanceOfType(result, typeof(ViewResult));
      }
    }
}
