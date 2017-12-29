using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Licenta.ORM;

namespace Licenta.Tests
{
    [TestClass]
    public class SqlStatementPreparationTests
    {
        [TestMethod]
        public void CanCreateSelectStatement()
        {
            var selectStatement = Sql.GetSelectStatement("table1", new List<string> { "column1", "column2", "column3" });
            Assert.AreEqual("SELECT [column1],[column2],[column3] FROM [dbo].[table1]", selectStatement);
        }

        [TestMethod]
        public void CanCreateInsertStatement()
        {
            var insertStatement = Sql.GetInsertStatement("table1", new List<string> { "column1", "column2", "column3" });
            Assert.AreEqual("INSERT INTO [dbo].[table1] ([column1],[column2],[column3]) VALUES (@column1,@column2,@column3)", insertStatement);
        }

        [TestMethod]
        public void CanCreateDeleteStatement()
        {
            var deleteStatement = Sql.GetDeleteStatement("table1", new List<string> { "column1", "column2", "column3" });
            Assert.AreEqual("DELETE FROM [dbo].[table1] WHERE [column1] = @column1 AND [column2] = @column2 AND [column3] = @column3", deleteStatement);
        }
    }
}
