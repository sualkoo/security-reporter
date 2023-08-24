using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using webapi.Login.Models;
using webapi.Models;

[assembly: InternalsVisibleTo("cosmosTools_uTest")]

namespace cosmosTools
{
    internal class Generators
    {
        internal int MinRandomValue { get; } = 3;
        internal int MaxRandomValue { get; } = 8;

        public List<string> workingTeam { get; }

        public Generators()
        {
            workingTeam = new List<string> { "John Smith", "Emma Johnson", "Michael Williams", "Emily Brown", "Daniel Jones", "Olivia Davis", "James Wilson",
                    "Ava Martinez", "William Taylor", "Sophia Anderson", "Alexander Thomas", "Mia Hernandez", "Benjamin White", "Charlotte Jackson", "Samuel Lee", "Harper Garcia",
                    "Joseph Martin", "Amelia Thompson", "David Perez", "Abigail Rodriguez", "Andrew Scott", "Grace Evans", "Christopher Turner", "Scarlett Murphy", "Matthew Adams",
                    "Chloe Cook", "Ryan Bell", "Isabella Bailey", "Ethan Murphy", "Elizabeth Murphy", "Anthony Reed", "Victoria Murphy", "Nicholas Rivera", "Madison Clark", "Jonathan Phillips",
                    "Hannah Green", "Robert Mitchell", "Ella Hall", "Matthew Torres", "Lily Wright", "Jacob Flores", "Lillian Walker", "Joshua Hill", "Avery Rivera", "Christopher Johnson",
                    "Zoey Howard", "Josephine Scott", "Natalie Ward", "David Morris", "Aubrey Roberts", "Benjamin Phillips", "Aria James", "William Bennett", "Addison Murphy", "Alexander Henderson"
            };
        }

        public string GenerateRandomEmail(string name)
        {
            var splitName = name.Split(' ');
            return $"{splitName[0].ToLower()}{splitName[1]}@siemens-healthineers.com";
        }

        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFG HIJKLMN OPQRSTU VWXYZabcdef ghijklmnopqrstuvwxyz0123456789 ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public DateTime GenerateRandomDateData(DateTime startDate = default, DateTime endDate = default)
        {
            if (startDate == default)
                startDate = DateTime.Now.AddMonths(-12);

            if (endDate == default)
                endDate = DateTime.Now.AddMonths(12).Date;

            if (endDate <= startDate)
            {
                var temp = startDate;
                startDate = endDate;
                endDate = temp;
            }

            int range = (int)(endDate - startDate).TotalDays;
            var randomDate = startDate.AddDays(new Random().Next(Math.Max(1, range + 1)));

            return randomDate;
        }

        public T GenerateRandomElement<T>(T[] array)
        {
            return array[new Random().Next(array.Length)];
        }

        public string GenerateProjectName()
        {
            var projectNames = new List<string> { "eHealth", "Seamless Stroke Companion", "Syngo.share", "Syngo.via", "AI Rad", "Smart Connect", "IMS", "Aria Clinical Pathway", "HET", "Mobius", "Aria OS" };
            return projectNames[new Random().Next(0, projectNames.Count())];
        }

        public static string GenerateReportStatus()
        {
            var reportStatus = new List<string> { "Pending", "In Progress", "Completed", "Approved", "Rejected", "On Hold", "Cancelled", "Draft", "Error", "Awaiting Review" };
            return reportStatus[new Random().Next(0, reportStatus.Count())];
        }

        public async Task<List<string>> GenerateWords()
        {

            // Replace the URL with the actual API endpoint
            string apiUrl = "https://random-word-api.vercel.app/api?words=200";




            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Send the GET request and await the response
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    // Ensure a successful response
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    List<string> randomWordsList = JsonConvert.DeserializeObject<List<string>>(responseBody);

                    return randomWordsList;

                }
                catch (HttpRequestException ex)
                {
                    // Handle any errors that may occur during the request
                    return new List<string>();

                }
            }
        }

        public List<Comment> GenerateComment(List<string> randomWordsList)
        {
            var comment = new Comment();
            var Comments = new List<Comment> { };

            if (randomWordsList.Count() == 0)
            {
                comment.Text = "Sentence of words.";
                Comments.Add(comment);
                return Comments;
            }

            List<string> sentence = new List<string> { };

            for (int i = 0; i < new Random().Next(10, 20); i++)
            {
                sentence.Add(randomWordsList[new Random().Next(randomWordsList.Count())]);
            }

            comment.Text = string.Join(" ", sentence);

            Comments.Add(comment);
            return Comments;
        }


        public string GeneratePentest()
        {
            var pentestAspects = new List<string> { "Network Penetration Testing", "Web Application Penetration Testing", "Mobile Application Penetration Testing", "Wireless Penetration Testing", "Social Engineering", "Physical Security Assessment", "Red Team Exercises", "Cloud Infrastructure Penetration Testing", "IoT Device Security Assessment" };
            return pentestAspects[new Random().Next(0, pentestAspects.Count())];
        }

        public List<string> GenerateWorkingTeam(List<UserRole> userRoles)
        {

            var newTeam = new List<string>();

            for (int j = 0; j < new Random().Next(MinRandomValue, MaxRandomValue); j++)
            {
                var newUser = userRoles[new Random().Next(0, userRoles.Count())].id;
                if (!newTeam.Contains(newUser))
                {
                    newTeam.Add(newUser);

                }
            }
            return newTeam;
        }


        public List<string> GenerateContacts()
        {
            var contactForClients = new List<string>();

            for (int j = 0; j < new Random().Next(MinRandomValue, MaxRandomValue); j++)
            {
                contactForClients.Add(GenerateRandomEmail(workingTeam[new Random().Next(0, workingTeam.Count())]));
            }
            return contactForClients;
        }
    }
}
