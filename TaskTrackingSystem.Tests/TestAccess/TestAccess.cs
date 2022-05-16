using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TaskTrackingSystem.Tests.TestAccess
{
    [TestClass]
    public class TestAccess
    {
        /*
        [TestMethod]
        public void TestAccessProjectAdmin()
        {
            HttpContext.Current = FakeMock.FakeHttpContext();
            FakeMock.LoginUser("TestAdmin", "admin");

            var projectType = 1;

            var project = new Project(0, "TestProject", new AccountBase("", "TestAdmin"), new ProjectType(projectType, ""), new Status(1, ""), DateTime.Now, DateTime.Now, "", "", 0);

            project.Id = Logic.Logic.Instance.Project.Create(project);

            var access = Logic.Access.AccessBLL.Instance.ProjectAccess(project.ProjectType.Id, project.Id);

            Assert.AreEqual(access.CostField && access.CreateNew && access.Delete && access.Edit, true);


            Assert.AreEqual(Logic.Logic.Instance.Project.FullDelete(project.Id).Success, true);
        }

        [TestMethod]
        public void TestAccessProjectBuh()
        {
            HttpContext.Current = FakeMock.FakeHttpContext();

            var projectType = 1;

            var Login = "TestBuh";
            var Role = "buh";

            FakeMock.LoginUser(Login, Role);            

            var project = new Project(0, "TestProject", new AccountBase("", Login), new ProjectType(projectType, ""), new Status(1, ""), DateTime.Now, DateTime.Now, "", "", 0);

            project.Id = Logic.Logic.Instance.Project.Create(project);

            var access = Logic.Access.AccessBLL.Instance.ProjectAccess(project.ProjectType.Id, project.Id);

            Assert.AreEqual(access.CostField && access.CreateNew && access.Delete && access.Edit, true);

            Assert.AreEqual(Logic.Logic.Instance.Project.FullDelete(project.Id).Success, true);
        }
        */
    }
}