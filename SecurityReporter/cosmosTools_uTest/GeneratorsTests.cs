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
            string name = "Any name";

            // Act
            string email = SubjectUnderTest.GenerateRandomEmail(name);

            // Assert
            Assert.That(email, Does.EndWith("@siemens-healthineers.com"));
        }

        [Test]
        public void GenerateRandomEmail_ReturnsValidNameFormat()
        {
            // Arrange
            var name = "John Doe";

            // Act
            var result = SubjectUnderTest.GenerateRandomEmail(name);

            // Assert
            Assert.That(result.Contains("johnDoe"));
        }

        [Test]
        public void GenerateRandomString_ReturnsGivenStringLength()
        {
            // Arrange
            int length = 10;

            // Act
            var result = SubjectUnderTest.GenerateRandomString(length);

            // Assert
            Assert.AreEqual(length, result.Length, "The generated string has the correct length.");
        }


        [Test]
        public void GenerateRandomDateData_ReturnsValidDateRange()
        {
            // Act
            var result = SubjectUnderTest.GenerateRandomDateData();

            // Assert
            Assert.That(result, Is.AtLeast(DateTime.Now.AddMonths(-12)).And.AtMost(DateTime.Now.AddMonths(12)));
        }

        [Test]
        public void GenerateProjectName_ReturnsValidName()
        {
            // Act
            var result = SubjectUnderTest.GenerateProjectName();

            // Assert
            Assert.That(result.Count, Is.AtLeast(3));
        }

        [Test]
        public void GenerateRandomString_ReturnsNonEmptyString()
        {
            // Arrange
            int length = 10;

            // Act
            var result = SubjectUnderTest.GenerateRandomString(length);

            // Assert
            Assert.That(result.Count, Is.AtLeast(length));
        }

        [Test]
        public void GenerateRandomElement_ReturnsValidElementFromArray()
        {
            // Arrange
            int[] array = { 1, 2, 3, 4, 5 };

            // Act
            var result = SubjectUnderTest.GenerateRandomElement(array);

            // Assert
            Assert.Contains(result, array);
        }

        [Test]
        public void GenerateRandomDateData_ReturnsValidDateRangeTest()
        {
            // Act
            DateTime startDate = DateTime.Now.AddMonths(-12);
            DateTime endDate = DateTime.Now.AddMonths(12).Date;
            DateTime result = SubjectUnderTest.GenerateRandomDateData(startDate, endDate);

            // Assert
            Assert.That(result, Is.AtLeast(startDate).And.AtMost(endDate));
        }

        [Test]
        public void GenerateProjectName_ReturnsValidNameFromList()
        {
            // Arrange
            var projectNames = new List<string> { "eHealth", "Seamless Stroke Companion", "Syngo.share", "Syngo.via", "AI Rad", "Smart Connect", "IMS", "Aria Clinical Pathway", "HET", "Mobius", "Aria OS" };

            // Act
            string result = SubjectUnderTest.GenerateProjectName();

            // Assert
            Assert.That(projectNames.Contains(result));
        }

        [Test]
        public void GenerateRandomElement_ReturnsValidElementFromList()
        {
            // Arrange
            string[] testArray = { "Apple", "Banana", "Orange", "Mango", "Grapes" };

            // Act
            string result = SubjectUnderTest.GenerateRandomElement(testArray);

            // Assert
            Assert.That(testArray.Contains(result));
        }

        [Test]
        public void GenerateRandomString_ReturnsStringWithSpecificLength()
        {
            // Arrange
            int length = 15;

            // Act
            string result = SubjectUnderTest.GenerateRandomString(length);

            // Assert
            Assert.AreEqual(length, result.Length);
        }

        [Test]
        public void GenerateRandomDate_StartDateBeforeEndDate_ReturnsRandomDateInRange()
        {
            // Arrange
            DateTime startDate = DateTime.Now.AddMonths(12);
            DateTime endDate = DateTime.Now.AddMonths(-12);

            // Act
            DateTime result = SubjectUnderTest.GenerateRandomDateData(startDate, endDate);

            // Assert
            Assert.That(result, Is.AtLeast(endDate).And.AtMost(startDate));
        }

        [Test]
        public void GenerateReportStatus_ReturnsValidStatusFromList()
        {
            // Arrange
            string[] testArray = {"Pending", "In Progress", "Completed",
                                  "Approved", "Rejected", "On Hold",
                                  "Cancelled", "Draft", "Error", "Awaiting Review" };

            // Act
            string result = Generators.GenerateReportStatus();

            // Assert
            Assert.That(testArray.Contains(result));
        }

        [Test]
        public void GeneratePentest_ReturnsValidPentestNameFromTheList()
        {
            // Arrange
            string[] testArray = { "Network Penetration Testing", "Web Application Penetration Testing",
                                   "Mobile Application Penetration Testing", "Wireless Penetration Testing",
                                   "Social Engineering", "Physical Security Assessment", "Red Team Exercises",
                                   "Cloud Infrastructure Penetration Testing", "IoT Device Security Assessment" };

            // Act
            string result = SubjectUnderTest.GeneratePentest();

            // Assert
            Assert.That(testArray.Contains(result));
        }
    }
}