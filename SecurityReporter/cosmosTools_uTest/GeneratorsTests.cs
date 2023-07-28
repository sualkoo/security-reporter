using cosmosTools;

namespace cosmosTools_uTest
{
    public class GeneratorsTests
    {
        private Generators SubjectUnderTest { get; set; }

        [SetUp]
        public void Setup()
        {
            SubjectUnderTest = new Generators();
        }

        [Test]
        public void GenerateRandomEmail_ReturnsValidDomainEmail()
        {
            // Arrange
            string name = "Any string";

            // Act
            string email = SubjectUnderTest.GenerateRandomEmail(name);

            // Assert
            Assert.That(email, Does.EndWith("@siemens-healthineers.com"));
        }

        [Test]
        public void GenerateRandomEmail_ReturnsValidFormat()
        {
            // Arrange
            var name = "John Doe";

            // Act
            var result = SubjectUnderTest.GenerateRandomEmail(name);

            // Assert
            Assert.IsTrue(result.EndsWith("@siemens-healthineers.com"));
        }

        [Test]
        public void GenerateWorkingTeam_ReturnsNonNullList()
        {
            // Act
            var result = SubjectUnderTest.GenerateWorkingTeam();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GenerateRandomString_ReturnsValidLength()
        {
            // Arrange
            int length = 10;

            // Act
            var result = SubjectUnderTest.GenerateRandomString(length);

            // Assert
            Assert.AreEqual(length, result.Length);
        }


        [Test]
        public void GenerateRandomDateData_ReturnsValidDateRange()
        {

            // Act
            var result = SubjectUnderTest.GenerateRandomDateData();

            // Assert
            Assert.IsTrue(result >= DateTime.Now.AddMonths(-12) && result <= DateTime.Now.AddMonths(12).Date);
        }

        [Test]
        public void GenerateProjectName_ReturnsValidName()
        {
            // Act
            var result = SubjectUnderTest.GenerateProjectName();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GenerateRandomEmail_ReturnsNonEmptyString()
        {
            // Arrange
            var name = "John Doe";

            // Act
            var result = SubjectUnderTest.GenerateRandomEmail(name);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [Test]
        public void GenerateRandomString_ReturnsNonEmptyString()
        {
            // Arrange
            int length = 10;

            // Act
            var result = SubjectUnderTest.GenerateRandomString(length);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [Test]
        public void GenerateRandomDateData_ReturnsValidDate()
        {
            // Act
            var result = SubjectUnderTest.GenerateRandomDateData();

            // Assert
            Assert.IsTrue(result.Date >= DateTime.Now.AddMonths(-12).Date && result.Date <= DateTime.Now.AddMonths(12).Date);
        }


        [Test]
        public void GenerateRandomElement_ReturnsValidElementFromArray()
        {
            // Arrange
            int[] array = { 1, 2, 3, 4, 5 };

            // Act
            var result = SubjectUnderTest.GenerateRandomElement(array);

            // Assert
            Assert.IsTrue(array.Contains(result));
        }

        [Test]
        public void GenerateProjectName_ReturnsNonEmptyString()
        {
            // Act
            var result = SubjectUnderTest.GenerateProjectName();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }
        [Test]
        public void GenerateReportStatus_ReturnsNonEmptyString()
        { 
            // Act
            var result = SubjectUnderTest.GenerateReportStatus();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }


        [Test]
        public void GenerateWorkingTeam_ReturnsNonEmptyTeamMembers()
        {
            // Act
            var result = SubjectUnderTest.GenerateWorkingTeam();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        public void GenerateRandomDateData_ReturnsValidDateRangeTest()
        {
            // Act
            DateTime startDate = DateTime.Now.AddMonths(-12);
            DateTime endDate = DateTime.Now.AddMonths(12).Date;
            DateTime result = SubjectUnderTest.GenerateRandomDateData(startDate, endDate);

            // Assert
            Assert.IsTrue(result >= startDate && result <= endDate);
        }

        [Test]
        public void GenerateProjectName_ReturnsValidNameTest()
        {
            // Act
            string result = SubjectUnderTest.GenerateProjectName();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(new List<string> { "eHealth", "Seamless Stroke Companion", "Syngo.share", "Syngo.via", "AI Rad", "Smart Connect", "IMS", "Aria Clinical Pathway", "HET", "Mobius", "Aria OS" }.Contains(result));
        }

        [Test]
        public void GenerateRandomElement_ReturnsValidElement()
        {
            // Arrange
            var generators = new Generators();
            string[] testArray = { "Apple", "Banana", "Orange", "Mango", "Grapes" };

            // Act
            string result = generators.GenerateRandomElement(testArray);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(testArray.Contains(result));
        }
        [Test]
        public void GenerateRandomString_ReturnsStringWithSpecificLength()
        {
            // Arrange
            int length = 15;

            // Act
            string result = SubjectUnderTest.GenerateRandomString(length);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(length, result.Length);
        }

        [Test]
        public void GenerateRandomDateData_WithEndDateBeforeStartDate_ReturnsRandomDateInRange()
        {
            // Arrange
            DateTime startDate = DateTime.Now.AddMonths(12);
            DateTime endDate = DateTime.Now.AddMonths(-12);

            // Act
            DateTime result = SubjectUnderTest.GenerateRandomDateData(startDate, endDate);

            // Assert
            Assert.IsTrue(result >= endDate && result <= startDate);
        }


        [Test]
        public void GenerateProjectName_ReturnsNonEmptyName()
        {
            // Act
            string result = SubjectUnderTest.GenerateProjectName();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [Test]
        public void GenerateReportStatus_ReturnsNonEmptyStatus()
        {
            // Act
            string result = SubjectUnderTest.GenerateReportStatus();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [Test]
        public void GeneratePentest_ReturnsNonEmptyPentest()
        {
            // Act
            string result = SubjectUnderTest.GeneratePentest();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }
    }
}