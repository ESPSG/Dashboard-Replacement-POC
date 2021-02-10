using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GraphQLApi.Tests
{
    public class GraphQLApiTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture Fixture;

        public GraphQLApiTests(TestFixture testFixture)
        {
            Fixture = testFixture;
        }

        [Fact]
        public async Task TestBasicStudentInfo()
        {
            var json = await Fixture.QueryGraphQLAsync(@"query{
  students(schoolId:867530011, limit:2)
  { fullName,
    studentUsi,
    gradeLevel,
    gradeLevelSortOrder
    schoolName,
    schoolCategory
  }
}");
            var expectedValue = @"
{
  ""data"": {
    ""students"": [
      {
        ""fullName"": ""Richard G. Balderas"",
        ""studentUsi"": ""100084714"",
        ""gradeLevel"": ""Seventh grade"",
        ""gradeLevelSortOrder"": 7,
        ""schoolName"": ""Cooper"",
        ""schoolCategory"": ""Ungraded""
      },
      {
        ""fullName"": ""Jessica Y. Fenner"",
        ""studentUsi"": ""100083290"",
        ""gradeLevel"": ""Eighth grade"",
        ""gradeLevelSortOrder"": 8,
        ""schoolName"": ""Cooper"",
        ""schoolCategory"": ""Ungraded""
      }
    ]
  }
}
";
            Assert.True(JToken.DeepEquals(JObject.Parse(expectedValue), JObject.Parse(json)));
        }

        [Fact]
        public async Task TestAbsencesForStudentViaOds()
        {
            IDictionary<string, Tuple<string, string>> keyValuePairs = new Dictionary<string, Tuple<string, string>>
            {
                {"192297", new Tuple<string, string>("12","11") },
                {"197180", new Tuple<string, string>("24","13") },
                {"198841", new Tuple<string, string>("59","41") },
                {"201777", new Tuple<string, string>("51","27") },
                {"201151", new Tuple<string, string>("28","10") }
            };
            foreach(var studentKey in keyValuePairs.Keys)
            {
                var query = String.Format(@"query{{studentAttendance(studentKey:{0},schoolKey:867530011){{studentUsi,name,value}}}}", studentKey);
                var json = await Fixture.QueryGraphQLAsync(query);
                string expectedValue = String.Format(@"{{
  ""data"": {{
    ""studentAttendance"": [
      {{
        ""studentUsi"": ""{2}"",
        ""name"": ""Days Absent"",
        ""value"": ""{0}""
      }},
      {{
        ""studentUsi"": ""{2}"",
        ""name"": ""Unexcused Days"",
        ""value"": ""{1}""
      }}
    ]
  }}
}}", keyValuePairs[studentKey].Item1, keyValuePairs[studentKey].Item2,studentKey);
                Assert.True(JToken.DeepEquals(JObject.Parse(expectedValue), JObject.Parse(json)));
            }
            
        }
        [Fact]
        public async Task TestAbsencesForSchoolViaOds()
        {
            var json = await Fixture.QueryGraphQLAsync(@"query{studentAttendance(schoolKey:867530011){studentUsi,name,value}}");
            var expectedValue = @"{
  ""data"": {
    ""studentAttendance"": [
      {
        ""studentUsi"": ""192297"",
        ""name"": ""Days Absent"",
        ""value"": ""12""
      },
      {
        ""studentUsi"": ""192297"",
        ""name"": ""Unexcused Days"",
        ""value"": ""11""
      },
      {
        ""studentUsi"": ""193049"",
        ""name"": ""Days Absent"",
        ""value"": ""4""
      },
      {
        ""studentUsi"": ""193049"",
        ""name"": ""Unexcused Days"",
        ""value"": ""3""
      },
      {
        ""studentUsi"": ""193964"",
        ""name"": ""Days Absent"",
        ""value"": ""40""
      },
      {
        ""studentUsi"": ""193964"",
        ""name"": ""Unexcused Days"",
        ""value"": ""18""
      },
      {
        ""studentUsi"": ""196576"",
        ""name"": ""Days Absent"",
        ""value"": ""61""
      },
      {
        ""studentUsi"": ""196576"",
        ""name"": ""Unexcused Days"",
        ""value"": ""44""
      },
      {
        ""studentUsi"": ""196738"",
        ""name"": ""Days Absent"",
        ""value"": ""16""
      },
      {
        ""studentUsi"": ""196738"",
        ""name"": ""Unexcused Days"",
        ""value"": ""8""
      },
      {
        ""studentUsi"": ""197180"",
        ""name"": ""Days Absent"",
        ""value"": ""24""
      },
      {
        ""studentUsi"": ""197180"",
        ""name"": ""Unexcused Days"",
        ""value"": ""13""
      },
      {
        ""studentUsi"": ""197469"",
        ""name"": ""Days Absent"",
        ""value"": ""15""
      },
      {
        ""studentUsi"": ""197469"",
        ""name"": ""Unexcused Days"",
        ""value"": ""5""
      },
      {
        ""studentUsi"": ""197507"",
        ""name"": ""Days Absent"",
        ""value"": ""42""
      },
      {
        ""studentUsi"": ""197507"",
        ""name"": ""Unexcused Days"",
        ""value"": ""40""
      },
      {
        ""studentUsi"": ""198443"",
        ""name"": ""Days Absent"",
        ""value"": ""83""
      },
      {
        ""studentUsi"": ""198443"",
        ""name"": ""Unexcused Days"",
        ""value"": ""53""
      },
      {
        ""studentUsi"": ""198591"",
        ""name"": ""Days Absent"",
        ""value"": ""32""
      },
      {
        ""studentUsi"": ""198591"",
        ""name"": ""Unexcused Days"",
        ""value"": ""10""
      },
      {
        ""studentUsi"": ""198602"",
        ""name"": ""Days Absent"",
        ""value"": ""50""
      },
      {
        ""studentUsi"": ""198602"",
        ""name"": ""Unexcused Days"",
        ""value"": ""30""
      },
      {
        ""studentUsi"": ""198639"",
        ""name"": ""Days Absent"",
        ""value"": ""51""
      },
      {
        ""studentUsi"": ""198639"",
        ""name"": ""Unexcused Days"",
        ""value"": ""42""
      },
      {
        ""studentUsi"": ""198644"",
        ""name"": ""Days Absent"",
        ""value"": ""22""
      },
      {
        ""studentUsi"": ""198644"",
        ""name"": ""Unexcused Days"",
        ""value"": ""9""
      },
      {
        ""studentUsi"": ""198729"",
        ""name"": ""Days Absent"",
        ""value"": ""8""
      },
      {
        ""studentUsi"": ""198729"",
        ""name"": ""Unexcused Days"",
        ""value"": ""8""
      },
      {
        ""studentUsi"": ""198841"",
        ""name"": ""Days Absent"",
        ""value"": ""59""
      },
      {
        ""studentUsi"": ""198841"",
        ""name"": ""Unexcused Days"",
        ""value"": ""41""
      },
      {
        ""studentUsi"": ""198855"",
        ""name"": ""Days Absent"",
        ""value"": ""4""
      },
      {
        ""studentUsi"": ""198855"",
        ""name"": ""Unexcused Days"",
        ""value"": ""4""
      },
      {
        ""studentUsi"": ""198887"",
        ""name"": ""Days Absent"",
        ""value"": ""108""
      },
      {
        ""studentUsi"": ""198887"",
        ""name"": ""Unexcused Days"",
        ""value"": ""97""
      },
      {
        ""studentUsi"": ""198916"",
        ""name"": ""Days Absent"",
        ""value"": ""65""
      },
      {
        ""studentUsi"": ""198916"",
        ""name"": ""Unexcused Days"",
        ""value"": ""29""
      },
      {
        ""studentUsi"": ""198999"",
        ""name"": ""Days Absent"",
        ""value"": ""23""
      },
      {
        ""studentUsi"": ""198999"",
        ""name"": ""Unexcused Days"",
        ""value"": ""13""
      },
      {
        ""studentUsi"": ""199018"",
        ""name"": ""Days Absent"",
        ""value"": ""36""
      },
      {
        ""studentUsi"": ""199018"",
        ""name"": ""Unexcused Days"",
        ""value"": ""32""
      },
      {
        ""studentUsi"": ""199021"",
        ""name"": ""Days Absent"",
        ""value"": ""23""
      },
      {
        ""studentUsi"": ""199021"",
        ""name"": ""Unexcused Days"",
        ""value"": ""6""
      },
      {
        ""studentUsi"": ""199329"",
        ""name"": ""Days Absent"",
        ""value"": ""67""
      },
      {
        ""studentUsi"": ""199329"",
        ""name"": ""Unexcused Days"",
        ""value"": ""60""
      },
      {
        ""studentUsi"": ""199615"",
        ""name"": ""Days Absent"",
        ""value"": ""6""
      },
      {
        ""studentUsi"": ""199615"",
        ""name"": ""Unexcused Days"",
        ""value"": ""5""
      },
      {
        ""studentUsi"": ""199774"",
        ""name"": ""Days Absent"",
        ""value"": ""46""
      },
      {
        ""studentUsi"": ""199774"",
        ""name"": ""Unexcused Days"",
        ""value"": ""35""
      },
      {
        ""studentUsi"": ""199788"",
        ""name"": ""Days Absent"",
        ""value"": ""72""
      },
      {
        ""studentUsi"": ""199788"",
        ""name"": ""Unexcused Days"",
        ""value"": ""58""
      },
      {
        ""studentUsi"": ""200026"",
        ""name"": ""Days Absent"",
        ""value"": ""6""
      },
      {
        ""studentUsi"": ""200026"",
        ""name"": ""Unexcused Days"",
        ""value"": ""5""
      },
      {
        ""studentUsi"": ""200488"",
        ""name"": ""Days Absent"",
        ""value"": ""14""
      },
      {
        ""studentUsi"": ""200488"",
        ""name"": ""Unexcused Days"",
        ""value"": ""5""
      },
      {
        ""studentUsi"": ""200608"",
        ""name"": ""Days Absent"",
        ""value"": ""54""
      },
      {
        ""studentUsi"": ""200608"",
        ""name"": ""Unexcused Days"",
        ""value"": ""34""
      },
      {
        ""studentUsi"": ""200902"",
        ""name"": ""Days Absent"",
        ""value"": ""33""
      },
      {
        ""studentUsi"": ""200902"",
        ""name"": ""Unexcused Days"",
        ""value"": ""10""
      },
      {
        ""studentUsi"": ""200913"",
        ""name"": ""Days Absent"",
        ""value"": ""34""
      },
      {
        ""studentUsi"": ""200913"",
        ""name"": ""Unexcused Days"",
        ""value"": ""34""
      },
      {
        ""studentUsi"": ""201002"",
        ""name"": ""Days Absent"",
        ""value"": ""9""
      },
      {
        ""studentUsi"": ""201002"",
        ""name"": ""Unexcused Days"",
        ""value"": ""1""
      },
      {
        ""studentUsi"": ""201076"",
        ""name"": ""Days Absent"",
        ""value"": ""5""
      },
      {
        ""studentUsi"": ""201076"",
        ""name"": ""Unexcused Days"",
        ""value"": ""2""
      },
      {
        ""studentUsi"": ""201151"",
        ""name"": ""Days Absent"",
        ""value"": ""28""
      },
      {
        ""studentUsi"": ""201151"",
        ""name"": ""Unexcused Days"",
        ""value"": ""10""
      },
      {
        ""studentUsi"": ""201173"",
        ""name"": ""Days Absent"",
        ""value"": ""13""
      },
      {
        ""studentUsi"": ""201173"",
        ""name"": ""Unexcused Days"",
        ""value"": ""6""
      },
      {
        ""studentUsi"": ""201221"",
        ""name"": ""Days Absent"",
        ""value"": ""10""
      },
      {
        ""studentUsi"": ""201221"",
        ""name"": ""Unexcused Days"",
        ""value"": ""4""
      },
      {
        ""studentUsi"": ""201493"",
        ""name"": ""Days Absent"",
        ""value"": ""31""
      },
      {
        ""studentUsi"": ""201493"",
        ""name"": ""Unexcused Days"",
        ""value"": ""20""
      },
      {
        ""studentUsi"": ""201506"",
        ""name"": ""Days Absent"",
        ""value"": ""30""
      },
      {
        ""studentUsi"": ""201506"",
        ""name"": ""Unexcused Days"",
        ""value"": ""28""
      },
      {
        ""studentUsi"": ""201591"",
        ""name"": ""Days Absent"",
        ""value"": ""18""
      },
      {
        ""studentUsi"": ""201591"",
        ""name"": ""Unexcused Days"",
        ""value"": ""8""
      },
      {
        ""studentUsi"": ""201597"",
        ""name"": ""Days Absent"",
        ""value"": ""38""
      },
      {
        ""studentUsi"": ""201597"",
        ""name"": ""Unexcused Days"",
        ""value"": ""20""
      },
      {
        ""studentUsi"": ""201777"",
        ""name"": ""Days Absent"",
        ""value"": ""51""
      },
      {
        ""studentUsi"": ""201777"",
        ""name"": ""Unexcused Days"",
        ""value"": ""27""
      },
      {
        ""studentUsi"": ""202266"",
        ""name"": ""Days Absent"",
        ""value"": ""26""
      },
      {
        ""studentUsi"": ""202266"",
        ""name"": ""Unexcused Days"",
        ""value"": ""15""
      },
      {
        ""studentUsi"": ""202323"",
        ""name"": ""Days Absent"",
        ""value"": ""59""
      },
      {
        ""studentUsi"": ""202323"",
        ""name"": ""Unexcused Days"",
        ""value"": ""36""
      },
      {
        ""studentUsi"": ""202994"",
        ""name"": ""Days Absent"",
        ""value"": ""9""
      },
      {
        ""studentUsi"": ""202994"",
        ""name"": ""Unexcused Days"",
        ""value"": ""8""
      },
      {
        ""studentUsi"": ""203022"",
        ""name"": ""Days Absent"",
        ""value"": ""43""
      },
      {
        ""studentUsi"": ""203022"",
        ""name"": ""Unexcused Days"",
        ""value"": ""33""
      },
      {
        ""studentUsi"": ""203036"",
        ""name"": ""Days Absent"",
        ""value"": ""34""
      },
      {
        ""studentUsi"": ""203036"",
        ""name"": ""Unexcused Days"",
        ""value"": ""18""
      },
      {
        ""studentUsi"": ""203097"",
        ""name"": ""Days Absent"",
        ""value"": ""12""
      },
      {
        ""studentUsi"": ""203097"",
        ""name"": ""Unexcused Days"",
        ""value"": ""8""
      },
      {
        ""studentUsi"": ""203684"",
        ""name"": ""Days Absent"",
        ""value"": ""5""
      },
      {
        ""studentUsi"": ""203684"",
        ""name"": ""Unexcused Days"",
        ""value"": ""2""
      },
      {
        ""studentUsi"": ""203972"",
        ""name"": ""Days Absent"",
        ""value"": ""13""
      },
      {
        ""studentUsi"": ""203972"",
        ""name"": ""Unexcused Days"",
        ""value"": ""12""
      },
      {
        ""studentUsi"": ""204029"",
        ""name"": ""Days Absent"",
        ""value"": ""9""
      },
      {
        ""studentUsi"": ""204029"",
        ""name"": ""Unexcused Days"",
        ""value"": ""2""
      },
      {
        ""studentUsi"": ""204270"",
        ""name"": ""Days Absent"",
        ""value"": ""11""
      },
      {
        ""studentUsi"": ""204270"",
        ""name"": ""Unexcused Days"",
        ""value"": ""7""
      },
      {
        ""studentUsi"": ""204570"",
        ""name"": ""Days Absent"",
        ""value"": ""34""
      },
      {
        ""studentUsi"": ""204570"",
        ""name"": ""Unexcused Days"",
        ""value"": ""22""
      },
      {
        ""studentUsi"": ""204726"",
        ""name"": ""Days Absent"",
        ""value"": ""47""
      },
      {
        ""studentUsi"": ""204726"",
        ""name"": ""Unexcused Days"",
        ""value"": ""22""
      },
      {
        ""studentUsi"": ""204874"",
        ""name"": ""Days Absent"",
        ""value"": ""45""
      },
      {
        ""studentUsi"": ""204874"",
        ""name"": ""Unexcused Days"",
        ""value"": ""11""
      },
      {
        ""studentUsi"": ""205575"",
        ""name"": ""Days Absent"",
        ""value"": ""90""
      },
      {
        ""studentUsi"": ""205575"",
        ""name"": ""Unexcused Days"",
        ""value"": ""71""
      },
      {
        ""studentUsi"": ""206374"",
        ""name"": ""Days Absent"",
        ""value"": ""35""
      },
      {
        ""studentUsi"": ""206374"",
        ""name"": ""Unexcused Days"",
        ""value"": ""21""
      },
      {
        ""studentUsi"": ""207020"",
        ""name"": ""Days Absent"",
        ""value"": ""123""
      },
      {
        ""studentUsi"": ""207020"",
        ""name"": ""Unexcused Days"",
        ""value"": ""70""
      },
      {
        ""studentUsi"": ""208712"",
        ""name"": ""Days Absent"",
        ""value"": ""41""
      },
      {
        ""studentUsi"": ""208712"",
        ""name"": ""Unexcused Days"",
        ""value"": ""18""
      },
      {
        ""studentUsi"": ""209018"",
        ""name"": ""Days Absent"",
        ""value"": ""22""
      },
      {
        ""studentUsi"": ""209018"",
        ""name"": ""Unexcused Days"",
        ""value"": ""11""
      },
      {
        ""studentUsi"": ""209445"",
        ""name"": ""Days Absent"",
        ""value"": ""27""
      },
      {
        ""studentUsi"": ""209445"",
        ""name"": ""Unexcused Days"",
        ""value"": ""8""
      },
      {
        ""studentUsi"": ""211973"",
        ""name"": ""Days Absent"",
        ""value"": ""26""
      },
      {
        ""studentUsi"": ""211973"",
        ""name"": ""Unexcused Days"",
        ""value"": ""8""
      },
      {
        ""studentUsi"": ""212534"",
        ""name"": ""Days Absent"",
        ""value"": ""29""
      },
      {
        ""studentUsi"": ""212534"",
        ""name"": ""Unexcused Days"",
        ""value"": ""7""
      },
      {
        ""studentUsi"": ""214596"",
        ""name"": ""Days Absent"",
        ""value"": ""51""
      },
      {
        ""studentUsi"": ""214596"",
        ""name"": ""Unexcused Days"",
        ""value"": ""17""
      },
      {
        ""studentUsi"": ""217115"",
        ""name"": ""Days Absent"",
        ""value"": ""21""
      },
      {
        ""studentUsi"": ""217115"",
        ""name"": ""Unexcused Days"",
        ""value"": ""7""
      },
      {
        ""studentUsi"": ""217641"",
        ""name"": ""Days Absent"",
        ""value"": ""51""
      },
      {
        ""studentUsi"": ""217641"",
        ""name"": ""Unexcused Days"",
        ""value"": ""43""
      },
      {
        ""studentUsi"": ""218771"",
        ""name"": ""Days Absent"",
        ""value"": ""6""
      },
      {
        ""studentUsi"": ""218771"",
        ""name"": ""Unexcused Days"",
        ""value"": ""2""
      },
      {
        ""studentUsi"": ""224744"",
        ""name"": ""Days Absent"",
        ""value"": ""57""
      },
      {
        ""studentUsi"": ""224744"",
        ""name"": ""Unexcused Days"",
        ""value"": ""36""
      },
      {
        ""studentUsi"": ""224775"",
        ""name"": ""Days Absent"",
        ""value"": ""15""
      },
      {
        ""studentUsi"": ""224775"",
        ""name"": ""Unexcused Days"",
        ""value"": ""7""
      },
      {
        ""studentUsi"": ""226678"",
        ""name"": ""Days Absent"",
        ""value"": ""54""
      },
      {
        ""studentUsi"": ""226678"",
        ""name"": ""Unexcused Days"",
        ""value"": ""43""
      },
      {
        ""studentUsi"": ""235186"",
        ""name"": ""Days Absent"",
        ""value"": ""19""
      },
      {
        ""studentUsi"": ""235186"",
        ""name"": ""Unexcused Days"",
        ""value"": ""17""
      },
      {
        ""studentUsi"": ""235909"",
        ""name"": ""Days Absent"",
        ""value"": ""13""
      },
      {
        ""studentUsi"": ""235909"",
        ""name"": ""Unexcused Days"",
        ""value"": ""4""
      },
      {
        ""studentUsi"": ""235950"",
        ""name"": ""Days Absent"",
        ""value"": ""59""
      },
      {
        ""studentUsi"": ""235950"",
        ""name"": ""Unexcused Days"",
        ""value"": ""56""
      },
      {
        ""studentUsi"": ""236792"",
        ""name"": ""Days Absent"",
        ""value"": ""3""
      },
      {
        ""studentUsi"": ""236792"",
        ""name"": ""Unexcused Days"",
        ""value"": ""3""
      }
    ]
  }
}";
            JToken actualToken = JObject.Parse(json).First.First.First.First;
            JToken expectedToken = JObject.Parse(expectedValue).First.First.First.First;
            IEnumerator<JToken> actualTokenRootEnumerator = actualToken.Children().GetEnumerator();
            IEnumerator<JToken> expectedTokenRootEnumerator = expectedToken.Children().GetEnumerator();
            while (actualTokenRootEnumerator.MoveNext() && expectedTokenRootEnumerator.MoveNext())
            {
                JToken actualTokenRoot = actualTokenRootEnumerator.Current;
                JToken expectedTokenRoot = expectedTokenRootEnumerator.Current;

                try
                {
                    Assert.True(JToken.DeepEquals(actualTokenRoot, expectedTokenRoot));
                }
                catch (Xunit.Sdk.TrueException e)
                {
                    List<string> failingKeys = new List<string>() { "198887", "214596", "204726" };
                    Assert.Contains(actualTokenRoot.Value<string>("studentUsi"), failingKeys);
                    Assert.Contains(expectedTokenRoot.Value<string>("studentUsi"), failingKeys);

                }

            }

            //Assert.True(JToken.DeepEquals(JObject.Parse(expectedValue), JObject.Parse(json)));
        }

        [Fact]
        public async Task TestAbsencesViaOdsForFailingStudents()
        {
            for (int count = 0; count < 100; count++)
            {
                try
                {
                    var json = await Fixture.QueryGraphQLAsync(@"query{studentAttendance{studentUsi,name,value}}");
                    var expectedValue = @"{
  ""data"": {
    ""studentAttendance"": [
      {
        ""studentUsi"": ""198887"",
        ""name"": ""Days Absent"",
        ""value"": ""108""
      },
      {
        ""studentUsi"": ""198887"",
        ""name"": ""Unexcused Days"",
        ""value"": ""96""
      },
      {
        ""studentUsi"": ""204726"",
        ""name"": ""Days Absent"",
        ""value"": ""47""
      },
      {
        ""studentUsi"": ""204726"",
        ""name"": ""Unexcused Days"",
        ""value"": ""22""
      },
        {
        ""studentUsi"": ""214596"",
        ""name"": ""Days Absent"",
        ""value"": ""51""
      },
      {
        ""studentUsi"": ""214596"",
        ""name"": ""Unexcused Days"",
        ""value"": ""16""
      }
     ]
  }
}";
                    
                    Assert.True(JToken.DeepEquals(JObject.Parse(expectedValue), JObject.Parse(json)));
                }
                catch (Exception error)
                {

                }
            }
        }


        [Fact]
        public async Task TestStudentDetailsWithMetrics()
        {
            var json = await Fixture.QueryGraphQLAsync(@"
query{
  students(schoolId:867530011,offset:100,limit:5)
  { fullName firstName addressLine1 city currentAge dateOfBirth gender 
    gradeLevel hispanicLatinoEthnicity homeLanguage lateEnrollment
    middleName race studentUsi gradeLevelListDisplayText
    gradeLevelSortOrder language parentMilitary placeOfBirth schoolCategory
    schoolId schoolName
    studentIndicators{ displayOrder name status studentUsi type}
    studentParentInformation{addressLine1 addressLine2 emailAddress fullName
    primaryContact relation studentUsi telephoneNumber relation}
    studentSchoolInformation{ dateOfEntry dateOfWithdrawal expectedGraduationYear
    gradeLevel graduationPlan homeroom lateEnrollment schoolId studentUsi}
    metrics(schoolId:867530011,metricIds:[3,4,5]){
      name
      parentId
      parentName
      trendDirection
      state
      value
    }
  }
}
");
            var expectedValue = @"{
  ""data"": {
    ""students"": [
      {
        ""fullName"": ""Sunny Lessner"",
        ""firstName"": ""Sunny"",
        ""addressLine1"": ""353 Montview Cir"",
        ""city"": ""Elgin"",
        ""currentAge"": 18,
        ""dateOfBirth"": ""0001-01-01"",
        ""gender"": ""Female"",
        ""gradeLevel"": ""Twelfth grade"",
        ""hispanicLatinoEthnicity"": ""No"",
        ""homeLanguage"": null,
        ""lateEnrollment"": ""Yes"",
        ""middleName"": null,
        ""race"": ""White"",
        ""studentUsi"": ""100141791"",
        ""gradeLevelListDisplayText"": ""12th      "",
        ""gradeLevelSortOrder"": 12,
        ""language"": null,
        ""parentMilitary"": null,
        ""placeOfBirth"": null,
        ""schoolCategory"": ""Ungraded"",
        ""schoolId"": 867530011,
        ""schoolName"": ""Cooper"",
        ""studentIndicators"": [
          {
            ""displayOrder"": null,
            ""name"": ""At Risk"",
            ""status"": true,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Economically Disadvantaged"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Free or Reduced Priced Lunch Eligible"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Homeless"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Immigrant"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 1"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 2"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Migrant"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Over Age"",
            ""status"": true,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": 0,
            ""name"": ""Bilingual"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 1,
            ""name"": ""Bilingual Summer"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 2,
            ""name"": ""Career and Technical Education"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 3,
            ""name"": ""English as a Second Language (ESL)"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 4,
            ""name"": ""Gifted and Talented"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 5,
            ""name"": ""Section 504 Placement"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 6,
            ""name"": ""Special Education"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 7,
            ""name"": ""Title I Part A"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          }
        ],
        ""studentParentInformation"": [
          {
            ""addressLine1"": ""353 Montview Cir"",
            ""addressLine2"": null,
            ""emailAddress"": ""maeve.wheeler@tsds.org"",
            ""fullName"": ""Maeve Wheeler"",
            ""primaryContact"": true,
            ""relation"": ""Other"",
            ""studentUsi"": ""100141791"",
            ""telephoneNumber"": ""(281)-836-5086""
          }
        ],
        ""studentSchoolInformation"": [
          {
            ""dateOfEntry"": ""2012-03-19"",
            ""dateOfWithdrawal"": null,
            ""expectedGraduationYear"": ""2012"",
            ""gradeLevel"": ""Twelfth grade"",
            ""graduationPlan"": ""Career and Technical"",
            ""homeroom"": """",
            ""lateEnrollment"": ""Yes"",
            ""schoolId"": 867530011,
            ""studentUsi"": ""100141791""
          }
        ],
        ""metrics"": [
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 0,
            ""parentName"": """",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 0,
            ""parentName"": """",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 0,
            ""parentName"": """",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          }
        ]
      },
      {
        ""fullName"": ""Chad Patton"",
        ""firstName"": ""Chad"",
        ""addressLine1"": ""491 Fairview Cir"",
        ""city"": ""Taft"",
        ""currentAge"": 19,
        ""dateOfBirth"": ""0001-01-01"",
        ""gender"": ""Male"",
        ""gradeLevel"": ""Twelfth grade"",
        ""hispanicLatinoEthnicity"": ""No"",
        ""homeLanguage"": null,
        ""lateEnrollment"": ""Yes"",
        ""middleName"": null,
        ""race"": ""White"",
        ""studentUsi"": ""100140764"",
        ""gradeLevelListDisplayText"": ""12th      "",
        ""gradeLevelSortOrder"": 12,
        ""language"": null,
        ""parentMilitary"": null,
        ""placeOfBirth"": null,
        ""schoolCategory"": ""Ungraded"",
        ""schoolId"": 867530011,
        ""schoolName"": ""Cooper"",
        ""studentIndicators"": [
          {
            ""displayOrder"": null,
            ""name"": ""At Risk"",
            ""status"": true,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Economically Disadvantaged"",
            ""status"": true,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Free or Reduced Priced Lunch Eligible"",
            ""status"": true,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Homeless"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Immigrant"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 1"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 2"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Migrant"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Over Age"",
            ""status"": true,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": 0,
            ""name"": ""Bilingual"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 1,
            ""name"": ""Bilingual Summer"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 2,
            ""name"": ""Career and Technical Education"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 3,
            ""name"": ""English as a Second Language (ESL)"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 4,
            ""name"": ""Gifted and Talented"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 5,
            ""name"": ""Section 504 Placement"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 6,
            ""name"": ""Special Education"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 7,
            ""name"": ""Title I Part A"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          }
        ],
        ""studentParentInformation"": [
          {
            ""addressLine1"": ""491 Fairview Cir"",
            ""addressLine2"": null,
            ""emailAddress"": null,
            ""fullName"": ""Zion Hickman"",
            ""primaryContact"": true,
            ""relation"": ""Other"",
            ""studentUsi"": ""100140764"",
            ""telephoneNumber"": ""(927)-897-6988""
          }
        ],
        ""studentSchoolInformation"": [
          {
            ""dateOfEntry"": ""2011-09-27"",
            ""dateOfWithdrawal"": null,
            ""expectedGraduationYear"": ""2012"",
            ""gradeLevel"": ""Twelfth grade"",
            ""graduationPlan"": ""Career and Technical"",
            ""homeroom"": ""Angelica D. Thompson, Aaron Q. Patton"",
            ""lateEnrollment"": ""Yes"",
            ""schoolId"": 867530011,
            ""studentUsi"": ""100140764""
          }
        ],
        ""metrics"": [
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 0,
            ""parentName"": """",
            ""trendDirection"": 0,
            ""state"": ""Bad"",
            ""value"": ""70.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 0,
            ""parentName"": """",
            ""trendDirection"": 0,
            ""state"": ""Bad"",
            ""value"": ""72.5%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 0,
            ""parentName"": """",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""60.7%""
          }
        ]
      },
      {
        ""fullName"": ""Jason Siebold"",
        ""firstName"": ""Jason"",
        ""addressLine1"": ""427 Kiowa Ct"",
        ""city"": ""Huntsville"",
        ""currentAge"": 17,
        ""dateOfBirth"": ""0001-01-01"",
        ""gender"": ""Male"",
        ""gradeLevel"": ""Twelfth grade"",
        ""hispanicLatinoEthnicity"": ""Yes"",
        ""homeLanguage"": ""English"",
        ""lateEnrollment"": ""Yes"",
        ""middleName"": null,
        ""race"": ""White"",
        ""studentUsi"": ""100067117"",
        ""gradeLevelListDisplayText"": ""12th      "",
        ""gradeLevelSortOrder"": 12,
        ""language"": null,
        ""parentMilitary"": null,
        ""placeOfBirth"": null,
        ""schoolCategory"": ""Ungraded"",
        ""schoolId"": 867530011,
        ""schoolName"": ""Cooper"",
        ""studentIndicators"": [
          {
            ""displayOrder"": null,
            ""name"": ""At Risk"",
            ""status"": true,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Economically Disadvantaged"",
            ""status"": true,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Free or Reduced Priced Lunch Eligible"",
            ""status"": true,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Homeless"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Immigrant"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 1"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 2"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Migrant"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Over Age"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": 0,
            ""name"": ""Bilingual"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 1,
            ""name"": ""Bilingual Summer"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 2,
            ""name"": ""Career and Technical Education"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 3,
            ""name"": ""English as a Second Language (ESL)"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 4,
            ""name"": ""Gifted and Talented"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 5,
            ""name"": ""Section 504 Placement"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 6,
            ""name"": ""Special Education"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 7,
            ""name"": ""Title I Part A"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          }
        ],
        ""studentParentInformation"": [
          {
            ""addressLine1"": ""427 Kiowa Ct"",
            ""addressLine2"": null,
            ""emailAddress"": null,
            ""fullName"": ""Kade Rodas"",
            ""primaryContact"": true,
            ""relation"": ""Mother"",
            ""studentUsi"": ""100067117"",
            ""telephoneNumber"": ""(708)-863-8898""
          }
        ],
        ""studentSchoolInformation"": [
          {
            ""dateOfEntry"": ""2011-09-09"",
            ""dateOfWithdrawal"": null,
            ""expectedGraduationYear"": ""2012"",
            ""gradeLevel"": ""Twelfth grade"",
            ""graduationPlan"": ""Career and Technical"",
            ""homeroom"": ""Angelica D. Thompson, Rachel H. Wentz"",
            ""lateEnrollment"": ""Yes"",
            ""schoolId"": 867530011,
            ""studentUsi"": ""100067117""
          }
        ],
        ""metrics"": [
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 0,
            ""parentName"": """",
            ""trendDirection"": 1,
            ""state"": ""Bad"",
            ""value"": ""85.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 0,
            ""parentName"": """",
            ""trendDirection"": -1,
            ""state"": ""Bad"",
            ""value"": ""72.5%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 0,
            ""parentName"": """",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""80.2%""
          }
        ]
      },
      {
        ""fullName"": ""Charlie B. Mosley"",
        ""firstName"": ""Charlie"",
        ""addressLine1"": ""384 Green Loop Rd"",
        ""city"": ""Gun Barrel City"",
        ""currentAge"": 16,
        ""dateOfBirth"": ""0001-01-01"",
        ""gender"": ""Male"",
        ""gradeLevel"": ""Twelfth grade"",
        ""hispanicLatinoEthnicity"": ""No"",
        ""homeLanguage"": ""English"",
        ""lateEnrollment"": ""Yes"",
        ""middleName"": ""B"",
        ""race"": ""Black - African American"",
        ""studentUsi"": ""100067128"",
        ""gradeLevelListDisplayText"": ""12th      "",
        ""gradeLevelSortOrder"": 12,
        ""language"": null,
        ""parentMilitary"": null,
        ""placeOfBirth"": null,
        ""schoolCategory"": ""Ungraded"",
        ""schoolId"": 867530011,
        ""schoolName"": ""Cooper"",
        ""studentIndicators"": [
          {
            ""displayOrder"": null,
            ""name"": ""At Risk"",
            ""status"": true,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Economically Disadvantaged"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Free or Reduced Priced Lunch Eligible"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Homeless"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Immigrant"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 1"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 2"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Migrant"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Over Age"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": 0,
            ""name"": ""Bilingual"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 1,
            ""name"": ""Bilingual Summer"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 2,
            ""name"": ""Career and Technical Education"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 3,
            ""name"": ""English as a Second Language (ESL)"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 4,
            ""name"": ""Gifted and Talented"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 5,
            ""name"": ""Section 504 Placement"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 6,
            ""name"": ""Special Education"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 7,
            ""name"": ""Title I Part A"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          }
        ],
        ""studentParentInformation"": [
          {
            ""addressLine1"": ""384 Green Loop Rd"",
            ""addressLine2"": null,
            ""emailAddress"": ""rory.becks@tsds.org"",
            ""fullName"": ""Rory Becks"",
            ""primaryContact"": true,
            ""relation"": ""Mother"",
            ""studentUsi"": ""100067128"",
            ""telephoneNumber"": ""(930)-948-4193""
          }
        ],
        ""studentSchoolInformation"": [
          {
            ""dateOfEntry"": ""2012-05-18"",
            ""dateOfWithdrawal"": null,
            ""expectedGraduationYear"": ""2012"",
            ""gradeLevel"": ""Twelfth grade"",
            ""graduationPlan"": ""Career and Technical"",
            ""homeroom"": ""Angelica D. Thompson, Thomas N. Moore"",
            ""lateEnrollment"": ""Yes"",
            ""schoolId"": 867530011,
            ""studentUsi"": ""100067128""
          }
        ],
        ""metrics"": [
          {
            ""name"": ""Year to Date"",
            ""parentId"": 0,
            ""parentName"": """",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          }
        ]
      },
      {
        ""fullName"": ""Damiyun L. Dudley"",
        ""firstName"": ""Damiyun"",
        ""addressLine1"": ""610 Dodson Rd"",
        ""city"": ""Balcones Heights"",
        ""currentAge"": 17,
        ""dateOfBirth"": ""0001-01-01"",
        ""gender"": ""Male"",
        ""gradeLevel"": ""Twelfth grade"",
        ""hispanicLatinoEthnicity"": ""Yes"",
        ""homeLanguage"": ""English"",
        ""lateEnrollment"": ""Yes"",
        ""middleName"": ""L"",
        ""race"": ""White"",
        ""studentUsi"": ""100067689"",
        ""gradeLevelListDisplayText"": ""12th      "",
        ""gradeLevelSortOrder"": 12,
        ""language"": null,
        ""parentMilitary"": null,
        ""placeOfBirth"": null,
        ""schoolCategory"": ""Ungraded"",
        ""schoolId"": 867530011,
        ""schoolName"": ""Cooper"",
        ""studentIndicators"": [
          {
            ""displayOrder"": null,
            ""name"": ""At Risk"",
            ""status"": true,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Economically Disadvantaged"",
            ""status"": true,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Free or Reduced Priced Lunch Eligible"",
            ""status"": true,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Homeless"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Immigrant"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 1"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 2"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Migrant"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Over Age"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": 0,
            ""name"": ""Bilingual"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 1,
            ""name"": ""Bilingual Summer"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 2,
            ""name"": ""Career and Technical Education"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 3,
            ""name"": ""English as a Second Language (ESL)"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 4,
            ""name"": ""Gifted and Talented"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 5,
            ""name"": ""Section 504 Placement"",
            ""status"": true,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 6,
            ""name"": ""Special Education"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 7,
            ""name"": ""Title I Part A"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          }
        ],
        ""studentParentInformation"": [
          {
            ""addressLine1"": ""610 Dodson Rd"",
            ""addressLine2"": null,
            ""emailAddress"": ""parker.woody@tsds.org"",
            ""fullName"": ""Parker Woody"",
            ""primaryContact"": true,
            ""relation"": ""Mother"",
            ""studentUsi"": ""100067689"",
            ""telephoneNumber"": ""(215)-732-1037""
          }
        ],
        ""studentSchoolInformation"": [
          {
            ""dateOfEntry"": ""2012-05-18"",
            ""dateOfWithdrawal"": null,
            ""expectedGraduationYear"": ""2012"",
            ""gradeLevel"": ""Twelfth grade"",
            ""graduationPlan"": ""Career and Technical"",
            ""homeroom"": ""Ada S. Canty"",
            ""lateEnrollment"": ""Yes"",
            ""schoolId"": 867530011,
            ""studentUsi"": ""100067689""
          }
        ],
        ""metrics"": [
          {
            ""name"": ""Year to Date"",
            ""parentId"": 0,
            ""parentName"": """",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          }
        ]
      }
    ]
  }
}";
            Assert.True(JToken.DeepEquals(JObject.Parse(expectedValue), JObject.Parse(json)));
        }

        [Fact]
        public async Task TestStudentDetailsWithMetricSubTree()
        {
            var json = await Fixture.QueryGraphQLAsync(@"query{
  students(schoolId:867530011,offset:100,limit:5)
  { fullName firstName addressLine1 city currentAge dateOfBirth gender 
    gradeLevel hispanicLatinoEthnicity homeLanguage lateEnrollment
    middleName race studentUsi gradeLevelListDisplayText
    gradeLevelSortOrder language parentMilitary placeOfBirth schoolCategory
    schoolId schoolName
    studentIndicators{ displayOrder name status studentUsi type}
    studentParentInformation{addressLine1 addressLine2 emailAddress fullName
    primaryContact relation studentUsi telephoneNumber relation}
    studentSchoolInformation{ dateOfEntry dateOfWithdrawal expectedGraduationYear
    gradeLevel graduationPlan homeroom lateEnrollment schoolId studentUsi}
    metrics(schoolId:867530011,metricId:65){
      name
      parentId
      parentName
      trendDirection
      state
      value
    }
  }
}
");
            var expectedValue = @"{
  ""data"": {
    ""students"": [
      {
        ""fullName"": ""Sunny Lessner"",
        ""firstName"": ""Sunny"",
        ""addressLine1"": ""353 Montview Cir"",
        ""city"": ""Elgin"",
        ""currentAge"": 18,
        ""dateOfBirth"": ""0001-01-01"",
        ""gender"": ""Female"",
        ""gradeLevel"": ""Twelfth grade"",
        ""hispanicLatinoEthnicity"": ""No"",
        ""homeLanguage"": null,
        ""lateEnrollment"": ""Yes"",
        ""middleName"": null,
        ""race"": ""White"",
        ""studentUsi"": ""100141791"",
        ""gradeLevelListDisplayText"": ""12th      "",
        ""gradeLevelSortOrder"": 12,
        ""language"": null,
        ""parentMilitary"": null,
        ""placeOfBirth"": null,
        ""schoolCategory"": ""Ungraded"",
        ""schoolId"": 867530011,
        ""schoolName"": ""Cooper"",
        ""studentIndicators"": [
          {
            ""displayOrder"": null,
            ""name"": ""At Risk"",
            ""status"": true,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Economically Disadvantaged"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Free or Reduced Priced Lunch Eligible"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Homeless"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Immigrant"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 1"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 2"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Migrant"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Over Age"",
            ""status"": true,
            ""studentUsi"": ""100141791"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": 0,
            ""name"": ""Bilingual"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 1,
            ""name"": ""Bilingual Summer"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 2,
            ""name"": ""Career and Technical Education"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 3,
            ""name"": ""English as a Second Language (ESL)"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 4,
            ""name"": ""Gifted and Talented"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 5,
            ""name"": ""Section 504 Placement"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 6,
            ""name"": ""Special Education"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 7,
            ""name"": ""Title I Part A"",
            ""status"": false,
            ""studentUsi"": ""100141791"",
            ""type"": ""Program""
          }
        ],
        ""studentParentInformation"": [
          {
            ""addressLine1"": ""353 Montview Cir"",
            ""addressLine2"": null,
            ""emailAddress"": ""maeve.wheeler@tsds.org"",
            ""fullName"": ""Maeve Wheeler"",
            ""primaryContact"": true,
            ""relation"": ""Other"",
            ""studentUsi"": ""100141791"",
            ""telephoneNumber"": ""(281)-836-5086""
          }
        ],
        ""studentSchoolInformation"": [
          {
            ""dateOfEntry"": ""2012-03-19"",
            ""dateOfWithdrawal"": null,
            ""expectedGraduationYear"": ""2012"",
            ""gradeLevel"": ""Twelfth grade"",
            ""graduationPlan"": ""Career and Technical"",
            ""homeroom"": """",
            ""lateEnrollment"": ""Yes"",
            ""schoolId"": 867530011,
            ""studentUsi"": ""100141791""
          }
        ],
        ""metrics"": [
          {
            ""name"": ""Attendance and Discipline"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Attendance"",
            ""parentId"": 66,
            ""parentName"": ""Attendance and Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Daily Attendance Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          },
          {
            ""name"": ""Days Absent"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Unexcused Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year Unexcused Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Tardy Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Class Period Absence Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Discipline"",
            ""parentId"": 66,
            ""parentName"": ""Attendance and Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Offenses"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year State Offenses"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Non-State Offenses"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year Non-State Offenses"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Discipline Referrals"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Reportable Offenses"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year State Reportable Offenses"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""School Code of Conduct"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year School Code of Conduct"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Assessments"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Standardized Assessments"",
            ""parentId"": 67,
            ""parentName"": ""State Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Assessment Performance"",
            ""parentId"": 68,
            ""parentName"": ""State Standardized Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""ELA / Reading"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Science"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessments"",
            ""parentId"": 67,
            ""parentName"": ""State Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessment Proficiency"",
            ""parentId"": 1004,
            ""parentName"": ""Language Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessment"",
            ""parentId"": 1,
            ""parentName"": ""Language Assessment Proficiency"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Local Assessments"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Benchmark Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Benchmark Assessment Mastery"",
            ""parentId"": 100,
            ""parentName"": ""Benchmark Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""ELA / Reading"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Writing"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Science"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Learning Standard Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Learning Standard Mastery By Core Subject Area"",
            ""parentId"": 100000,
            ""parentName"": ""Learning Standard Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""ELA/Reading"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Science"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessment"",
            ""parentId"": 1080,
            ""parentName"": ""Reading Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessment"",
            ""parentId"": 1259,
            ""parentName"": ""Reading Assessment"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Reading Level"",
            ""parentId"": 1259,
            ""parentName"": ""Reading Assessment"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Grades and Credits"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Subject Area Grades"",
            ""parentId"": 93,
            ""parentName"": ""Grades and Credits"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Failing Subject Area Course Grades"",
            ""parentId"": 1086,
            ""parentName"": ""Subject Area Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading/ELA"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Reading/ELA"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Writing"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Writing"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Mathematics"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Science"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Science"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Social Studies"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          }
        ]
      },
      {
        ""fullName"": ""Chad Patton"",
        ""firstName"": ""Chad"",
        ""addressLine1"": ""491 Fairview Cir"",
        ""city"": ""Taft"",
        ""currentAge"": 19,
        ""dateOfBirth"": ""0001-01-01"",
        ""gender"": ""Male"",
        ""gradeLevel"": ""Twelfth grade"",
        ""hispanicLatinoEthnicity"": ""No"",
        ""homeLanguage"": null,
        ""lateEnrollment"": ""Yes"",
        ""middleName"": null,
        ""race"": ""White"",
        ""studentUsi"": ""100140764"",
        ""gradeLevelListDisplayText"": ""12th      "",
        ""gradeLevelSortOrder"": 12,
        ""language"": null,
        ""parentMilitary"": null,
        ""placeOfBirth"": null,
        ""schoolCategory"": ""Ungraded"",
        ""schoolId"": 867530011,
        ""schoolName"": ""Cooper"",
        ""studentIndicators"": [
          {
            ""displayOrder"": null,
            ""name"": ""At Risk"",
            ""status"": true,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Economically Disadvantaged"",
            ""status"": true,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Free or Reduced Priced Lunch Eligible"",
            ""status"": true,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Homeless"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Immigrant"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 1"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 2"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Migrant"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Over Age"",
            ""status"": true,
            ""studentUsi"": ""100140764"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": 0,
            ""name"": ""Bilingual"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 1,
            ""name"": ""Bilingual Summer"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 2,
            ""name"": ""Career and Technical Education"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 3,
            ""name"": ""English as a Second Language (ESL)"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 4,
            ""name"": ""Gifted and Talented"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 5,
            ""name"": ""Section 504 Placement"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 6,
            ""name"": ""Special Education"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 7,
            ""name"": ""Title I Part A"",
            ""status"": false,
            ""studentUsi"": ""100140764"",
            ""type"": ""Program""
          }
        ],
        ""studentParentInformation"": [
          {
            ""addressLine1"": ""491 Fairview Cir"",
            ""addressLine2"": null,
            ""emailAddress"": null,
            ""fullName"": ""Zion Hickman"",
            ""primaryContact"": true,
            ""relation"": ""Other"",
            ""studentUsi"": ""100140764"",
            ""telephoneNumber"": ""(927)-897-6988""
          }
        ],
        ""studentSchoolInformation"": [
          {
            ""dateOfEntry"": ""2011-09-27"",
            ""dateOfWithdrawal"": null,
            ""expectedGraduationYear"": ""2012"",
            ""gradeLevel"": ""Twelfth grade"",
            ""graduationPlan"": ""Career and Technical"",
            ""homeroom"": ""Angelica D. Thompson, Aaron Q. Patton"",
            ""lateEnrollment"": ""Yes"",
            ""schoolId"": 867530011,
            ""studentUsi"": ""100140764""
          }
        ],
        ""metrics"": [
          {
            ""name"": ""Attendance and Discipline"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Attendance"",
            ""parentId"": 66,
            ""parentName"": ""Attendance and Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Daily Attendance Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": 0,
            ""state"": ""Bad"",
            ""value"": ""70.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": 0,
            ""state"": ""Bad"",
            ""value"": ""72.5%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""60.7%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""60.7%""
          },
          {
            ""name"": ""Days Absent"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""59""
          },
          {
            ""name"": ""Prior Year Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""59""
          },
          {
            ""name"": ""Unexcused Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""56""
          },
          {
            ""name"": ""Prior Year Unexcused Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""56""
          },
          {
            ""name"": ""Tardy Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""2.5%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""5.3%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""5.3%""
          },
          {
            ""name"": ""Class Period Absence Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": 1,
            ""state"": ""Bad"",
            ""value"": ""28.3%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": -1,
            ""state"": ""Bad"",
            ""value"": ""25.4%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""34.8%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""34.8%""
          },
          {
            ""name"": ""Discipline"",
            ""parentId"": 66,
            ""parentName"": ""Attendance and Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Offenses"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year State Offenses"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Non-State Offenses"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year Non-State Offenses"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Discipline Referrals"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Reportable Offenses"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year State Reportable Offenses"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""School Code of Conduct"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year School Code of Conduct"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Assessments"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Standardized Assessments"",
            ""parentId"": 67,
            ""parentName"": ""State Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Assessment Performance"",
            ""parentId"": 68,
            ""parentName"": ""State Standardized Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""ELA / Reading"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Science"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessments"",
            ""parentId"": 67,
            ""parentName"": ""State Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessment Proficiency"",
            ""parentId"": 1004,
            ""parentName"": ""Language Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessment"",
            ""parentId"": 1,
            ""parentName"": ""Language Assessment Proficiency"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Local Assessments"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Benchmark Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Benchmark Assessment Mastery"",
            ""parentId"": 100,
            ""parentName"": ""Benchmark Assessments"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""ELA / Reading"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Writing"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""25%""
          },
          {
            ""name"": ""Science"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""26%""
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Learning Standard Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Learning Standard Mastery By Core Subject Area"",
            ""parentId"": 100000,
            ""parentName"": ""Learning Standard Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""ELA/Reading"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Science"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessment"",
            ""parentId"": 1080,
            ""parentName"": ""Reading Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessment"",
            ""parentId"": 1259,
            ""parentName"": ""Reading Assessment"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Reading Level"",
            ""parentId"": 1259,
            ""parentName"": ""Reading Assessment"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Grades and Credits"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Subject Area Grades"",
            ""parentId"": 93,
            ""parentName"": ""Grades and Credits"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Failing Subject Area Course Grades"",
            ""parentId"": 1086,
            ""parentName"": ""Subject Area Grades"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Reading/ELA"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Reading/ELA"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Writing"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Writing"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": 0,
            ""state"": ""Bad"",
            ""value"": ""1/1""
          },
          {
            ""name"": ""Prior Year Mathematics"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": 0,
            ""state"": ""Bad"",
            ""value"": ""1/1""
          },
          {
            ""name"": ""Science"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": 1,
            ""state"": ""Bad"",
            ""value"": ""1/1""
          },
          {
            ""name"": ""Prior Year Science"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": 1,
            ""state"": ""Bad"",
            ""value"": ""1/1""
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Social Studies"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          }
        ]
      },
      {
        ""fullName"": ""Jason Siebold"",
        ""firstName"": ""Jason"",
        ""addressLine1"": ""427 Kiowa Ct"",
        ""city"": ""Huntsville"",
        ""currentAge"": 17,
        ""dateOfBirth"": ""0001-01-01"",
        ""gender"": ""Male"",
        ""gradeLevel"": ""Twelfth grade"",
        ""hispanicLatinoEthnicity"": ""Yes"",
        ""homeLanguage"": ""English"",
        ""lateEnrollment"": ""Yes"",
        ""middleName"": null,
        ""race"": ""White"",
        ""studentUsi"": ""100067117"",
        ""gradeLevelListDisplayText"": ""12th      "",
        ""gradeLevelSortOrder"": 12,
        ""language"": null,
        ""parentMilitary"": null,
        ""placeOfBirth"": null,
        ""schoolCategory"": ""Ungraded"",
        ""schoolId"": 867530011,
        ""schoolName"": ""Cooper"",
        ""studentIndicators"": [
          {
            ""displayOrder"": null,
            ""name"": ""At Risk"",
            ""status"": true,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Economically Disadvantaged"",
            ""status"": true,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Free or Reduced Priced Lunch Eligible"",
            ""status"": true,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Homeless"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Immigrant"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 1"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 2"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Migrant"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Over Age"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": 0,
            ""name"": ""Bilingual"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 1,
            ""name"": ""Bilingual Summer"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 2,
            ""name"": ""Career and Technical Education"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 3,
            ""name"": ""English as a Second Language (ESL)"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 4,
            ""name"": ""Gifted and Talented"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 5,
            ""name"": ""Section 504 Placement"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 6,
            ""name"": ""Special Education"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 7,
            ""name"": ""Title I Part A"",
            ""status"": false,
            ""studentUsi"": ""100067117"",
            ""type"": ""Program""
          }
        ],
        ""studentParentInformation"": [
          {
            ""addressLine1"": ""427 Kiowa Ct"",
            ""addressLine2"": null,
            ""emailAddress"": null,
            ""fullName"": ""Kade Rodas"",
            ""primaryContact"": true,
            ""relation"": ""Mother"",
            ""studentUsi"": ""100067117"",
            ""telephoneNumber"": ""(708)-863-8898""
          }
        ],
        ""studentSchoolInformation"": [
          {
            ""dateOfEntry"": ""2011-09-09"",
            ""dateOfWithdrawal"": null,
            ""expectedGraduationYear"": ""2012"",
            ""gradeLevel"": ""Twelfth grade"",
            ""graduationPlan"": ""Career and Technical"",
            ""homeroom"": ""Angelica D. Thompson, Rachel H. Wentz"",
            ""lateEnrollment"": ""Yes"",
            ""schoolId"": 867530011,
            ""studentUsi"": ""100067117""
          }
        ],
        ""metrics"": [
          {
            ""name"": ""Attendance and Discipline"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Attendance"",
            ""parentId"": 66,
            ""parentName"": ""Attendance and Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Daily Attendance Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": 1,
            ""state"": ""Bad"",
            ""value"": ""85.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": -1,
            ""state"": ""Bad"",
            ""value"": ""72.5%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""80.2%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""80.2%""
          },
          {
            ""name"": ""Days Absent"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""32""
          },
          {
            ""name"": ""Prior Year Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""32""
          },
          {
            ""name"": ""Unexcused Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""10""
          },
          {
            ""name"": ""Prior Year Unexcused Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""10""
          },
          {
            ""name"": ""Tardy Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Class Period Absence Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": -1,
            ""state"": ""Bad"",
            ""value"": ""13.1%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": 0,
            ""state"": ""Bad"",
            ""value"": ""26.9%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""22.9%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""22.9%""
          },
          {
            ""name"": ""Discipline"",
            ""parentId"": 66,
            ""parentName"": ""Attendance and Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Offenses"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year State Offenses"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Non-State Offenses"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year Non-State Offenses"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Discipline Referrals"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Reportable Offenses"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year State Reportable Offenses"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""School Code of Conduct"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year School Code of Conduct"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Assessments"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Standardized Assessments"",
            ""parentId"": 67,
            ""parentName"": ""State Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Assessment Performance"",
            ""parentId"": 68,
            ""parentName"": ""State Standardized Assessments"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""ELA / Reading"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""22""
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""23""
          },
          {
            ""name"": ""Science"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""19""
          },
          {
            ""name"": ""Language Assessments"",
            ""parentId"": 67,
            ""parentName"": ""State Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessment Proficiency"",
            ""parentId"": 1004,
            ""parentName"": ""Language Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessment"",
            ""parentId"": 1,
            ""parentName"": ""Language Assessment Proficiency"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Local Assessments"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Benchmark Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Benchmark Assessment Mastery"",
            ""parentId"": 100,
            ""parentName"": ""Benchmark Assessments"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""ELA / Reading"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Writing"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""15%""
          },
          {
            ""name"": ""Science"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""46%""
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""72%""
          },
          {
            ""name"": ""Learning Standard Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Learning Standard Mastery By Core Subject Area"",
            ""parentId"": 100000,
            ""parentName"": ""Learning Standard Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""ELA/Reading"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Science"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessment"",
            ""parentId"": 1080,
            ""parentName"": ""Reading Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessment"",
            ""parentId"": 1259,
            ""parentName"": ""Reading Assessment"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Reading Level"",
            ""parentId"": 1259,
            ""parentName"": ""Reading Assessment"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Grades and Credits"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Subject Area Grades"",
            ""parentId"": 93,
            ""parentName"": ""Grades and Credits"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Failing Subject Area Course Grades"",
            ""parentId"": 1086,
            ""parentName"": ""Subject Area Grades"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Reading/ELA"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Reading/ELA"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Writing"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Writing"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0/2""
          },
          {
            ""name"": ""Prior Year Mathematics"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0/2""
          },
          {
            ""name"": ""Science"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Science"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0/1""
          },
          {
            ""name"": ""Prior Year Social Studies"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0/1""
          }
        ]
      },
      {
        ""fullName"": ""Charlie B. Mosley"",
        ""firstName"": ""Charlie"",
        ""addressLine1"": ""384 Green Loop Rd"",
        ""city"": ""Gun Barrel City"",
        ""currentAge"": 16,
        ""dateOfBirth"": ""0001-01-01"",
        ""gender"": ""Male"",
        ""gradeLevel"": ""Twelfth grade"",
        ""hispanicLatinoEthnicity"": ""No"",
        ""homeLanguage"": ""English"",
        ""lateEnrollment"": ""Yes"",
        ""middleName"": ""B"",
        ""race"": ""Black - African American"",
        ""studentUsi"": ""100067128"",
        ""gradeLevelListDisplayText"": ""12th      "",
        ""gradeLevelSortOrder"": 12,
        ""language"": null,
        ""parentMilitary"": null,
        ""placeOfBirth"": null,
        ""schoolCategory"": ""Ungraded"",
        ""schoolId"": 867530011,
        ""schoolName"": ""Cooper"",
        ""studentIndicators"": [
          {
            ""displayOrder"": null,
            ""name"": ""At Risk"",
            ""status"": true,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Economically Disadvantaged"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Free or Reduced Priced Lunch Eligible"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Homeless"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Immigrant"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 1"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 2"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Migrant"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Over Age"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": 0,
            ""name"": ""Bilingual"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 1,
            ""name"": ""Bilingual Summer"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 2,
            ""name"": ""Career and Technical Education"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 3,
            ""name"": ""English as a Second Language (ESL)"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 4,
            ""name"": ""Gifted and Talented"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 5,
            ""name"": ""Section 504 Placement"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 6,
            ""name"": ""Special Education"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 7,
            ""name"": ""Title I Part A"",
            ""status"": false,
            ""studentUsi"": ""100067128"",
            ""type"": ""Program""
          }
        ],
        ""studentParentInformation"": [
          {
            ""addressLine1"": ""384 Green Loop Rd"",
            ""addressLine2"": null,
            ""emailAddress"": ""rory.becks@tsds.org"",
            ""fullName"": ""Rory Becks"",
            ""primaryContact"": true,
            ""relation"": ""Mother"",
            ""studentUsi"": ""100067128"",
            ""telephoneNumber"": ""(930)-948-4193""
          }
        ],
        ""studentSchoolInformation"": [
          {
            ""dateOfEntry"": ""2012-05-18"",
            ""dateOfWithdrawal"": null,
            ""expectedGraduationYear"": ""2012"",
            ""gradeLevel"": ""Twelfth grade"",
            ""graduationPlan"": ""Career and Technical"",
            ""homeroom"": ""Angelica D. Thompson, Thomas N. Moore"",
            ""lateEnrollment"": ""Yes"",
            ""schoolId"": 867530011,
            ""studentUsi"": ""100067128""
          }
        ],
        ""metrics"": [
          {
            ""name"": ""Attendance and Discipline"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Attendance"",
            ""parentId"": 66,
            ""parentName"": ""Attendance and Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Daily Attendance Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          },
          {
            ""name"": ""Days Absent"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Unexcused Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year Unexcused Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Tardy Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""33.3%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""33.3%""
          },
          {
            ""name"": ""Class Period Absence Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Discipline"",
            ""parentId"": 66,
            ""parentName"": ""Attendance and Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Offenses"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year State Offenses"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Non-State Offenses"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year Non-State Offenses"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Discipline Referrals"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Reportable Offenses"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year State Reportable Offenses"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""School Code of Conduct"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year School Code of Conduct"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Assessments"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Standardized Assessments"",
            ""parentId"": 67,
            ""parentName"": ""State Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Assessment Performance"",
            ""parentId"": 68,
            ""parentName"": ""State Standardized Assessments"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""ELA / Reading"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""44""
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Science"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""33""
          },
          {
            ""name"": ""Language Assessments"",
            ""parentId"": 67,
            ""parentName"": ""State Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessment Proficiency"",
            ""parentId"": 1004,
            ""parentName"": ""Language Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessment"",
            ""parentId"": 1,
            ""parentName"": ""Language Assessment Proficiency"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Local Assessments"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Benchmark Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Benchmark Assessment Mastery"",
            ""parentId"": 100,
            ""parentName"": ""Benchmark Assessments"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""ELA / Reading"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Writing"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""40%""
          },
          {
            ""name"": ""Science"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""64%""
          },
          {
            ""name"": ""Learning Standard Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Learning Standard Mastery By Core Subject Area"",
            ""parentId"": 100000,
            ""parentName"": ""Learning Standard Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""ELA/Reading"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Science"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessment"",
            ""parentId"": 1080,
            ""parentName"": ""Reading Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessment"",
            ""parentId"": 1259,
            ""parentName"": ""Reading Assessment"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Reading Level"",
            ""parentId"": 1259,
            ""parentName"": ""Reading Assessment"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Grades and Credits"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Subject Area Grades"",
            ""parentId"": 93,
            ""parentName"": ""Grades and Credits"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Failing Subject Area Course Grades"",
            ""parentId"": 1086,
            ""parentName"": ""Subject Area Grades"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Reading/ELA"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Reading/ELA"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Writing"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Writing"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0/1""
          },
          {
            ""name"": ""Prior Year Mathematics"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0/1""
          },
          {
            ""name"": ""Science"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Science"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0/1""
          },
          {
            ""name"": ""Prior Year Social Studies"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0/1""
          }
        ]
      },
      {
        ""fullName"": ""Damiyun L. Dudley"",
        ""firstName"": ""Damiyun"",
        ""addressLine1"": ""610 Dodson Rd"",
        ""city"": ""Balcones Heights"",
        ""currentAge"": 17,
        ""dateOfBirth"": ""0001-01-01"",
        ""gender"": ""Male"",
        ""gradeLevel"": ""Twelfth grade"",
        ""hispanicLatinoEthnicity"": ""Yes"",
        ""homeLanguage"": ""English"",
        ""lateEnrollment"": ""Yes"",
        ""middleName"": ""L"",
        ""race"": ""White"",
        ""studentUsi"": ""100067689"",
        ""gradeLevelListDisplayText"": ""12th      "",
        ""gradeLevelSortOrder"": 12,
        ""language"": null,
        ""parentMilitary"": null,
        ""placeOfBirth"": null,
        ""schoolCategory"": ""Ungraded"",
        ""schoolId"": 867530011,
        ""schoolName"": ""Cooper"",
        ""studentIndicators"": [
          {
            ""displayOrder"": null,
            ""name"": ""At Risk"",
            ""status"": true,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Economically Disadvantaged"",
            ""status"": true,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Free or Reduced Priced Lunch Eligible"",
            ""status"": true,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Homeless"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Immigrant"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 1"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Limited English Proficiency Monitored 2"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Migrant"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": null,
            ""name"": ""Over Age"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Other""
          },
          {
            ""displayOrder"": 0,
            ""name"": ""Bilingual"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 1,
            ""name"": ""Bilingual Summer"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 2,
            ""name"": ""Career and Technical Education"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 3,
            ""name"": ""English as a Second Language (ESL)"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 4,
            ""name"": ""Gifted and Talented"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 5,
            ""name"": ""Section 504 Placement"",
            ""status"": true,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 6,
            ""name"": ""Special Education"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          },
          {
            ""displayOrder"": 7,
            ""name"": ""Title I Part A"",
            ""status"": false,
            ""studentUsi"": ""100067689"",
            ""type"": ""Program""
          }
        ],
        ""studentParentInformation"": [
          {
            ""addressLine1"": ""610 Dodson Rd"",
            ""addressLine2"": null,
            ""emailAddress"": ""parker.woody@tsds.org"",
            ""fullName"": ""Parker Woody"",
            ""primaryContact"": true,
            ""relation"": ""Mother"",
            ""studentUsi"": ""100067689"",
            ""telephoneNumber"": ""(215)-732-1037""
          }
        ],
        ""studentSchoolInformation"": [
          {
            ""dateOfEntry"": ""2012-05-18"",
            ""dateOfWithdrawal"": null,
            ""expectedGraduationYear"": ""2012"",
            ""gradeLevel"": ""Twelfth grade"",
            ""graduationPlan"": ""Career and Technical"",
            ""homeroom"": ""Ada S. Canty"",
            ""lateEnrollment"": ""Yes"",
            ""schoolId"": 867530011,
            ""studentUsi"": ""100067689""
          }
        ],
        ""metrics"": [
          {
            ""name"": ""Attendance and Discipline"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Attendance"",
            ""parentId"": 66,
            ""parentName"": ""Attendance and Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Daily Attendance Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 2,
            ""parentName"": ""Daily Attendance Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""100.0%""
          },
          {
            ""name"": ""Days Absent"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Unexcused Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year Unexcused Days Absent"",
            ""parentId"": 1670,
            ""parentName"": ""Days Absent"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Tardy Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 1000,
            ""parentName"": ""Tardy Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Class Period Absence Rate"",
            ""parentId"": 98,
            ""parentName"": ""Attendance"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Prior Year"",
            ""parentId"": 6,
            ""parentName"": ""Class Period Absence Rate"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0.0%""
          },
          {
            ""name"": ""Discipline"",
            ""parentId"": 66,
            ""parentName"": ""Attendance and Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Offenses"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year State Offenses"",
            ""parentId"": 105,
            ""parentName"": ""State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Non-State Offenses"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Last 20 Days"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Last 40 Days"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": 0,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Year to Date"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Prior Year Non-State Offenses"",
            ""parentId"": 111,
            ""parentName"": ""Non-State Offenses"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0""
          },
          {
            ""name"": ""Discipline Referrals"",
            ""parentId"": 99,
            ""parentName"": ""Discipline"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Reportable Offenses"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year State Reportable Offenses"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""School Code of Conduct"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year School Code of Conduct"",
            ""parentId"": 10,
            ""parentName"": ""Discipline Referrals"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Assessments"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Standardized Assessments"",
            ""parentId"": 67,
            ""parentName"": ""State Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""State Assessment Performance"",
            ""parentId"": 68,
            ""parentName"": ""State Standardized Assessments"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""ELA / Reading"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""27""
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""43""
          },
          {
            ""name"": ""Science"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""35""
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 15,
            ""parentName"": ""State Assessment Performance"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""32""
          },
          {
            ""name"": ""Language Assessments"",
            ""parentId"": 67,
            ""parentName"": ""State Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessment Proficiency"",
            ""parentId"": 1004,
            ""parentName"": ""Language Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Language Assessment"",
            ""parentId"": 1,
            ""parentName"": ""Language Assessment Proficiency"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Local Assessments"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Benchmark Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Benchmark Assessment Mastery"",
            ""parentId"": 100,
            ""parentName"": ""Benchmark Assessments"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""ELA / Reading"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Writing"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""50%""
          },
          {
            ""name"": ""Science"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""26%""
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 73,
            ""parentName"": ""Benchmark Assessment Mastery"",
            ""trendDirection"": null,
            ""state"": ""Bad"",
            ""value"": ""68%""
          },
          {
            ""name"": ""Learning Standard Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Learning Standard Mastery By Core Subject Area"",
            ""parentId"": 100000,
            ""parentName"": ""Learning Standard Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""ELA/Reading"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Science"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 1230,
            ""parentName"": ""Learning Standard Mastery By Core Subject Area"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessments"",
            ""parentId"": 1490,
            ""parentName"": ""Local Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessment"",
            ""parentId"": 1080,
            ""parentName"": ""Reading Assessments"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Reading Assessment"",
            ""parentId"": 1259,
            ""parentName"": ""Reading Assessment"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Reading Level"",
            ""parentId"": 1259,
            ""parentName"": ""Reading Assessment"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Grades and Credits"",
            ""parentId"": 65,
            ""parentName"": ""Overview"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Subject Area Grades"",
            ""parentId"": 93,
            ""parentName"": ""Grades and Credits"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Failing Subject Area Course Grades"",
            ""parentId"": 1086,
            ""parentName"": ""Subject Area Grades"",
            ""trendDirection"": null,
            ""state"": """",
            ""value"": null
          },
          {
            ""name"": ""Reading/ELA"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Reading/ELA"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Writing"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Writing"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Mathematics"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0/1""
          },
          {
            ""name"": ""Prior Year Mathematics"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0/1""
          },
          {
            ""name"": ""Science"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0/2""
          },
          {
            ""name"": ""Prior Year Science"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": ""Good"",
            ""value"": ""0/2""
          },
          {
            ""name"": ""Social Studies"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          },
          {
            ""name"": ""Prior Year Social Studies"",
            ""parentId"": 1491,
            ""parentName"": ""Failing Subject Area Course Grades"",
            ""trendDirection"": null,
            ""state"": null,
            ""value"": null
          }
        ]
      }
    ]
  }
}";
            Assert.True(JToken.DeepEquals(JObject.Parse(expectedValue), JObject.Parse(json)));
        }

        [Fact]
        public async Task TestMetricMetadataAll()
        {
            var json = await Fixture.QueryGraphQLAsync(@"query{
  metricMetadata{
    description displayName domainEntityType displayOrder metricId name url
    children{
      description displayName domainEntityType displayOrder metricId name url
      children{
        description displayName domainEntityType displayOrder metricId name url
        children{
          description displayName domainEntityType displayOrder metricId name url
          children{
            description displayName domainEntityType displayOrder metricId name url
            children{
              description displayName domainEntityType displayOrder metricId name url
              children{
                description displayName domainEntityType displayOrder metricId name url
              }
            }
          }
        }
      }
    }
  }
}");
            var expectedValue = @"{
  ""data"": {
    ""metricMetadata"": [
      {
        ""description"": """",
        ""displayName"": ""Elementary School"",
        ""domainEntityType"": """",
        ""displayOrder"": 10,
        ""metricId"": 1040,
        ""name"": ""Elementary School"",
        ""url"": """",
        ""children"": [
          {
            ""description"": ""STUDENT OVERVIEW"",
            ""displayName"": ""Overview"",
            ""domainEntityType"": ""StudentSchool"",
            ""displayOrder"": 10,
            ""metricId"": 65,
            ""name"": ""Overview"",
            ""url"": ""~/Application/Student/Overview.aspx"",
            ""children"": [
              {
                ""description"": ""Student attendance and discipline patterns"",
                ""displayName"": ""Attendance and Discipline"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 10,
                ""metricId"": 66,
                ""name"": ""Attendance and Discipline"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Daily and class period attendance"",
                    ""displayName"": ""Attendance"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 98,
                    ""metricId"": 98,
                    ""name"": ""Attendance"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of days student is in attendance"",
                        ""displayName"": ""Daily Attendance Rate"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 2,
                        ""name"": ""Daily Attendance Rate"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 3,
                            ""metricId"": 3,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 4,
                            ""metricId"": 4,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 5,
                            ""metricId"": 5,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 5,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""# of days absent year to date"",
                        ""displayName"": ""Days Absent"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 40,
                        ""metricId"": 1670,
                        ""name"": ""Days Absent"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": ""# of days absent year to date"",
                            ""displayName"": ""Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1483,
                            ""name"": ""Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""# of days absent year to date"",
                            ""displayName"": ""Prior Year Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1483,
                            ""name"": ""Prior Year Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Unexcused Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1671,
                            ""name"": ""Unexcused Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Unexcused Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 35,
                            ""metricId"": 1671,
                            ""name"": ""Prior Year Unexcused Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of tardies in a given period"",
                        ""displayName"": ""Tardy Rate"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 50,
                        ""metricId"": 1000,
                        ""name"": ""Tardy Rate"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1077,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1078,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1079,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 35,
                            ""metricId"": 1079,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of total class periods missed"",
                        ""displayName"": ""Class Period Absence Rate"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 60,
                        ""metricId"": 6,
                        ""name"": ""Class Period Absence Rate"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 7,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 8,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 9,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 35,
                            ""metricId"": 9,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Log of discipline incidents and actions year to date"",
                    ""displayName"": ""Discipline"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 99,
                    ""metricId"": 99,
                    ""name"": ""Discipline"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""# of state reportable offenses in a given period"",
                        ""displayName"": ""State Offenses"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 105,
                        ""name"": ""State Offenses"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 106,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 107,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 108,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year State Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 108,
                            ""name"": ""Prior Year State Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""# of Non-state reportable offenses in a given period"",
                        ""displayName"": ""Non-State Offenses"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 111,
                        ""name"": ""Non-State Offenses"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 112,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 113,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 114,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Non-State Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 114,
                            ""name"": ""Prior Year Non-State Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""Log of discipline incidents and actions year-to-date"",
                        ""displayName"": ""Discipline Referrals"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 40,
                        ""metricId"": 10,
                        ""name"": ""Discipline Referrals"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""State Reportable Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 78,
                            ""name"": ""State Reportable Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year State Reportable Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 78,
                            ""name"": ""Prior Year State Reportable Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""School Code of Conduct"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 79,
                            ""name"": ""School Code of Conduct"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year School Code of Conduct"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 79,
                            ""name"": ""Prior Year School Code of Conduct"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""State examinations and assessments"",
                ""displayName"": ""State Assessments"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 20,
                ""metricId"": 67,
                ""name"": ""State Assessments"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress on state standardized test"",
                    ""displayName"": ""State Standardized Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 20,
                    ""metricId"": 68,
                    ""name"": ""State Standardized Assessments"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Test scores and whether met standard"",
                        ""displayName"": ""State Assessment Performance"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 40,
                        ""metricId"": 15,
                        ""name"": ""State Assessment Performance"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 16,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 17,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 19,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 50,
                            ""metricId"": 20,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""English Language Learner"",
                    ""displayName"": ""Language Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 30,
                    ""metricId"": 1004,
                    ""name"": ""Language Assessments"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Test scores and proficiency level"",
                        ""displayName"": ""Language Assessment Proficiency"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 1,
                        ""name"": ""Language Assessment Proficiency"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Language Assessment"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1008,
                            ""name"": ""Language Assessment"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Local examinations and assessments"",
                ""displayName"": ""Local Assessments"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 30,
                ""metricId"": 1490,
                ""name"": ""Local Assessments"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Benchmark assessments"",
                    ""displayName"": ""Benchmark Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 10,
                    ""metricId"": 100,
                    ""name"": ""Benchmark Assessments"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Mastery of content in core courses"",
                        ""displayName"": ""Benchmark Assessment Mastery"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 73,
                        ""name"": ""Benchmark Assessment Mastery"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 74,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1042,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 75,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 76,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 50,
                            ""metricId"": 80,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Learning Standard Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 20,
                    ""metricId"": 100000,
                    ""name"": ""Learning Standard Assessments"",
                    ""url"": """",
                    ""children"": [
                      {
                        ""description"": ""Mastered 70%+ of Learning Standards (as of the latest administration)"",
                        ""displayName"": ""Learning Standard Mastery By Core Subject Area"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 1230,
                        ""name"": ""Learning Standard Mastery By Core Subject Area"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA/Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1232,
                            ""name"": ""ELA/Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1238,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1240,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1244,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Performance and progress on reading assessments"",
                    ""displayName"": ""Reading Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 30,
                    ""metricId"": 1080,
                    ""name"": ""Reading Assessments"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Performance and progress on reading assessments"",
                        ""displayName"": ""Reading Assessment"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 40,
                        ""metricId"": 1259,
                        ""name"": ""Reading Assessment"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Reading Assessment"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1083,
                            ""name"": ""Reading Assessment"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Reading Level"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 35,
                            ""metricId"": 1083,
                            ""name"": ""Prior Year Reading Level"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Students progression in coursework"",
                ""displayName"": ""Grades and Credits"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 40,
                ""metricId"": 93,
                ""name"": ""Grades and Credits"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress in subject areas"",
                    ""displayName"": ""Subject Area Grades"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 10,
                    ""metricId"": 1086,
                    ""name"": ""Subject Area Grades"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Performance and progress in subject areas"",
                        ""displayName"": ""Failing Subject Area Course Grades"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 1491,
                        ""name"": ""Failing Subject Area Course Grades"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Reading/ELA"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1492,
                            ""name"": ""Reading/ELA"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Reading/ELA"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 15,
                            ""metricId"": 1492,
                            ""name"": ""Prior Year Reading/ELA"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1493,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Writing"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 25,
                            ""metricId"": 1493,
                            ""name"": ""Prior Year Writing"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1494,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 35,
                            ""metricId"": 1494,
                            ""name"": ""Prior Year Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1495,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 45,
                            ""metricId"": 1495,
                            ""name"": ""Prior Year Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 50,
                            ""metricId"": 1496,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 55,
                            ""metricId"": 1496,
                            ""name"": ""Prior Year Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              }
            ]
          },
          {
            ""description"": ""Campus Overview"",
            ""displayName"": ""Overview"",
            ""domainEntityType"": ""School"",
            ""displayOrder"": 40,
            ""metricId"": 200,
            ""name"": ""Overview"",
            ""url"": ""~/Application/School/Overview.aspx"",
            ""children"": [
              {
                ""description"": ""Students attendance and discipline patterns"",
                ""displayName"": ""Attendance and Discipline"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 10,
                ""metricId"": 201,
                ""name"": ""Attendance and Discipline"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Daily and class period attendance"",
                    ""displayName"": ""Attendance"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 210,
                    ""metricId"": 210,
                    ""name"": ""Attendance"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Average daily attendance for all students with membership on the campus (unweighted)"",
                        ""displayName"": ""Average Daily Attendance"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 1444,
                        ""name"": ""Average Daily Attendance"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 0,
                            ""metricId"": 1445,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 1,
                            ""metricId"": 1448,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 2,
                            ""metricId"": 1446,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 3,
                            ""metricId"": 1446,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students meeting attendance rate threshold of 90% during the specified time frame"",
                        ""displayName"": ""Daily Attendance Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 211,
                        ""name"": ""Daily Attendance Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 212,
                            ""metricId"": 212,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 213,
                            ""metricId"": 213,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 214,
                            ""metricId"": 214,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 219,
                            ""metricId"": 214,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 8 or more absences year to date"",
                        ""displayName"": ""Days Absent"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 1672,
                        ""name"": ""Days Absent"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": ""% of students with 8 or more absences year to date"",
                            ""displayName"": ""Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1484,
                            ""name"": ""Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more absences year to date"",
                            ""displayName"": ""Prior Year Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1484,
                            ""name"": ""Prior Year Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more unexcused absences year to date"",
                            ""displayName"": ""Unexcused Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1673,
                            ""name"": ""Unexcused Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more unexcused absences year to date"",
                            ""displayName"": ""Prior Year Unexcused Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 35,
                            ""metricId"": 1673,
                            ""name"": ""Prior Year Unexcused Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% students exceeding tardy rate threshold of 10% during specified time frame"",
                        ""displayName"": ""Tardy Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 60,
                        ""metricId"": 1130,
                        ""name"": ""Tardy Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1132,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1133,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1134,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 35,
                            ""metricId"": 1134,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students exceeding class period absence rate threshold of 10% during the specified timeframe"",
                        ""displayName"": ""Class Period Absence Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 70,
                        ""metricId"": 216,
                        ""name"": ""Class Period Absence Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 217,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 218,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 219,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 35,
                            ""metricId"": 219,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Discipline incidents and actions"",
                    ""displayName"": ""Discipline"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 416,
                    ""metricId"": 416,
                    ""name"": ""Discipline"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with 1 or more incidents in a given grading period"",
                        ""displayName"": ""State Offenses"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 2350,
                        ""name"": ""State Offenses"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 2351,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 2352,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 2353,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year State Offenses"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 2353,
                            ""name"": ""Prior Year State Offenses"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1 or more incidents in a given grading period"",
                        ""displayName"": ""Non-State Offenses"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 2354,
                        ""name"": ""Non-State Offenses"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 2355,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 2356,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 2357,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Non-State Offenses"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 2357,
                            ""name"": ""Prior Year Non-State Offenses"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1+ state reportable offenses and/or excessive (5+) school code of conduct incidents in a given grading period"",
                        ""displayName"": ""All Discipline Incidents"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 221,
                        ""name"": ""All Discipline Incidents"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Current Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 222,
                            ""metricId"": 222,
                            ""name"": ""Current Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Previous Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 223,
                            ""metricId"": 223,
                            ""name"": ""Previous Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 224,
                            ""metricId"": 224,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 229,
                            ""metricId"": 224,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1 or more incidents in a given grading period"",
                        ""displayName"": ""School Code of Conduct Incidents"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 50,
                        ""metricId"": 226,
                        ""name"": ""School Code of Conduct Incidents"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Current Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 227,
                            ""metricId"": 227,
                            ""name"": ""Current Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Previous Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 228,
                            ""metricId"": 228,
                            ""name"": ""Previous Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 229,
                            ""metricId"": 229,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 234,
                            ""metricId"": 229,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""State examinations and assessments"",
                ""displayName"": ""State Assessments"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 20,
                ""metricId"": 203,
                ""name"": ""State Assessments"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress on state standardized test"",
                    ""displayName"": ""State Standardized Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 1013,
                    ""name"": ""State Standardized Assessments"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students meeting standard"",
                        ""displayName"": ""State Assessment"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 60,
                        ""metricId"": 233,
                        ""name"": ""State Assessment"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 234,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 235,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 236,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 41,
                            ""metricId"": 237,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""All tests"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 42,
                            ""metricId"": 238,
                            ""name"": ""All tests"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with advanced performance"",
                        ""displayName"": ""State Assessment Advanced Performance"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 70,
                        ""metricId"": 1017,
                        ""name"": ""State Assessment Advanced Performance"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1018,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1019,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 1020,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 41,
                            ""metricId"": 1021,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students failing state assessment 2 consecutive administrations "",
                        ""displayName"": ""Repeat State Assessment Failures"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 80,
                        ""metricId"": 244,
                        ""name"": ""Repeat State Assessment Failures"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of eligible students who did not take test"",
                        ""displayName"": ""State Assessment Non-Participation"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 90,
                        ""metricId"": 240,
                        ""name"": ""State Assessment Non-Participation"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 410,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 411,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 412,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 41,
                            ""metricId"": 413,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Performance and progress on language assessments"",
                    ""displayName"": ""Language Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 20,
                    ""metricId"": 1001,
                    ""name"": ""Language Assessments"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students at or above the advanced proficiency level"",
                        ""displayName"": ""Language Assessment Proficiency"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 1002,
                        ""name"": ""Language Assessment Proficiency"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": ""Beginning Proficiency Level Rate"",
                            ""displayName"": ""Language Assessment"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 0,
                            ""metricId"": 1154,
                            ""name"": ""Language Assessment"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Local examinations and assessments"",
                ""displayName"": ""Local Assessments"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 30,
                ""metricId"": 1489,
                ""name"": ""Local Assessments"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress on local benchmark assessments"",
                    ""displayName"": ""Benchmark Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 1014,
                    ""name"": ""Benchmark Assessments"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students scoring at or above the campus threshold of 70% (as of the latest administration)"",
                        ""displayName"": ""Benchmark Performance"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 250,
                        ""name"": ""Benchmark Performance"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 251,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1052,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 254,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 257,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 1015,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Learning Standard Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 20,
                    ""metricId"": 100001,
                    ""name"": ""Learning Standard Assessments"",
                    ""url"": """",
                    ""children"": [
                      {
                        ""description"": ""Mastered 70%+ of Learning Standards (as of the latest administration)"",
                        ""displayName"": ""Learning Standard Mastery by Core Subject Area"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 1253,
                        ""name"": ""Learning Standard Mastery by Core Subject Area"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1254,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1256,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 1257,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 1258,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Performance and progress on reading assessments"",
                    ""displayName"": ""Reading Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 30,
                    ""metricId"": 1044,
                    ""name"": ""Reading Assessments"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": """",
                        ""displayName"": ""Reading Assessments"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 0,
                        ""metricId"": 1260,
                        ""name"": ""Reading Assessments"",
                        ""url"": """",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Reading Assessment"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 0,
                            ""metricId"": 1486,
                            ""name"": ""Reading Assessment"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Reading Assessment"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 1,
                            ""metricId"": 1486,
                            ""name"": ""Prior Year Reading Assessment"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Students progression in coursework"",
                ""displayName"": ""Grades and Credits"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 40,
                ""metricId"": 202,
                ""name"": ""Grades and Credits"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""% of students with failing grades in one of the four core subject areas during the current year"",
                    ""displayName"": ""Subject Area / Course Grades"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 1061,
                    ""name"": ""Subject Area / Course Grades"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with a failing grade in one or more courses in each subject area."",
                        ""displayName"": ""Failing Subject Area Grades"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 1062,
                        ""name"": ""Failing Subject Area Grades"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1063,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 25,
                            ""metricId"": 1063,
                            ""name"": ""Prior Year ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 1064,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Writing"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 45,
                            ""metricId"": 1064,
                            ""name"": ""Prior Year Writing"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 1065,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 55,
                            ""metricId"": 1065,
                            ""name"": ""Prior Year Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 60,
                            ""metricId"": 1066,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 65,
                            ""metricId"": 1066,
                            ""name"": ""Prior Year Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 70,
                            ""metricId"": 1067,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 75,
                            ""metricId"": 1067,
                            ""name"": ""Prior Year Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              }
            ]
          },
          {
            ""description"": """",
            ""displayName"": ""Operational Dashboard"",
            ""domainEntityType"": ""School"",
            ""displayOrder"": 50,
            ""metricId"": 1029,
            ""name"": ""Operational Dashboard"",
            ""url"": ""~/Application/School/Metrics.aspx"",
            ""children"": [
              {
                ""description"": """",
                ""displayName"": ""Staff"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 10,
                ""metricId"": 1030,
                ""name"": ""Staff"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": """",
                    ""displayName"": ""Teacher Attendance"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 1031,
                    ""name"": ""Teacher Attendance"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers meeting attendance rate threshold of 90%"",
                        ""displayName"": ""Teacher Attendance Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 361,
                        ""name"": ""Teacher Attendance Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1032,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1033,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1034,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 35,
                            ""metricId"": 1034,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Experience, Education, and Certifications"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 20,
                    ""metricId"": 1035,
                    ""name"": ""Experience, Education, and Certifications"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers with >5 years of professional teaching experience"",
                        ""displayName"": ""Teacher Experience"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 369,
                        ""name"": ""Teacher Experience"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""All Subjects"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 370,
                            ""metricId"": 370,
                            ""name"": ""All Subjects"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""ELA/Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 371,
                            ""metricId"": 371,
                            ""name"": ""ELA/Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 372,
                            ""metricId"": 372,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 424,
                            ""metricId"": 424,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 425,
                            ""metricId"": 425,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of teachers with advanced degrees"",
                        ""displayName"": ""Teacher Education"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 1069,
                        ""name"": ""Teacher Education"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Bachelors or Higher"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1070,
                            ""name"": ""Bachelors or Higher"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Masters or Doctorate"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1071,
                            ""name"": ""Masters or Doctorate"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Retention and Staffing"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 40,
                    ""metricId"": 1037,
                    ""name"": ""Retention and Staffing"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers returning from the previous year"",
                        ""displayName"": ""Teacher Retention"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 375,
                        ""name"": ""Teacher Retention"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      }
                    ]
                  }
                ]
              }
            ]
          }
        ]
      },
      {
        ""description"": """",
        ""displayName"": ""Staff"",
        ""domainEntityType"": ""Staff"",
        ""displayOrder"": 10,
        ""metricId"": 2394,
        ""name"": ""Staff"",
        ""url"": """",
        ""children"": [
          {
            ""description"": """",
            ""displayName"": ""Staff Overview"",
            ""domainEntityType"": ""Staff"",
            ""displayOrder"": 20,
            ""metricId"": 2395,
            ""name"": ""Staff Overview"",
            ""url"": """",
            ""children"": [
              {
                ""description"": """",
                ""displayName"": ""Teacher Attendance"",
                ""domainEntityType"": ""Staff"",
                ""displayOrder"": 30,
                ""metricId"": 2396,
                ""name"": ""Teacher Attendance"",
                ""url"": """",
                ""children"": [
                  {
                    ""description"": """",
                    ""displayName"": ""Attendance"",
                    ""domainEntityType"": ""Staff"",
                    ""displayOrder"": 0,
                    ""metricId"": 2397,
                    ""name"": ""Attendance"",
                    ""url"": """",
                    ""children"": [
                      {
                        ""description"": """",
                        ""displayName"": ""Teachers Attendance Threshold"",
                        ""domainEntityType"": ""Staff"",
                        ""displayOrder"": 0,
                        ""metricId"": 2380,
                        ""name"": ""Teachers Attendance Threshold"",
                        ""url"": """",
                        ""children"": []
                      }
                    ]
                  }
                ]
              }
            ]
          }
        ]
      },
      {
        ""description"": """",
        ""displayName"": ""Middle School"",
        ""domainEntityType"": """",
        ""displayOrder"": 20,
        ""metricId"": 1041,
        ""name"": ""Middle School"",
        ""url"": """",
        ""children"": [
          {
            ""description"": ""STUDENT OVERVIEW"",
            ""displayName"": ""Overview"",
            ""domainEntityType"": ""StudentSchool"",
            ""displayOrder"": 10,
            ""metricId"": 65,
            ""name"": ""Overview"",
            ""url"": ""~/Application/Student/Overview.aspx"",
            ""children"": [
              {
                ""description"": ""Student attendance and discipline patterns"",
                ""displayName"": ""Attendance and Discipline"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 10,
                ""metricId"": 66,
                ""name"": ""Attendance and Discipline"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Daily and class period attendance"",
                    ""displayName"": ""Attendance"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 98,
                    ""metricId"": 98,
                    ""name"": ""Attendance"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of days student is in attendance"",
                        ""displayName"": ""Daily Attendance Rate"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 2,
                        ""name"": ""Daily Attendance Rate"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 3,
                            ""metricId"": 3,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 4,
                            ""metricId"": 4,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 5,
                            ""metricId"": 5,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 5,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""# of days absent year to date"",
                        ""displayName"": ""Days Absent"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 1670,
                        ""name"": ""Days Absent"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": ""# of days absent year to date"",
                            ""displayName"": ""Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1483,
                            ""name"": ""Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""# of days absent year to date"",
                            ""displayName"": ""Prior Year Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1483,
                            ""name"": ""Prior Year Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Unexcused Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1671,
                            ""name"": ""Unexcused Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Unexcused Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 35,
                            ""metricId"": 1671,
                            ""name"": ""Prior Year Unexcused Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of total class periods missed"",
                        ""displayName"": ""Class Period Absence Rate"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 30,
                        ""metricId"": 6,
                        ""name"": ""Class Period Absence Rate"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 7,
                            ""metricId"": 7,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 8,
                            ""metricId"": 8,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 9,
                            ""metricId"": 9,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 14,
                            ""metricId"": 9,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of tardies in a given period"",
                        ""displayName"": ""Tardy Rate"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 40,
                        ""metricId"": 1000,
                        ""name"": ""Tardy Rate"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1077,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1078,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1079,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 35,
                            ""metricId"": 1079,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Log of discipline incidents and actions year to date"",
                    ""displayName"": ""Discipline"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 99,
                    ""metricId"": 99,
                    ""name"": ""Discipline"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""# of state reportable offenses in a given period"",
                        ""displayName"": ""State Offenses"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 105,
                        ""name"": ""State Offenses"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 106,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 107,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 108,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year State Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 108,
                            ""name"": ""Prior Year State Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""# of Non-state reportable offenses in a given period"",
                        ""displayName"": ""Non-State Offenses"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 111,
                        ""name"": ""Non-State Offenses"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 112,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 113,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 114,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Non-State Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 114,
                            ""name"": ""Prior Year Non-State Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""Log of discipline incidents and actions year-to-date"",
                        ""displayName"": ""Discipline Referrals"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 40,
                        ""metricId"": 10,
                        ""name"": ""Discipline Referrals"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""State Reportable Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 78,
                            ""name"": ""State Reportable Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year State Reportable Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 78,
                            ""name"": ""Prior Year State Reportable Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""School Code of Conduct"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 79,
                            ""name"": ""School Code of Conduct"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year School Code of Conduct"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 79,
                            ""name"": ""Prior Year School Code of Conduct"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""State examinations and assessments"",
                ""displayName"": ""State Assessments"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 20,
                ""metricId"": 67,
                ""name"": ""State Assessments"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress on state standardized test"",
                    ""displayName"": ""State Standardized Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 10,
                    ""metricId"": 68,
                    ""name"": ""State Standardized Assessments"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Test scores and whether met standard"",
                        ""displayName"": ""State Assessment Performance"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 70,
                        ""metricId"": 15,
                        ""name"": ""State Assessment Performance"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 16,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 17,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 19,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 50,
                            ""metricId"": 20,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""English Language Learner"",
                    ""displayName"": ""Language Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 20,
                    ""metricId"": 1004,
                    ""name"": ""Language Assessments"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Test scores and proficiency level"",
                        ""displayName"": ""Language Assessment Proficiency"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 1,
                        ""name"": ""Language Assessment Proficiency"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Language Assessment"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1008,
                            ""name"": ""Language Assessment"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Local examinations and assessments"",
                ""displayName"": ""Local Assessments"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 30,
                ""metricId"": 1490,
                ""name"": ""Local Assessments"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Benchmark assessments"",
                    ""displayName"": ""Benchmark Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 10,
                    ""metricId"": 100,
                    ""name"": ""Benchmark Assessments"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Mastery of content in core courses"",
                        ""displayName"": ""Benchmark Assessment Mastery"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 73,
                        ""name"": ""Benchmark Assessment Mastery"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 15,
                            ""metricId"": 1042,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 74,
                            ""metricId"": 74,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 75,
                            ""metricId"": 75,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 76,
                            ""metricId"": 76,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 80,
                            ""metricId"": 80,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Learning Standard Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 20,
                    ""metricId"": 100000,
                    ""name"": ""Learning Standard Assessments"",
                    ""url"": """",
                    ""children"": [
                      {
                        ""description"": ""Mastered 70%+ of Learning Standards (as of the latest administration)"",
                        ""displayName"": ""Learning Standard Mastery By Core Subject Area"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 1230,
                        ""name"": ""Learning Standard Mastery By Core Subject Area"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA/Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1232,
                            ""name"": ""ELA/Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1238,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1240,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1244,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Students progression in coursework"",
                ""displayName"": ""Grades and Credits"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 40,
                ""metricId"": 93,
                ""name"": ""Grades and Credits"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress in current courses"",
                    ""displayName"": ""Course Grades"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 69,
                    ""metricId"": 69,
                    ""name"": ""Course Grades"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Mastery of content in core courses "",
                        ""displayName"": ""Class Grades"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 23,
                        ""name"": ""Class Grades"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Failing"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 24,
                            ""name"": ""Failing"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Failing"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 24,
                            ""name"": ""Prior Year Failing"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""# of course grades dropping below proficiency"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 25,
                            ""name"": ""# of course grades dropping below proficiency"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""# of courses repeating"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1122,
                            ""name"": ""# of courses repeating"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""Number of courses with Grades Below Proficiency for the last grading period"",
                        ""displayName"": ""Grades Below Proficiency"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 26,
                        ""name"": ""Grades Below Proficiency"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""Number of courses with Grades Below Proficiency for the last grading period"",
                        ""displayName"": ""Prior Year Grades Below Proficiency Level"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 25,
                        ""metricId"": 26,
                        ""name"": ""Prior Year Grades Below Proficiency Level"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""Whether or not student has taken; score of latest assessment"",
                        ""displayName"": ""Algebra I"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 40,
                        ""metricId"": 27,
                        ""name"": ""Algebra I"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Taken or enrolled"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 28,
                            ""name"": ""Taken or enrolled"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Passing or has passed"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 29,
                            ""name"": ""Passing or has passed"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Performance and progress in subject areas"",
                    ""displayName"": ""Subject Area Grades"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 79,
                    ""metricId"": 1086,
                    ""name"": ""Subject Area Grades"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Performance and progress in subject areas"",
                        ""displayName"": ""Failing Subject Area Course Grades"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 1491,
                        ""name"": ""Failing Subject Area Course Grades"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Reading/ELA"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1492,
                            ""name"": ""Reading/ELA"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1493,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1494,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1495,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 50,
                            ""metricId"": 1496,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Advanced coursework opportunity and performance"",
                ""displayName"": ""Advanced Academics"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 50,
                ""metricId"": 94,
                ""name"": ""Advanced Academics"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Achieving success on recent tests (eligible) vs. current enrollment in advanced courses"",
                    ""displayName"": ""Advanced Course Potential"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 103,
                    ""metricId"": 103,
                    ""name"": ""Advanced Course Potential"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Achieving success on recent tests (eligible) vs. current enrollment in advanced courses"",
                        ""displayName"": ""Advanced Course Potential"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 1285,
                        ""name"": ""Advanced Course Potential"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1472,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1474,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1476,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1478,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Achieving success through prior years' enrollment, completion and mastery of advanced courses"",
                    ""displayName"": ""Advanced Course Enrollment, Completion and Mastery - Prior Years"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 104,
                    ""metricId"": 104,
                    ""name"": ""Advanced Course Enrollment, Completion and Mastery - Prior Years"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Achieving success through prior years' enrollment, completion and mastery of advanced courses"",
                        ""displayName"": ""Advanced Course Enrollment, Completion and Mastery - Prior Years"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 1286,
                        ""name"": ""Advanced Course Enrollment, Completion and Mastery - Prior Years"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1473,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1475,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1477,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1479,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Students ability to succeed in higher education and the workforce"",
                ""displayName"": ""College and Career Readiness"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 70,
                ""metricId"": 95,
                ""name"": ""College and Career Readiness"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Whether the student has taken and score of the latest assessment"",
                    ""displayName"": ""College Entrance Exams"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 20,
                    ""metricId"": 47,
                    ""name"": ""College Entrance Exams"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Whether the student has taken and score of the latest assessment"",
                        ""displayName"": ""PSAT"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 46,
                        ""name"": ""PSAT"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Taken"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 101,
                            ""name"": ""Taken"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""At or Above Benchmark"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 102,
                            ""name"": ""At or Above Benchmark"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              }
            ]
          },
          {
            ""description"": ""Campus Overview"",
            ""displayName"": ""Overview"",
            ""domainEntityType"": ""School"",
            ""displayOrder"": 20,
            ""metricId"": 200,
            ""name"": ""Overview"",
            ""url"": ""~/Application/School/Overview.aspx"",
            ""children"": [
              {
                ""description"": ""Students attendance and discipline patterns"",
                ""displayName"": ""Attendance and Discipline"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 10,
                ""metricId"": 201,
                ""name"": ""Attendance and Discipline"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Daily and class period attendance"",
                    ""displayName"": ""Attendance"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 210,
                    ""metricId"": 210,
                    ""name"": ""Attendance"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Average daily attendance for all students with membership on the campus (unweighted)"",
                        ""displayName"": ""Average Daily Attendance"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 1444,
                        ""name"": ""Average Daily Attendance"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 0,
                            ""metricId"": 1445,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 1,
                            ""metricId"": 1448,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 2,
                            ""metricId"": 1446,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 3,
                            ""metricId"": 1446,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students meeting attendance rate threshold of 90% during the specified time frame"",
                        ""displayName"": ""Daily Attendance Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 211,
                        ""name"": ""Daily Attendance Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 212,
                            ""metricId"": 212,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 213,
                            ""metricId"": 213,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 214,
                            ""metricId"": 214,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 219,
                            ""metricId"": 214,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 8 or more absences year to date"",
                        ""displayName"": ""Days Absent"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 1672,
                        ""name"": ""Days Absent"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": ""% of students with 8 or more absences year to date"",
                            ""displayName"": ""Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1484,
                            ""name"": ""Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more absences year to date"",
                            ""displayName"": ""Prior Year Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1484,
                            ""name"": ""Prior Year Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more unexcused absences year to date"",
                            ""displayName"": ""Unexcused Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1673,
                            ""name"": ""Unexcused Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more unexcused absences year to date"",
                            ""displayName"": ""Prior Year Unexcused Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 35,
                            ""metricId"": 1673,
                            ""name"": ""Prior Year Unexcused Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students exceeding class period absence rate threshold of 10% during the specified timeframe"",
                        ""displayName"": ""Class Period Absence Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 40,
                        ""metricId"": 216,
                        ""name"": ""Class Period Absence Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 217,
                            ""metricId"": 217,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 218,
                            ""metricId"": 218,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 219,
                            ""metricId"": 219,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 224,
                            ""metricId"": 219,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% students exceeding tardy rate threshold of 10% during specified time frame"",
                        ""displayName"": ""Tardy Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 50,
                        ""metricId"": 1130,
                        ""name"": ""Tardy Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1132,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1133,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1134,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 35,
                            ""metricId"": 1134,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Discipline incidents and actions"",
                    ""displayName"": ""Discipline"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 416,
                    ""metricId"": 416,
                    ""name"": ""Discipline"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with 1 or more incidents in a given grading period"",
                        ""displayName"": ""State Offenses"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 2350,
                        ""name"": ""State Offenses"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 2351,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 2352,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 2353,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year State Offenses"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 2353,
                            ""name"": ""Prior Year State Offenses"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1 or more incidents in a given grading period"",
                        ""displayName"": ""Non-State Offenses"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 2354,
                        ""name"": ""Non-State Offenses"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 2355,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 2356,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 2357,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Non-State Offenses"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 2357,
                            ""name"": ""Prior Year Non-State Offenses"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1+ state reportable offenses and/or excessive (5+) school code of conduct incidents in a given grading period"",
                        ""displayName"": ""All Discipline Incidents"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 221,
                        ""name"": ""All Discipline Incidents"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Current Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 222,
                            ""metricId"": 222,
                            ""name"": ""Current Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Previous Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 223,
                            ""metricId"": 223,
                            ""name"": ""Previous Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 224,
                            ""metricId"": 224,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 229,
                            ""metricId"": 224,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1 or more incidents in a given grading period"",
                        ""displayName"": ""School Code of Conduct Incidents"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 50,
                        ""metricId"": 226,
                        ""name"": ""School Code of Conduct Incidents"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Current Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 227,
                            ""metricId"": 227,
                            ""name"": ""Current Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Previous Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 228,
                            ""metricId"": 228,
                            ""name"": ""Previous Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 229,
                            ""metricId"": 229,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 234,
                            ""metricId"": 229,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""State examinations and assessments"",
                ""displayName"": ""State Assessments"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 20,
                ""metricId"": 203,
                ""name"": ""State Assessments"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress on state standardized test"",
                    ""displayName"": ""State Standardized Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 1013,
                    ""name"": ""State Standardized Assessments"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students meeting standard"",
                        ""displayName"": ""State Assessment"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 60,
                        ""metricId"": 233,
                        ""name"": ""State Assessment"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 234,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 235,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 236,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 237,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""All tests"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 51,
                            ""metricId"": 238,
                            ""name"": ""All tests"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with advanced performance"",
                        ""displayName"": ""State Assessment Advanced Performance"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 63,
                        ""metricId"": 1017,
                        ""name"": ""State Assessment Advanced Performance"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1018,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1019,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 1020,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 1021,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students failing state assessment 2 consecutive administrations "",
                        ""displayName"": ""Repeat State Assessment Failures"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 70,
                        ""metricId"": 244,
                        ""name"": ""Repeat State Assessment Failures"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of eligible students who did not take test"",
                        ""displayName"": ""State Assessment Non-Participation"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 80,
                        ""metricId"": 240,
                        ""name"": ""State Assessment Non-Participation"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 410,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 411,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 412,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 413,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Performance and progress on language assessments"",
                    ""displayName"": ""Language Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 20,
                    ""metricId"": 1001,
                    ""name"": ""Language Assessments"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students at or above the advanced proficiency level"",
                        ""displayName"": ""Language Assessment Proficiency"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 1002,
                        ""name"": ""Language Assessment Proficiency"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": ""Beginning Proficiency Level Rate"",
                            ""displayName"": ""Language Assessment"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 0,
                            ""metricId"": 1154,
                            ""name"": ""Language Assessment"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Local examinations and assessments"",
                ""displayName"": ""Local Assessments"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 30,
                ""metricId"": 1489,
                ""name"": ""Local Assessments"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress on local benchmark assessments"",
                    ""displayName"": ""Benchmark Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 1014,
                    ""name"": ""Benchmark Assessments"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students scoring at or above the campus threshold of 70% (as of the latest administration)"",
                        ""displayName"": ""Benchmark Performance"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 250,
                        ""name"": ""Benchmark Performance"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 251,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1052,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 254,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 257,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 1015,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Learning Standard Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 20,
                    ""metricId"": 100001,
                    ""name"": ""Learning Standard Assessments"",
                    ""url"": """",
                    ""children"": [
                      {
                        ""description"": ""Mastered 70%+ of Learning Standards (as of the latest administration)"",
                        ""displayName"": ""Learning Standard Mastery by Core Subject Area"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 1253,
                        ""name"": ""Learning Standard Mastery by Core Subject Area"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1254,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1256,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 1257,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 1258,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Students progression in coursework"",
                ""displayName"": ""Grades and Credits"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 40,
                ""metricId"": 202,
                ""name"": ""Grades and Credits"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress in current courses"",
                    ""displayName"": ""Course Grades"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 204,
                    ""name"": ""Course Grades"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with at least 1 grade below proficiency"",
                        ""displayName"": ""Grades Below Proficiency"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 262,
                        ""name"": ""Grades Below Proficiency"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""% with 1 or more course grades below proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 263,
                            ""name"": ""% with 1 or more course grades below proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year % with 1 or more course Grades Below Proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 263,
                            ""name"": ""Prior Year % with 1 or more course Grades Below Proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with 2 or more course grades below proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 264,
                            ""name"": ""% with 2 or more course grades below proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year % with 2 or more course Grades Below Proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 264,
                            ""name"": ""Prior Year % with 2 or more course Grades Below Proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with 3 or more course grades below proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 265,
                            ""name"": ""% with 3 or more course grades below proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year % with 3 or more course Grades Below Proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 60,
                            ""metricId"": 265,
                            ""name"": ""Prior Year % with 3 or more course Grades Below Proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with core subject course grades dropping below proficiency from prior grading period"",
                        ""displayName"": ""Dropping Class Grades"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 40,
                        ""metricId"": 266,
                        ""name"": ""Dropping Class Grades"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 267,
                            ""metricId"": 267,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 268,
                            ""metricId"": 268,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 269,
                            ""metricId"": 269,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 414,
                            ""metricId"": 414,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students who are taking/taken course; % who are passing/passed course by 8th grade"",
                        ""displayName"": ""Algebra I - 8th Grade"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 50,
                        ""metricId"": 1283,
                        ""name"": ""Algebra I - 8th Grade"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Taking or have Taken"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1442,
                            ""name"": ""Taking or have Taken"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Passing or have Passed"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 1443,
                            ""name"": ""Passing or have Passed"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""% of students with failing grades in one of the four core subject areas during the current year"",
                    ""displayName"": ""Subject Area / Course Grades"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 20,
                    ""metricId"": 1061,
                    ""name"": ""Subject Area / Course Grades"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with a failing grade in one or more courses in each subject area."",
                        ""displayName"": ""Failing Subject Area Grades"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 1062,
                        ""name"": ""Failing Subject Area Grades"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1063,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1064,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1065,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 1066,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 1067,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Advanced coursework opportunity and performance"",
                ""displayName"": ""Advanced Academics"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 50,
                ""metricId"": 206,
                ""name"": ""Advanced Academics"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Student opportunity and performance in advanced coursework"",
                    ""displayName"": ""Pre Advanced/Advanced Academics"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 30,
                    ""metricId"": 1105,
                    ""name"": ""Pre Advanced/Advanced Academics"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": """",
                        ""displayName"": ""Pre Advanced/Advanced Course Potential"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 1106,
                        ""name"": ""Pre Advanced/Advanced Course Potential"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""% with ELA/Reading State Assessment Commended or PSAT 80% currently enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 2384,
                            ""name"": ""% with ELA/Reading State Assessment Commended or PSAT 80% currently enrolled"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with Mathematics State Assessment Commended or PSAT 80% currently enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 2385,
                            ""name"": ""% with Mathematics State Assessment Commended or PSAT 80% currently enrolled"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with Science State Assessment Commended or PSAT 80% currently enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 2386,
                            ""name"": ""% with Science State Assessment Commended or PSAT 80% currently enrolled"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with Social Studies State Assessment Commended or PSAT 80% currently enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 2387,
                            ""name"": ""% with Social Studies State Assessment Commended or PSAT 80% currently enrolled"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with ELA / Reading State Assessment Commended or PSAT 80%, but not enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 380,
                            ""name"": ""% with ELA / Reading State Assessment Commended or PSAT 80%, but not enrolled"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with Mathematics State Assessment Commended or PSAT 80%, but not enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 60,
                            ""metricId"": 381,
                            ""name"": ""% with Mathematics State Assessment Commended or PSAT 80%, but not enrolled"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with Science State Assessment Commended, but not enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 70,
                            ""metricId"": 382,
                            ""name"": ""% with Science State Assessment Commended, but not enrolled"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with Social Studies State Assessment Commended, but not enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 80,
                            ""metricId"": 383,
                            ""name"": ""% with Social Studies State Assessment Commended, but not enrolled"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students currently enrolled in at least 1 AP, IB or Dual Credit core course"",
                        ""displayName"": ""Advanced Course Enrollment"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 301,
                        ""name"": ""Advanced Course Enrollment"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of students completing at least 1 AP, IB or Dual Credit course in one of the four core subject areas in prior years"",
                        ""displayName"": ""Advanced Course Completion"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 40,
                        ""metricId"": 303,
                        ""name"": ""Advanced Course Completion"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      }
                    ]
                  }
                ]
              }
            ]
          },
          {
            ""description"": """",
            ""displayName"": ""Operational Dashboard"",
            ""domainEntityType"": ""School"",
            ""displayOrder"": 40,
            ""metricId"": 1029,
            ""name"": ""Operational Dashboard"",
            ""url"": ""~/Application/School/Metrics.aspx"",
            ""children"": [
              {
                ""description"": """",
                ""displayName"": ""Staff"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 10,
                ""metricId"": 1030,
                ""name"": ""Staff"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": """",
                    ""displayName"": ""Teacher Attendance"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 1031,
                    ""name"": ""Teacher Attendance"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers meeting attendance rate threshold of 90%"",
                        ""displayName"": ""Teacher Attendance Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 361,
                        ""name"": ""Teacher Attendance Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1032,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1033,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1034,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 35,
                            ""metricId"": 1034,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Experience, Education, and Certifications"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 20,
                    ""metricId"": 1035,
                    ""name"": ""Experience, Education, and Certifications"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers with >5 years of professional teaching experience"",
                        ""displayName"": ""Teacher Experience"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 369,
                        ""name"": ""Teacher Experience"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""All Subjects"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 370,
                            ""metricId"": 370,
                            ""name"": ""All Subjects"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""ELA/Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 371,
                            ""metricId"": 371,
                            ""name"": ""ELA/Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 372,
                            ""metricId"": 372,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 424,
                            ""metricId"": 424,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 425,
                            ""metricId"": 425,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of teachers with advanced degrees"",
                        ""displayName"": ""Teacher Education"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 1069,
                        ""name"": ""Teacher Education"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Bachelors or Higher"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1070,
                            ""name"": ""Bachelors or Higher"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Masters or Doctorate"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1071,
                            ""name"": ""Masters or Doctorate"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Retention and Staffing"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 30,
                    ""metricId"": 1037,
                    ""name"": ""Retention and Staffing"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers returning from the previous year"",
                        ""displayName"": ""Teacher Retention"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 375,
                        ""name"": ""Teacher Retention"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      }
                    ]
                  }
                ]
              }
            ]
          }
        ]
      },
      {
        ""description"": """",
        ""displayName"": ""High School"",
        ""domainEntityType"": """",
        ""displayOrder"": 30,
        ""metricId"": 1006,
        ""name"": ""High School"",
        ""url"": """",
        ""children"": [
          {
            ""description"": ""STUDENT OVERVIEW"",
            ""displayName"": ""Overview"",
            ""domainEntityType"": ""StudentSchool"",
            ""displayOrder"": 10,
            ""metricId"": 65,
            ""name"": ""Overview"",
            ""url"": ""~/Application/Student/Overview.aspx"",
            ""children"": [
              {
                ""description"": ""Student attendance and discipline patterns"",
                ""displayName"": ""Attendance and Discipline"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 10,
                ""metricId"": 66,
                ""name"": ""Attendance and Discipline"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Daily and class period attendance"",
                    ""displayName"": ""Attendance"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 98,
                    ""metricId"": 98,
                    ""name"": ""Attendance"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of days student is in attendance"",
                        ""displayName"": ""Daily Attendance Rate"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 2,
                        ""name"": ""Daily Attendance Rate"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 3,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 4,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 5,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 35,
                            ""metricId"": 5,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""# of days absent year to date"",
                        ""displayName"": ""Days Absent"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 1670,
                        ""name"": ""Days Absent"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": ""# of days absent year to date"",
                            ""displayName"": ""Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1483,
                            ""name"": ""Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""# of days absent year to date"",
                            ""displayName"": ""Prior Year Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1483,
                            ""name"": ""Prior Year Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Unexcused Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1671,
                            ""name"": ""Unexcused Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Unexcused Days Absent"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 35,
                            ""metricId"": 1671,
                            ""name"": ""Prior Year Unexcused Days Absent"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of total class periods missed"",
                        ""displayName"": ""Class Period Absence Rate"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 30,
                        ""metricId"": 6,
                        ""name"": ""Class Period Absence Rate"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 7,
                            ""metricId"": 7,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 8,
                            ""metricId"": 8,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 9,
                            ""metricId"": 9,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 14,
                            ""metricId"": 9,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of tardies in a given period"",
                        ""displayName"": ""Tardy Rate"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 40,
                        ""metricId"": 1000,
                        ""name"": ""Tardy Rate"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1077,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1078,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1079,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 35,
                            ""metricId"": 1079,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Log of discipline incidents and actions year to date"",
                    ""displayName"": ""Discipline"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 99,
                    ""metricId"": 99,
                    ""name"": ""Discipline"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""# of state reportable offenses in a given period"",
                        ""displayName"": ""State Offenses"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 105,
                        ""name"": ""State Offenses"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 106,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 107,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 108,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year State Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 108,
                            ""name"": ""Prior Year State Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""# of Non-state reportable offenses in a given period"",
                        ""displayName"": ""Non-State Offenses"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 111,
                        ""name"": ""Non-State Offenses"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 112,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 113,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 114,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Non-State Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 114,
                            ""name"": ""Prior Year Non-State Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""Log of discipline incidents and actions year-to-date"",
                        ""displayName"": ""Discipline Referrals"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 40,
                        ""metricId"": 10,
                        ""name"": ""Discipline Referrals"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""State Reportable Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 78,
                            ""name"": ""State Reportable Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year State Reportable Offenses"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 78,
                            ""name"": ""Prior Year State Reportable Offenses"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""School Code of Conduct"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 79,
                            ""name"": ""School Code of Conduct"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year School Code of Conduct"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 79,
                            ""name"": ""Prior Year School Code of Conduct"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""State examinations and assessments"",
                ""displayName"": ""State Assessments"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 20,
                ""metricId"": 67,
                ""name"": ""State Assessments"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress on state standardized test"",
                    ""displayName"": ""State Standardized Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 10,
                    ""metricId"": 68,
                    ""name"": ""State Standardized Assessments"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Test scores and whether met standard"",
                        ""displayName"": ""State Assessment Performance"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 70,
                        ""metricId"": 15,
                        ""name"": ""State Assessment Performance"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 16,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 17,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 19,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 50,
                            ""metricId"": 20,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""English Language Learner"",
                    ""displayName"": ""Language Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 20,
                    ""metricId"": 1004,
                    ""name"": ""Language Assessments"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Test scores and proficiency level"",
                        ""displayName"": ""Language Assessment Proficiency"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 1,
                        ""name"": ""Language Assessment Proficiency"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Language Assessment"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1008,
                            ""name"": ""Language Assessment"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Local examinations and assessments"",
                ""displayName"": ""Local Assessments"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 30,
                ""metricId"": 1490,
                ""name"": ""Local Assessments"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Benchmark assessments"",
                    ""displayName"": ""Benchmark Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 10,
                    ""metricId"": 100,
                    ""name"": ""Benchmark Assessments"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Mastery of content in core courses"",
                        ""displayName"": ""Benchmark Assessment Mastery"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 73,
                        ""name"": ""Benchmark Assessment Mastery"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 74,
                            ""metricId"": 74,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 75,
                            ""metricId"": 75,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 76,
                            ""metricId"": 76,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 80,
                            ""metricId"": 80,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Learning Standard Assessments"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 20,
                    ""metricId"": 100000,
                    ""name"": ""Learning Standard Assessments"",
                    ""url"": """",
                    ""children"": [
                      {
                        ""description"": ""Mastered 70%+ of Learning Standards (as of the latest administration)"",
                        ""displayName"": ""Learning Standard Mastery By Core Subject Area"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 1230,
                        ""name"": ""Learning Standard Mastery By Core Subject Area"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA/Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1232,
                            ""name"": ""ELA/Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1238,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1240,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1244,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Students progression in coursework"",
                ""displayName"": ""Grades and Credits"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 40,
                ""metricId"": 93,
                ""name"": ""Grades and Credits"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress in current courses"",
                    ""displayName"": ""Course Grades"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 69,
                    ""metricId"": 69,
                    ""name"": ""Course Grades"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Mastery of content in core courses "",
                        ""displayName"": ""Class Grades"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 23,
                        ""name"": ""Class Grades"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Failing"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 24,
                            ""name"": ""Failing"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Failing"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 24,
                            ""name"": ""Prior Year Failing"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""# of course grades dropping below proficiency"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 25,
                            ""name"": ""# of course grades dropping below proficiency"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""# of courses repeating"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1122,
                            ""name"": ""# of courses repeating"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""Number of courses with Grades Below Proficiency for the last grading period"",
                        ""displayName"": ""Grades Below Proficiency"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 30,
                        ""metricId"": 26,
                        ""name"": ""Grades Below Proficiency"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""Number of courses with Grades Below Proficiency for the last grading period"",
                        ""displayName"": ""Prior Year Grades Below Proficiency Level"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 35,
                        ""metricId"": 26,
                        ""name"": ""Prior Year Grades Below Proficiency Level"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""Whether or not student has taken; score of latest assessment"",
                        ""displayName"": ""Algebra I"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 45,
                        ""metricId"": 27,
                        ""name"": ""Algebra I"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Taken or enrolled"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 28,
                            ""name"": ""Taken or enrolled"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Passing or has passed"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 29,
                            ""name"": ""Passing or has passed"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Student progression"",
                    ""displayName"": ""Credits"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 70,
                    ""metricId"": 70,
                    ""name"": ""Credits"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": """",
                        ""displayName"": ""Student Credit Accumulation"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 0,
                        ""metricId"": 2379,
                        ""name"": ""Student Credit Accumulation"",
                        ""url"": """",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Total Credit Accumulation"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 2366,
                            ""name"": ""Total Credit Accumulation"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""9th Grade"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 2367,
                            ""name"": ""9th Grade"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""10th Grade"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 2368,
                            ""name"": ""10th Grade"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""11th Grade"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 2369,
                            ""name"": ""11th Grade"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""12th Grade"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 50,
                            ""metricId"": 2370,
                            ""name"": ""12th Grade"",
                            ""url"": """",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": """",
                        ""displayName"": ""On Track To Graduate"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 1,
                        ""metricId"": 2381,
                        ""name"": ""On Track To Graduate"",
                        ""url"": """",
                        ""children"": [
                          {
                            ""description"": ""At the end of the prior school year, student earned at least one credit per year in each of the four core subject courses for the recommended graduation plan"",
                            ""displayName"": ""Student On Track to Graduate"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 0,
                            ""metricId"": 2377,
                            ""name"": ""Student On Track to Graduate"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""Cumulative number earned as of latest grading period, overall and by subject"",
                        ""displayName"": ""Credit Accumulation"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 40,
                        ""name"": ""Credit Accumulation"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""At the end of the prior school year, student earned at least one credit per year in each of the four core subject courses for the recommended graduation plan"",
                        ""displayName"": ""On Track to Graduate (4x4 Requirement)"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 20,
                        ""metricId"": 41,
                        ""name"": ""On Track to Graduate (4x4 Requirement)"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": []
                      }
                    ]
                  },
                  {
                    ""description"": ""Performance and progress in subject areas"",
                    ""displayName"": ""Subject Area Grades"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 80,
                    ""metricId"": 1086,
                    ""name"": ""Subject Area Grades"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Performance and progress in subject areas"",
                        ""displayName"": ""Failing Subject Area Course Grades"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 1491,
                        ""name"": ""Failing Subject Area Course Grades"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Reading/ELA"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1492,
                            ""name"": ""Reading/ELA"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1493,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1494,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1495,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 50,
                            ""metricId"": 1496,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Advanced coursework opportunity and performance"",
                ""displayName"": ""Advanced Academics"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 70,
                ""metricId"": 94,
                ""name"": ""Advanced Academics"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Achieving success on recent tests (eligible) vs. current enrollment in advanced courses"",
                    ""displayName"": ""Advanced Course Potential"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 10,
                    ""metricId"": 103,
                    ""name"": ""Advanced Course Potential"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Achieving success on recent tests (eligible) vs. current enrollment in advanced courses"",
                        ""displayName"": ""Advanced Course Potential"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 1285,
                        ""name"": ""Advanced Course Potential"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1472,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1474,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1476,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1478,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Achieving success through prior years' enrollment, completion and mastery of advanced courses"",
                    ""displayName"": ""Advanced Course Enrollment, Completion and Mastery - Prior Years"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 30,
                    ""metricId"": 104,
                    ""name"": ""Advanced Course Enrollment, Completion and Mastery - Prior Years"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Achieving success through prior years' enrollment, completion and mastery of advanced courses"",
                        ""displayName"": ""Advanced Course Enrollment, Completion and Mastery - Prior Years"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 10,
                        ""metricId"": 1286,
                        ""name"": ""Advanced Course Enrollment, Completion and Mastery - Prior Years"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 1473,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 1475,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 30,
                            ""metricId"": 1477,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 40,
                            ""metricId"": 1479,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Students ability to succeed in higher education and the workforce"",
                ""displayName"": ""College and Career Readiness"",
                ""domainEntityType"": ""StudentSchool"",
                ""displayOrder"": 90,
                ""metricId"": 95,
                ""name"": ""College and Career Readiness"",
                ""url"": ""~/Application/Student/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Whether the student has taken and score of the latest assessment"",
                    ""displayName"": ""College Entrance Exams"",
                    ""domainEntityType"": ""StudentSchool"",
                    ""displayOrder"": 10,
                    ""metricId"": 47,
                    ""name"": ""College Entrance Exams"",
                    ""url"": ""~/Application/Student/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Whether the student has taken and score of the latest assessment"",
                        ""displayName"": ""PSAT"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 30,
                        ""metricId"": 46,
                        ""name"": ""PSAT"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Taken"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 10,
                            ""metricId"": 101,
                            ""name"": ""Taken"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""At or Above Benchmark"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 20,
                            ""metricId"": 102,
                            ""name"": ""At or Above Benchmark"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""Whether the student has taken and score of the latest assessment"",
                        ""displayName"": ""SAT"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 40,
                        ""metricId"": 48,
                        ""name"": ""SAT"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Taken"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 49,
                            ""metricId"": 49,
                            ""name"": ""Taken"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""At or Above Benchmark"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 50,
                            ""metricId"": 50,
                            ""name"": ""At or Above Benchmark"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""Whether the student has taken and score of the latest assessment"",
                        ""displayName"": ""ACT"",
                        ""domainEntityType"": ""StudentSchool"",
                        ""displayOrder"": 90,
                        ""metricId"": 52,
                        ""name"": ""ACT"",
                        ""url"": ""~/Application/Student/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Taken"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 53,
                            ""metricId"": 53,
                            ""name"": ""Taken"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""At or Above Benchmark"",
                            ""domainEntityType"": ""StudentSchool"",
                            ""displayOrder"": 64,
                            ""metricId"": 54,
                            ""name"": ""At or Above Benchmark"",
                            ""url"": ""~/Application/Student/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              }
            ]
          },
          {
            ""description"": ""Campus Overview"",
            ""displayName"": ""Overview"",
            ""domainEntityType"": ""School"",
            ""displayOrder"": 20,
            ""metricId"": 200,
            ""name"": ""Overview"",
            ""url"": ""~/Application/School/Overview.aspx"",
            ""children"": [
              {
                ""description"": ""Students attendance and discipline patterns"",
                ""displayName"": ""Attendance and Discipline"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 10,
                ""metricId"": 201,
                ""name"": ""Attendance and Discipline"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Daily and class period attendance"",
                    ""displayName"": ""Attendance"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 210,
                    ""name"": ""Attendance"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Average daily attendance for all students with membership on the campus (unweighted)"",
                        ""displayName"": ""Average Daily Attendance"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 1444,
                        ""name"": ""Average Daily Attendance"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 0,
                            ""metricId"": 1445,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 1,
                            ""metricId"": 1448,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 2,
                            ""metricId"": 1446,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 3,
                            ""metricId"": 1446,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students meeting attendance rate threshold of 90% during the specified time frame"",
                        ""displayName"": ""Daily Attendance Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 211,
                        ""name"": ""Daily Attendance Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 212,
                            ""metricId"": 212,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 213,
                            ""metricId"": 213,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 214,
                            ""metricId"": 214,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 219,
                            ""metricId"": 214,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 8 or more absences year to date"",
                        ""displayName"": ""Days Absent"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 1672,
                        ""name"": ""Days Absent"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": ""% of students with 8 or more absences year to date"",
                            ""displayName"": ""Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1484,
                            ""name"": ""Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more absences year to date"",
                            ""displayName"": ""Prior Year Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1484,
                            ""name"": ""Prior Year Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more unexcused absences year to date"",
                            ""displayName"": ""Unexcused Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1673,
                            ""name"": ""Unexcused Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more unexcused absences year to date"",
                            ""displayName"": ""Prior Year Unexcused Days Absent"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 35,
                            ""metricId"": 1673,
                            ""name"": ""Prior Year Unexcused Days Absent"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students exceeding class period absence rate threshold of 10% during the specified timeframe"",
                        ""displayName"": ""Class Period Absence Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 40,
                        ""metricId"": 216,
                        ""name"": ""Class Period Absence Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 217,
                            ""metricId"": 217,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 218,
                            ""metricId"": 218,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 219,
                            ""metricId"": 219,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 224,
                            ""metricId"": 219,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% students exceeding tardy rate threshold of 10% during specified time frame"",
                        ""displayName"": ""Tardy Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 50,
                        ""metricId"": 1130,
                        ""name"": ""Tardy Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1132,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1133,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1134,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 35,
                            ""metricId"": 1134,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Discipline incidents and actions"",
                    ""displayName"": ""Discipline"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 20,
                    ""metricId"": 416,
                    ""name"": ""Discipline"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with 1 or more incidents in a given grading period"",
                        ""displayName"": ""State Offenses"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 2350,
                        ""name"": ""State Offenses"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 2351,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 2352,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 2353,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year State Offenses"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 2353,
                            ""name"": ""Prior Year State Offenses"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1 or more incidents in a given grading period"",
                        ""displayName"": ""Non-State Offenses"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 2354,
                        ""name"": ""Non-State Offenses"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 2355,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 2356,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 2357,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Non-State Offenses"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 2357,
                            ""name"": ""Prior Year Non-State Offenses"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1+ state reportable offenses and/or excessive (5+) school code of conduct incidents in a given grading period"",
                        ""displayName"": ""All Discipline Incidents"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 221,
                        ""name"": ""All Discipline Incidents"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Current Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 222,
                            ""metricId"": 222,
                            ""name"": ""Current Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Previous Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 223,
                            ""metricId"": 223,
                            ""name"": ""Previous Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 224,
                            ""metricId"": 224,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 229,
                            ""metricId"": 224,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1 or more incidents in a given grading period"",
                        ""displayName"": ""School Code of Conduct Incidents"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 50,
                        ""metricId"": 226,
                        ""name"": ""School Code of Conduct Incidents"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Current Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 227,
                            ""metricId"": 227,
                            ""name"": ""Current Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Previous Grading Period"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 228,
                            ""metricId"": 228,
                            ""name"": ""Previous Grading Period"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 229,
                            ""metricId"": 229,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 234,
                            ""metricId"": 229,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""State examinations and assessments"",
                ""displayName"": ""State Assessments"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 20,
                ""metricId"": 203,
                ""name"": ""State Assessments"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress on state standardized test"",
                    ""displayName"": ""State Standardized Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 1013,
                    ""name"": ""State Standardized Assessments"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students meeting standard"",
                        ""displayName"": ""State Assessment"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 233,
                        ""name"": ""State Assessment"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 234,
                            ""metricId"": 234,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 235,
                            ""metricId"": 235,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 236,
                            ""metricId"": 236,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 237,
                            ""metricId"": 237,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""All tests"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 238,
                            ""metricId"": 238,
                            ""name"": ""All tests"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with advanced performance"",
                        ""displayName"": ""State Assessment Advanced Performance"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 31,
                        ""metricId"": 1017,
                        ""name"": ""State Assessment Advanced Performance"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1018,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1019,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1020,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 1021,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students failing state assessment 2 consecutive administrations "",
                        ""displayName"": ""Repeat State Assessment Failures"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 40,
                        ""metricId"": 244,
                        ""name"": ""Repeat State Assessment Failures"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 245,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of eligible students who did not take test"",
                        ""displayName"": ""State Assessment Non-Participation"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 50,
                        ""metricId"": 240,
                        ""name"": ""State Assessment Non-Participation"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 410,
                            ""metricId"": 410,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 411,
                            ""metricId"": 411,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 412,
                            ""metricId"": 412,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 413,
                            ""metricId"": 413,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Performance and progress on language assessments"",
                    ""displayName"": ""Language Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 20,
                    ""metricId"": 1001,
                    ""name"": ""Language Assessments"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students at or above the advanced proficiency level"",
                        ""displayName"": ""Language Assessment Proficiency"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 1002,
                        ""name"": ""Language Assessment Proficiency"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": ""Beginning Proficiency Level Rate"",
                            ""displayName"": ""Language Assessment"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 0,
                            ""metricId"": 1154,
                            ""name"": ""Language Assessment"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Local examinations and assessments"",
                ""displayName"": ""Local Assessments"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 30,
                ""metricId"": 1489,
                ""name"": ""Local Assessments"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress on local benchmark assessments"",
                    ""displayName"": ""Benchmark Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 1014,
                    ""name"": ""Benchmark Assessments"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students scoring at or above the campus threshold of 70% (as of the latest administration)"",
                        ""displayName"": ""Benchmark Performance"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 250,
                        ""name"": ""Benchmark Performance"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 251,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 254,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 257,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 1015,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Learning Standard Assessments"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 20,
                    ""metricId"": 100001,
                    ""name"": ""Learning Standard Assessments"",
                    ""url"": """",
                    ""children"": [
                      {
                        ""description"": ""Mastered 70%+ of Learning Standards (as of the latest administration)"",
                        ""displayName"": ""Learning Standard Mastery by Core Subject Area"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 1253,
                        ""name"": ""Learning Standard Mastery by Core Subject Area"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1254,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1256,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 1257,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 1258,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Students progression in coursework"",
                ""displayName"": ""Grades and Credits"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 40,
                ""metricId"": 202,
                ""name"": ""Grades and Credits"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress in current courses"",
                    ""displayName"": ""Course Grades"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 30,
                    ""metricId"": 204,
                    ""name"": ""Course Grades"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with at least 1 grade below proficiency"",
                        ""displayName"": ""Grades Below Proficiency"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 262,
                        ""metricId"": 262,
                        ""name"": ""Grades Below Proficiency"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""% with 1 or more course grades below proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 263,
                            ""name"": ""% with 1 or more course grades below proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year % with 1 or more course Grades Below Proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 263,
                            ""name"": ""Prior Year % with 1 or more course Grades Below Proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with 2 or more course grades below proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 264,
                            ""name"": ""% with 2 or more course grades below proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year % with 2 or more course Grades Below Proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 264,
                            ""name"": ""Prior Year % with 2 or more course Grades Below Proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with 3 or more course grades below proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 265,
                            ""name"": ""% with 3 or more course grades below proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year % with 3 or more course Grades Below Proficiency"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 60,
                            ""metricId"": 265,
                            ""name"": ""Prior Year % with 3 or more course Grades Below Proficiency"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with core subject course grades dropping below proficiency from prior grading period"",
                        ""displayName"": ""Dropping Class Grades"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 276,
                        ""metricId"": 266,
                        ""name"": ""Dropping Class Grades"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 267,
                            ""metricId"": 267,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 268,
                            ""metricId"": 268,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 269,
                            ""metricId"": 269,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 414,
                            ""metricId"": 414,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students who are taking/taken course; % who are passing/passed course by 9th grade"",
                        ""displayName"": ""Algebra I"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 280,
                        ""metricId"": 270,
                        ""name"": ""Algebra I"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Taking or have Taken"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 70,
                            ""metricId"": 2331,
                            ""name"": ""Taking or have Taken"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Passing or have Passed"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 80,
                            ""metricId"": 2332,
                            ""name"": ""Passing or have Passed"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""% of students with failing grades in one of the four core subject areas during the current year"",
                    ""displayName"": ""Subject Area / Course Grades"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 35,
                    ""metricId"": 1061,
                    ""name"": ""Subject Area / Course Grades"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with a failing grade in one or more courses in each subject area."",
                        ""displayName"": ""Failing Subject Area Grades"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 1062,
                        ""name"": ""Failing Subject Area Grades"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1063,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1064,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1065,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 1066,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 50,
                            ""metricId"": 1067,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Students progress toward graduation"",
                    ""displayName"": ""Credits"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 40,
                    ""metricId"": 205,
                    ""name"": ""Credits"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": """",
                        ""displayName"": ""School Credit Accumulation"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 0,
                        ""metricId"": 2371,
                        ""name"": ""School Credit Accumulation"",
                        ""url"": """",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of students meeting required # of credits for Recommended Graduation Plan as of the end of the prior school year"",
                        ""displayName"": ""Credit Accumulation"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 283,
                        ""metricId"": 283,
                        ""name"": ""Credit Accumulation"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""10th Grade"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 285,
                            ""metricId"": 285,
                            ""name"": ""10th Grade"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""11th Grade"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 286,
                            ""metricId"": 286,
                            ""name"": ""11th Grade"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""12th Grade"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 287,
                            ""metricId"": 287,
                            ""name"": ""12th Grade"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students who have earned the expected credit each of the four core subject areas at the end of the prior school year"",
                        ""displayName"": ""On Track to Graduate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 288,
                        ""metricId"": 288,
                        ""name"": ""On Track to Graduate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""10th Grade"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 417,
                            ""metricId"": 417,
                            ""name"": ""10th Grade"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""11th Grade"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 418,
                            ""metricId"": 418,
                            ""name"": ""11th Grade"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""12th Grade"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 419,
                            ""metricId"": 419,
                            ""name"": ""12th Grade"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Advanced coursework opportunity and performance"",
                ""displayName"": ""Advanced Academics"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 60,
                ""metricId"": 206,
                ""name"": ""Advanced Academics"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Student opportunity and performance in advanced coursework"",
                    ""displayName"": ""Advanced Academics"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 30,
                    ""metricId"": 1022,
                    ""name"": ""Advanced Academics"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students eligible for advanced courses, but not currently enrolled"",
                        ""displayName"": ""Advanced Course Potential"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 290,
                        ""name"": ""Advanced Course Potential"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""% with ELA / Reading State Assessment Commended or PSAT 80%, but not enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 380,
                            ""name"": ""% with ELA / Reading State Assessment Commended or PSAT 80%, but not enrolled"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with Mathematics State Assessment Commended or PSAT 80%, but not enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 381,
                            ""name"": ""% with Mathematics State Assessment Commended or PSAT 80%, but not enrolled"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with Science State Assessment Commended, but not enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 382,
                            ""name"": ""% with Science State Assessment Commended, but not enrolled"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with Social Studies State Assessment Commended, but not enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 383,
                            ""name"": ""% with Social Studies State Assessment Commended, but not enrolled"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students eligible for advanced course currently enrolled"",
                        ""displayName"": ""Advanced Course Potential"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 2383,
                        ""name"": ""Advanced Course Potential"",
                        ""url"": """",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""% with ELA/Reading State Assessment Commended or PSAT 80% currently enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 2384,
                            ""name"": ""% with ELA/Reading State Assessment Commended or PSAT 80% currently enrolled"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": "" % with Mathematics State Assessment Commended or PSAT 80% currently enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 2385,
                            ""name"": ""% with Mathematics State Assessment Commended or PSAT 80% currently enrolled"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with Science State Assessment Commended or PSAT 80% currently enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 2386,
                            ""name"": ""% with Science State Assessment Commended or PSAT 80% currently enrolled"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""% with Social Studies State Assessment Commended or PSAT 80% currently enrolled"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 2387,
                            ""name"": ""% with Social Studies State Assessment Commended or PSAT 80% currently enrolled"",
                            ""url"": """",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students currently enrolled in at least 1 AP, IB or Dual Credit core course"",
                        ""displayName"": ""Advanced Course Enrollment"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 301,
                        ""name"": ""Advanced Course Enrollment"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of students completing at least 1 AP, IB or Dual Credit course in one of the four core subject areas in prior years"",
                        ""displayName"": ""Advanced Course Completion"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 40,
                        ""metricId"": 303,
                        ""name"": ""Advanced Course Completion"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Students ability to succeed in higher education and the workforce"",
                ""displayName"": ""College and Career Readiness"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 70,
                ""metricId"": 207,
                ""name"": ""College and Career Readiness"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Completion, Graduation, and Dropout Rates"",
                    ""displayName"": ""Graduation Status"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 1023,
                    ""name"": ""Graduation Status"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students, from 9th grade cohort, who have completed high school within four years"",
                        ""displayName"": ""High School Completion Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 1024,
                        ""name"": ""High School Completion Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of 9th grade freshman graduating within 4 years (4-year adjusted cohort)"",
                        ""displayName"": ""High School Graduation Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 316,
                        ""name"": ""High School Graduation Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of students dropping out who were enrolled in a given school year in grades 9�12, but did not return to a state public school"",
                        ""displayName"": ""High School Dropout Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 30,
                        ""metricId"": 317,
                        ""name"": ""High School Dropout Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      }
                    ]
                  },
                  {
                    ""description"": ""Student performance on college entrance exams"",
                    ""displayName"": ""College Entrance Exams"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 20,
                    ""metricId"": 1025,
                    ""name"": ""College Entrance Exams"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students who have taken college entrance exams"",
                        ""displayName"": ""College Entrance Exams"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 324,
                        ""name"": ""College Entrance Exams"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": ""% of students taken PSAT"",
                            ""displayName"": ""PSAT"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 325,
                            ""name"": ""PSAT"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": [
                              {
                                ""description"": """",
                                ""displayName"": ""9th Grade"",
                                ""domainEntityType"": ""School"",
                                ""displayOrder"": 10,
                                ""metricId"": 1269,
                                ""name"": ""9th Grade"",
                                ""url"": ""~/Application/School/Metrics.aspx""
                              },
                              {
                                ""description"": """",
                                ""displayName"": ""10th Grade"",
                                ""domainEntityType"": ""School"",
                                ""displayOrder"": 30,
                                ""metricId"": 1270,
                                ""name"": ""10th Grade"",
                                ""url"": ""~/Application/School/Metrics.aspx""
                              },
                              {
                                ""description"": """",
                                ""displayName"": ""11th Grade"",
                                ""domainEntityType"": ""School"",
                                ""displayOrder"": 40,
                                ""metricId"": 1271,
                                ""name"": ""11th Grade"",
                                ""url"": ""~/Application/School/Metrics.aspx""
                              }
                            ]
                          },
                          {
                            ""description"": ""% of students taken SAT/ACT"",
                            ""displayName"": ""SAT/ACT Taken"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 499,
                            ""name"": ""SAT/ACT Taken"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": [
                              {
                                ""description"": """",
                                ""displayName"": ""9th Grade"",
                                ""domainEntityType"": ""School"",
                                ""displayOrder"": 10,
                                ""metricId"": 500,
                                ""name"": ""9th Grade"",
                                ""url"": ""~/Application/School/Metrics.aspx""
                              },
                              {
                                ""description"": """",
                                ""displayName"": ""10th Grade"",
                                ""domainEntityType"": ""School"",
                                ""displayOrder"": 20,
                                ""metricId"": 501,
                                ""name"": ""10th Grade"",
                                ""url"": ""~/Application/School/Metrics.aspx""
                              },
                              {
                                ""description"": """",
                                ""displayName"": ""11th Grade"",
                                ""domainEntityType"": ""School"",
                                ""displayOrder"": 30,
                                ""metricId"": 502,
                                ""name"": ""11th Grade"",
                                ""url"": ""~/Application/School/Metrics.aspx""
                              },
                              {
                                ""description"": """",
                                ""displayName"": ""12th Grade"",
                                ""domainEntityType"": ""School"",
                                ""displayOrder"": 40,
                                ""metricId"": 503,
                                ""name"": ""12th Grade"",
                                ""url"": ""~/Application/School/Metrics.aspx""
                              }
                            ]
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""At or Above Benchmark"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 40,
                            ""metricId"": 1226,
                            ""name"": ""At or Above Benchmark"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": [
                              {
                                ""description"": ""% of students at/above benchmark"",
                                ""displayName"": ""ACT: % of students"",
                                ""domainEntityType"": ""School"",
                                ""displayOrder"": 10,
                                ""metricId"": 505,
                                ""name"": ""ACT: % of students"",
                                ""url"": ""~/Application/School/Metrics.aspx""
                              },
                              {
                                ""description"": ""% of students at/above benchmark"",
                                ""displayName"": ""SAT: % of students"",
                                ""domainEntityType"": ""School"",
                                ""displayOrder"": 20,
                                ""metricId"": 504,
                                ""name"": ""SAT: % of students"",
                                ""url"": ""~/Application/School/Metrics.aspx""
                              }
                            ]
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""High School Graduation Plan"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 110,
                    ""metricId"": 318,
                    ""name"": ""High School Graduation Plan"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students on each plan"",
                        ""displayName"": ""High School Graduation Plan"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 331,
                        ""metricId"": 1470,
                        ""name"": ""High School Graduation Plan"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Minimum"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 319,
                            ""name"": ""Minimum"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Recommended"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 320,
                            ""name"": ""Recommended"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Distinguished"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 321,
                            ""name"": ""Distinguished"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              }
            ]
          },
          {
            ""description"": """",
            ""displayName"": ""Operational Dashboard"",
            ""domainEntityType"": ""School"",
            ""displayOrder"": 30,
            ""metricId"": 1029,
            ""name"": ""Operational Dashboard"",
            ""url"": ""~/Application/School/Metrics.aspx"",
            ""children"": [
              {
                ""description"": """",
                ""displayName"": ""Staff"",
                ""domainEntityType"": ""School"",
                ""displayOrder"": 10,
                ""metricId"": 1030,
                ""name"": ""Staff"",
                ""url"": ""~/Application/School/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": """",
                    ""displayName"": ""Teacher Attendance"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 10,
                    ""metricId"": 1031,
                    ""name"": ""Teacher Attendance"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers meeting attendance rate threshold of 90%"",
                        ""displayName"": ""Teacher Attendance Rate"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 361,
                        ""name"": ""Teacher Attendance Rate"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1032,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1033,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 30,
                            ""metricId"": 1034,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 35,
                            ""metricId"": 1034,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Experience, Education, and Certifications"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 70,
                    ""metricId"": 1035,
                    ""name"": ""Experience, Education, and Certifications"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers with >5 years of professional teaching experience"",
                        ""displayName"": ""Teacher Experience"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 369,
                        ""name"": ""Teacher Experience"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""All Subjects"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 370,
                            ""metricId"": 370,
                            ""name"": ""All Subjects"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""ELA/Reading"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 371,
                            ""metricId"": 371,
                            ""name"": ""ELA/Reading"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 372,
                            ""metricId"": 372,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 424,
                            ""metricId"": 424,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 425,
                            ""metricId"": 425,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of teachers with advanced degrees"",
                        ""displayName"": ""Teacher Education"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 20,
                        ""metricId"": 1069,
                        ""name"": ""Teacher Education"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Bachelors or Higher"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 10,
                            ""metricId"": 1070,
                            ""name"": ""Bachelors or Higher"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Masters or Doctorate"",
                            ""domainEntityType"": ""School"",
                            ""displayOrder"": 20,
                            ""metricId"": 1071,
                            ""name"": ""Masters or Doctorate"",
                            ""url"": ""~/Application/School/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Retention and Staffing"",
                    ""domainEntityType"": ""School"",
                    ""displayOrder"": 80,
                    ""metricId"": 1037,
                    ""name"": ""Retention and Staffing"",
                    ""url"": ""~/Application/School/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers returning from the previous year"",
                        ""displayName"": ""Teacher Retention"",
                        ""domainEntityType"": ""School"",
                        ""displayOrder"": 10,
                        ""metricId"": 375,
                        ""name"": ""Teacher Retention"",
                        ""url"": ""~/Application/School/Metrics.aspx"",
                        ""children"": []
                      }
                    ]
                  }
                ]
              }
            ]
          }
        ]
      },
      {
        ""description"": """",
        ""displayName"": ""Local Education Agency"",
        ""domainEntityType"": """",
        ""displayOrder"": 40,
        ""metricId"": 1284,
        ""name"": ""Local Education Agency"",
        ""url"": """",
        ""children"": [
          {
            ""description"": """",
            ""displayName"": ""Overview"",
            ""domainEntityType"": ""Local Education Agency"",
            ""displayOrder"": 10,
            ""metricId"": 1288,
            ""name"": ""Overview"",
            ""url"": ""~/Application/LocalEducationAgency/Overview.aspx"",
            ""children"": [
              {
                ""description"": ""Students' attendance and discipline patterns"",
                ""displayName"": ""Attendance and Discipline"",
                ""domainEntityType"": ""Local Education Agency"",
                ""displayOrder"": 10,
                ""metricId"": 1289,
                ""name"": ""Attendance and Discipline"",
                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Daily and class period attendance"",
                    ""displayName"": ""Attendance"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 10,
                    ""metricId"": 1344,
                    ""name"": ""Attendance"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""Average daily attendance for all students with membership in the district (unweighted)"",
                        ""displayName"": ""Average Daily Attendance"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 1345,
                        ""name"": ""Average Daily Attendance"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 0,
                            ""metricId"": 1346,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 1,
                            ""metricId"": 1311,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 2,
                            ""metricId"": 1347,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 3,
                            ""metricId"": 1347,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students meeting attendance rate threshold of 90% during the specified time frame"",
                        ""displayName"": ""Daily Attendance Rate"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 20,
                        ""metricId"": 1348,
                        ""name"": ""Daily Attendance Rate"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 0,
                            ""metricId"": 1349,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 1,
                            ""metricId"": 1341,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 2,
                            ""metricId"": 1350,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 3,
                            ""metricId"": 1350,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students exceeding class period absence rate threshold of 10% during the specified timeframe"",
                        ""displayName"": ""Class Period Absence Rate"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 30,
                        ""metricId"": 1351,
                        ""name"": ""Class Period Absence Rate"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 0,
                            ""metricId"": 1352,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 1,
                            ""metricId"": 1342,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 2,
                            ""metricId"": 1353,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 3,
                            ""metricId"": 1353,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% students exceeding tardy rate threshold of 10% during specified time frame"",
                        ""displayName"": ""Tardy Rate"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 40,
                        ""metricId"": 1354,
                        ""name"": ""Tardy Rate"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 0,
                            ""metricId"": 1355,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 1,
                            ""metricId"": 1343,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 2,
                            ""metricId"": 1356,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 3,
                            ""metricId"": 1356,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 8 or more absences year to date"",
                        ""displayName"": ""Days Absent"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 80,
                        ""metricId"": 1674,
                        ""name"": ""Days Absent"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": ""% of students with 8 or more absences year to date"",
                            ""displayName"": ""Days Absent"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1561,
                            ""name"": ""Days Absent"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more absences year to date"",
                            ""displayName"": ""Prior Year Days Absent"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1561,
                            ""name"": ""Prior Year Days Absent"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more unexcused absences year to date"",
                            ""displayName"": ""Unexcused Days Absent"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 1675,
                            ""name"": ""Unexcused Days Absent"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": ""% of students with 8 or more unexcused absences year to date"",
                            ""displayName"": ""Prior Year Unexcused Days Absent"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 35,
                            ""metricId"": 1675,
                            ""name"": ""Prior Year Unexcused Days Absent"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Discipline incidents and actions"",
                    ""displayName"": ""Discipline"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 20,
                    ""metricId"": 1357,
                    ""name"": ""Discipline"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with 1+ state reportable offenses in a given grading period"",
                        ""displayName"": ""State Offenses"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 2358,
                        ""name"": ""State Offenses"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 2359,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 2360,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 2361,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year State Offenses"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 2361,
                            ""name"": ""Prior Year State Offenses"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1+ non state reportable offenses in a given grading period"",
                        ""displayName"": ""Non-State Offenses"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 20,
                        ""metricId"": 2362,
                        ""name"": ""Non-State Offenses"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 2363,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 2364,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 2365,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Non-State Offenses"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 2365,
                            ""name"": ""Prior Year Non-State Offenses"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1+ state reportable offenses and/or excessive (5+) school code of conduct incidents in a given grading period"",
                        ""displayName"": ""All Discipline Incidents"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 30,
                        ""metricId"": 1358,
                        ""name"": ""All Discipline Incidents"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Current Grading Period"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1428,
                            ""name"": ""Current Grading Period"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1429,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 25,
                            ""metricId"": 1429,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students with 1 or more incidents in a given grading period"",
                        ""displayName"": ""School Code of Conduct Incidents"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 40,
                        ""metricId"": 1359,
                        ""name"": ""School Code of Conduct Incidents"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Current Grading Period"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1430,
                            ""name"": ""Current Grading Period"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1431,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 25,
                            ""metricId"": 1431,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""State examinations and assessments"",
                ""displayName"": ""State Assessments"",
                ""domainEntityType"": ""Local Education Agency"",
                ""displayOrder"": 20,
                ""metricId"": 1290,
                ""name"": ""State Assessments"",
                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress on state standardized tests"",
                    ""displayName"": ""State Standardized Assessments"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 10,
                    ""metricId"": 1317,
                    ""name"": ""State Standardized Assessments"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students meeting standard"",
                        ""displayName"": ""State Assessment"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 1320,
                        ""name"": ""State Assessment"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1321,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1322,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 1323,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 1324,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 50,
                            ""metricId"": 1325,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of eligible students who did not take the test"",
                        ""displayName"": ""State Assessment Non-Participation"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 20,
                        ""metricId"": 1326,
                        ""name"": ""State Assessment Non-Participation"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1402,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1403,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 1404,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 1405,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 50,
                            ""metricId"": 1406,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students failing state assessment 2 consecutive administrations"",
                        ""displayName"": ""Repeat State Assessment Failures"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 30,
                        ""metricId"": 1327,
                        ""name"": ""Repeat State Assessment Failures"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": []
                      }
                    ]
                  },
                  {
                    ""description"": ""Performance and progress on language assessments"",
                    ""displayName"": ""Language Assessments"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 20,
                    ""metricId"": 1318,
                    ""name"": ""Language Assessments"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students that met performance level for their grade level"",
                        ""displayName"": ""Language Assessments"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 0,
                        ""metricId"": 1308,
                        ""name"": ""Language Assessments"",
                        ""url"": """",
                        ""children"": [
                          {
                            ""description"": ""% of students that met performance level for their grade level"",
                            ""displayName"": ""Language Assessment Proficiency"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 0,
                            ""metricId"": 1333,
                            ""name"": ""Language Assessment Proficiency"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Local examinations and assessments"",
                ""displayName"": ""Local Assessments"",
                ""domainEntityType"": ""Local Education Agency"",
                ""displayOrder"": 30,
                ""metricId"": 1488,
                ""name"": ""Local Assessments"",
                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress on local benchmark assessments"",
                    ""displayName"": ""Benchmark Assessments"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 10,
                    ""metricId"": 1319,
                    ""name"": ""Benchmark Assessments"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students at or above the district threshold (as of the latest administration)"",
                        ""displayName"": ""Benchmark Performance"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 1334,
                        ""name"": ""Benchmark Performance"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1335,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 1337,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 1338,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 50,
                            ""metricId"": 1339,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Learning Standard Assessments"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 20,
                    ""metricId"": 100002,
                    ""name"": ""Learning Standard Assessments"",
                    ""url"": """",
                    ""children"": [
                      {
                        ""description"": ""% of students at or above the district threshold (as of the latest administration)"",
                        ""displayName"": ""Learning Standard Mastery by Core Subject Area"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 20,
                        ""metricId"": 2167,
                        ""name"": ""Learning Standard Mastery by Core Subject Area"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 2168,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 2169,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 2170,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 2171,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Performance on early reading inventories and assessments"",
                    ""displayName"": ""Reading Assessments"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 30,
                    ""metricId"": 1340,
                    ""name"": ""Reading Assessments"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": """",
                        ""displayName"": ""Reading Assessments"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 0,
                        ""metricId"": 1261,
                        ""name"": ""Reading Assessments"",
                        ""url"": """",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Reading Assessment"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 0,
                            ""metricId"": 1480,
                            ""name"": ""Reading Assessment"",
                            ""url"": """",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Students' progression in coursework"",
                ""displayName"": ""Grades and Credits"",
                ""domainEntityType"": ""Local Education Agency"",
                ""displayOrder"": 40,
                ""metricId"": 1291,
                ""name"": ""Grades and Credits"",
                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Performance and progress in coursework"",
                    ""displayName"": ""Course Grades: Secondary"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 10,
                    ""metricId"": 1447,
                    ""name"": ""Course Grades: Secondary"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with 2 or more Grades Below Proficiency in the core subject areas"",
                        ""displayName"": ""Grades Below Proficiency"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 1303,
                        ""name"": ""Grades Below Proficiency"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of students with 2 or more Grades Below Proficiency in the core subject areas"",
                        ""displayName"": ""Prior Year Grades Below Proficiency Level"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 20,
                        ""metricId"": 1303,
                        ""name"": ""Prior Year Grades Below Proficiency Level"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of students with core subject course grades dropping below proficiency from prior grading period"",
                        ""displayName"": ""Dropping Class Grades"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 40,
                        ""metricId"": 1304,
                        ""name"": ""Dropping Class Grades"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1411,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1412,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 1413,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 1414,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students who are taking/have taken; % of students who are passing/have passed Algebra I courses"",
                        ""displayName"": ""Algebra I"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 50,
                        ""metricId"": 1307,
                        ""name"": ""Algebra I"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Taking or have Taken"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1312,
                            ""name"": ""Taking or have Taken"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Passing or have Passed by 9th grade"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1313,
                            ""name"": ""Passing or have Passed by 9th grade"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students who are taking/have taken; % of students who are passing/have passed Algebra I courses"",
                        ""displayName"": ""Algebra I - 8th Grade"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 60,
                        ""metricId"": 1676,
                        ""name"": ""Algebra I - 8th Grade"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Taking or have Taken"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1309,
                            ""name"": ""Taking or have Taken"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Passing or have Passed by 8th grade"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1310,
                            ""name"": ""Passing or have Passed by 8th grade"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Performance and progress in subject areas"",
                    ""displayName"": ""Course Grades: Primary"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 20,
                    ""metricId"": 1301,
                    ""name"": ""Course Grades: Primary"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with a failing grade in one or more grading periods"",
                        ""displayName"": ""Failing Subject Area Grades"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 20,
                        ""metricId"": 1305,
                        ""name"": ""Failing Subject Area Grades"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1415,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year ELA / Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 15,
                            ""metricId"": 1415,
                            ""name"": ""Prior Year ELA / Reading"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Writing"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1416,
                            ""name"": ""Writing"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Writing"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 25,
                            ""metricId"": 1416,
                            ""name"": ""Prior Year Writing"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 1417,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 35,
                            ""metricId"": 1417,
                            ""name"": ""Prior Year Mathematics"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 1418,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 45,
                            ""metricId"": 1418,
                            ""name"": ""Prior Year Science"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 50,
                            ""metricId"": 1419,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 55,
                            ""metricId"": 1419,
                            ""name"": ""Prior Year Social Studies"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Student progress toward graduation"",
                    ""displayName"": ""Credits"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 30,
                    ""metricId"": 1314,
                    ""name"": ""Credits"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": """",
                        ""displayName"": ""District Credit Accumulation"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 2376,
                        ""name"": ""District Credit Accumulation"",
                        ""url"": """",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of students meeting required # of credits for Recommended Graduation Plan as of the end of the prior school year"",
                        ""displayName"": ""Credit Accumulation"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 20,
                        ""metricId"": 1315,
                        ""name"": ""Credit Accumulation"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of students who have earned the expected credits in each of their graduation plan subject areas at the end of the prior school year"",
                        ""displayName"": ""On Track to Graduate"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 30,
                        ""metricId"": 1316,
                        ""name"": ""On Track to Graduate"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": []
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Advanced coursework opportunity and performance"",
                ""displayName"": ""Advanced Academics"",
                ""domainEntityType"": ""Local Education Agency"",
                ""displayOrder"": 50,
                ""metricId"": 1292,
                ""name"": ""Advanced Academics"",
                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Student performance on state standardized tests"",
                    ""displayName"": ""State Standardized Assessments"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 10,
                    ""metricId"": 1360,
                    ""name"": ""State Standardized Assessments"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students with advanced performance"",
                        ""displayName"": ""State Assessment Advanced Performance"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 1361,
                        ""name"": ""State Assessment Advanced Performance"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1362,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 1364,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 1365,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 50,
                            ""metricId"": 1366,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": ""Student opportunity and performance in advanced coursework"",
                    ""displayName"": ""Advanced Academics"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 20,
                    ""metricId"": 1367,
                    ""name"": ""Advanced Academics"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of middle and high school students eligible for advanced courses, but not currently enrolled"",
                        ""displayName"": ""Advanced Course Potential"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 1368,
                        ""name"": ""Advanced Course Potential"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1369,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1370,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 1371,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 1372,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of middle and high school students eligible for advanced courses and currently enrolled"",
                        ""displayName"": ""Advanced Course Potential"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 2388,
                        ""name"": ""Advanced Course Potential"",
                        ""url"": """",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA/Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 2389,
                            ""name"": ""ELA/Reading"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 2390,
                            ""name"": ""Mathematics"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 2391,
                            ""name"": ""Science"",
                            ""url"": """",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 2392,
                            ""name"": ""Social Studies"",
                            ""url"": """",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of middle and high school students currently enrolled in at least 1 pre-AP, AP, IB, or Dual Credit core course"",
                        ""displayName"": ""Advanced Course Enrollment"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 20,
                        ""metricId"": 1373,
                        ""name"": ""Advanced Course Enrollment"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Middle School"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1454,
                            ""name"": ""Middle School"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""High School"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1455,
                            ""name"": ""High School"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of students completing at least 1 pre-AP, AP, IB, or Dual Credit course in one of the four core subject areas in prior school years"",
                        ""displayName"": ""Advanced Course Completion"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 30,
                        ""metricId"": 1374,
                        ""name"": ""Advanced Course Completion"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Middle School"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1456,
                            ""name"": ""Middle School"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""High School"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1457,
                            ""name"": ""High School"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  }
                ]
              },
              {
                ""description"": ""Students' ability to succeed in higher education  and the workforce"",
                ""displayName"": ""College and Career Readiness"",
                ""domainEntityType"": ""Local Education Agency"",
                ""displayOrder"": 60,
                ""metricId"": 1293,
                ""name"": ""College and Career Readiness"",
                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": ""Graduation, completion and dropout rates"",
                    ""displayName"": ""Graduation Status"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 10,
                    ""metricId"": 1376,
                    ""name"": ""Graduation Status"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of students from the 9th grade cohort, who have completed high school within four years"",
                        ""displayName"": ""High School Completion Rate"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 20,
                        ""metricId"": 1378,
                        ""name"": ""High School Completion Rate"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of 9th grade freshmen graduating within 4 years (4-year adjusted cohort)"",
                        ""displayName"": ""High School Graduation Rate"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 30,
                        ""metricId"": 1379,
                        ""name"": ""High School Graduation Rate"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of students dropping out who were enrolled in a given school year in grades 9-12, but did not return to a state public school"",
                        ""displayName"": ""High School Dropout Rate"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 40,
                        ""metricId"": 1380,
                        ""name"": ""High School Dropout Rate"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": []
                      }
                    ]
                  },
                  {
                    ""description"": ""Student performance on college entrance exams"",
                    ""displayName"": ""College Entrance Exams"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 20,
                    ""metricId"": 1381,
                    ""name"": ""College Entrance Exams"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of high school students who have taken ACT or SAT"",
                        ""displayName"": ""College Entrance Exams"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 1382,
                        ""name"": ""College Entrance Exams"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""SAT/ACT Taken"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1383,
                            ""name"": ""SAT/ACT Taken"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": [
                              {
                                ""description"": """",
                                ""displayName"": ""9th Grade"",
                                ""domainEntityType"": ""Local Education Agency"",
                                ""displayOrder"": 10,
                                ""metricId"": 1427,
                                ""name"": ""9th Grade"",
                                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx""
                              },
                              {
                                ""description"": """",
                                ""displayName"": ""10th Grade"",
                                ""domainEntityType"": ""Local Education Agency"",
                                ""displayOrder"": 20,
                                ""metricId"": 1434,
                                ""name"": ""10th Grade"",
                                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx""
                              },
                              {
                                ""description"": """",
                                ""displayName"": ""11th Grade"",
                                ""domainEntityType"": ""Local Education Agency"",
                                ""displayOrder"": 30,
                                ""metricId"": 1435,
                                ""name"": ""11th Grade"",
                                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx""
                              },
                              {
                                ""description"": """",
                                ""displayName"": ""12th Grade"",
                                ""domainEntityType"": ""Local Education Agency"",
                                ""displayOrder"": 40,
                                ""metricId"": 1436,
                                ""name"": ""12th Grade"",
                                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx""
                              }
                            ]
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""At or Above Benchmark"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1384,
                            ""name"": ""At or Above Benchmark"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": [
                              {
                                ""description"": """",
                                ""displayName"": ""ACT: % of students"",
                                ""domainEntityType"": ""Local Education Agency"",
                                ""displayOrder"": 10,
                                ""metricId"": 1432,
                                ""name"": ""ACT: % of students"",
                                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx""
                              },
                              {
                                ""description"": """",
                                ""displayName"": ""SAT: % of students"",
                                ""domainEntityType"": ""Local Education Agency"",
                                ""displayOrder"": 20,
                                ""metricId"": 1433,
                                ""name"": ""SAT: % of students"",
                                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx""
                              }
                            ]
                          }
                        ]
                      }
                    ]
                  }
                ]
              }
            ]
          },
          {
            ""description"": """",
            ""displayName"": ""Operational Dashboard"",
            ""domainEntityType"": ""Local Education Agency"",
            ""displayOrder"": 20,
            ""metricId"": 1294,
            ""name"": ""Operational Dashboard"",
            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
            ""children"": [
              {
                ""description"": """",
                ""displayName"": ""Staff"",
                ""domainEntityType"": ""Local Education Agency"",
                ""displayOrder"": 10,
                ""metricId"": 1295,
                ""name"": ""Staff"",
                ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                ""children"": [
                  {
                    ""description"": """",
                    ""displayName"": ""Teacher Attendance"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 10,
                    ""metricId"": 1296,
                    ""name"": ""Teacher Attendance"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers meeting attendance rate threshold"",
                        ""displayName"": ""Teacher Attendance Rate"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 1297,
                        ""name"": ""Teacher Attendance Rate"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Last 20 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1298,
                            ""name"": ""Last 20 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Last 40 Days"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 2378,
                            ""name"": ""Last 40 Days"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Year to Date"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1299,
                            ""name"": ""Year to Date"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Prior Year"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 25,
                            ""metricId"": 1299,
                            ""name"": ""Prior Year"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Experience, Education, and Certifications"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 20,
                    ""metricId"": 1300,
                    ""name"": ""Experience, Education, and Certifications"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers with > 5 years of professional experience"",
                        ""displayName"": ""Teacher Experience"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 1389,
                        ""name"": ""Teacher Experience"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""All Subjects"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1462,
                            ""name"": ""All Subjects"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""ELA/Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1463,
                            ""name"": ""ELA/Reading"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 1464,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 1465,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 50,
                            ""metricId"": 1466,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of teachers with advanced degrees"",
                        ""displayName"": ""Teacher Education"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 20,
                        ""metricId"": 1391,
                        ""name"": ""Teacher Education"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""Bachelors or Higher"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 10,
                            ""metricId"": 1392,
                            ""name"": ""Bachelors or Higher"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Masters or Doctorate"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1393,
                            ""name"": ""Masters or Doctorate"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      },
                      {
                        ""description"": ""% of teachers qualified for teaching in subject they are teaching"",
                        ""displayName"": ""Teacher Certification by Subject Area"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 30,
                        ""metricId"": 1394,
                        ""name"": ""Teacher Certification by Subject Area"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": [
                          {
                            ""description"": """",
                            ""displayName"": ""ELA / Reading"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 20,
                            ""metricId"": 1396,
                            ""name"": ""ELA / Reading"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Mathematics"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 30,
                            ""metricId"": 1397,
                            ""name"": ""Mathematics"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Science"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 40,
                            ""metricId"": 1398,
                            ""name"": ""Science"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Social Studies"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 50,
                            ""metricId"": 1399,
                            ""name"": ""Social Studies"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          },
                          {
                            ""description"": """",
                            ""displayName"": ""Generalist"",
                            ""domainEntityType"": ""Local Education Agency"",
                            ""displayOrder"": 60,
                            ""metricId"": 1467,
                            ""name"": ""Generalist"",
                            ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                            ""children"": []
                          }
                        ]
                      }
                    ]
                  },
                  {
                    ""description"": """",
                    ""displayName"": ""Retention and Staffing"",
                    ""domainEntityType"": ""Local Education Agency"",
                    ""displayOrder"": 30,
                    ""metricId"": 1400,
                    ""name"": ""Retention and Staffing"",
                    ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                    ""children"": [
                      {
                        ""description"": ""% of teachers returning from the previous year"",
                        ""displayName"": ""Teacher Retention"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 10,
                        ""metricId"": 1401,
                        ""name"": ""Teacher Retention"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": []
                      },
                      {
                        ""description"": ""% of principals returning from the previous year"",
                        ""displayName"": ""Principal Retention"",
                        ""domainEntityType"": ""Local Education Agency"",
                        ""displayOrder"": 20,
                        ""metricId"": 1468,
                        ""name"": ""Principal Retention"",
                        ""url"": ""~/Application/LocalEducationAgency/Metrics.aspx"",
                        ""children"": []
                      }
                    ]
                  }
                ]
              }
            ]
          }
        ]
      }
    ]
  }
}";
            Assert.True(JToken.DeepEquals(JObject.Parse(expectedValue), JObject.Parse(json)));
        }
    }
}
