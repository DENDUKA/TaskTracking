using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TaskTrackingSystem.Tests.TestDAL
{
    [TestClass]
    public class ProjectDALTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            /*IProjectDAL projectDAL = new ProjectDAL();

            var now = DateTime.Now;

            var projectID = projectDAL.Create("TestProject", now, now, "DENDUKA", 1, Shared.Entities.Status.PostpondedId, "TestCustomer", "TestContractNumber");

            Assert.AreEqual(projectID != 0, true, "Проект не был создан");

            var project = projectDAL.Get(projectID);

            Assert.AreEqual(project.Name, "TestProject", "Не правильное значение");
            Assert.AreEqual(project.DateStart, now.Date, "Не правильное значение");
            Assert.AreEqual(project.DateEnd, now.Date, "Не правильное значение");
            Assert.AreEqual(project.Account.Login, "DENDUKA", "Не правильное значение");
            Assert.AreEqual(project.Status.Id, Shared.Entities.Status.PostpondedId, "Не правильное значение");
            Assert.AreEqual(project.Customer, "TestCustomer", "Не правильное значение");
            Assert.AreEqual(project.ContractNumber, "TestContractNumber", "Не правильное значение");

            now = now.AddDays(2);

            projectDAL.Update(projectID, "TestProject2", "DENDUKA", now, now, Shared.Entities.Status.InWorkId, "TestCustomer2", "TestContractNumber2");

            project = projectDAL.Get(projectID);

            Assert.AreEqual(project.Name, "TestProject2", "Не правильное значение");
            Assert.AreEqual(project.DateStart, now.Date, "Не правильное значение");
            Assert.AreEqual(project.DateEnd, now.Date, "Не правильное значение");
            Assert.AreEqual(project.Account.Login, "DENDUKA", "Не правильное значение");
            Assert.AreEqual(project.Status.Id, Shared.Entities.Status.InWorkId, "Не правильное значение");
            Assert.AreEqual(project.Customer, "TestCustomer2", "Не правильное значение");
            Assert.AreEqual(project.ContractNumber, "TestContractNumber2", "Не правильное значение");


            projectDAL.UpdateStatus(projectID, Shared.Entities.Status.DeletedId);
            project = projectDAL.Get(projectID);
            Assert.AreEqual(project.Status.Id, Shared.Entities.Status.DeletedId, "Не поменял статус");

            var result = projectDAL.Delete(projectID);
            Assert.AreEqual(result.Success, true, "Проект не был удален");*/
        }
    }
}
