using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace RestSharpTest
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:5000");
        }
        private IRestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);

            //act // Department d = new Employee(); type cast possible if department is parent class of employee class
            // Both response and Execute(request) has same interface implementation
            IRestResponse response = client.Execute(request);
            return response;
        }
        [TestMethod]
        public void onCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            //assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(10, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.id + "Name: " + item.name + "Salary: " + item.Salary);
            }
        }


        [TestMethod]
        public void givenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "Clark");
            jObjectbody.Add("Salary", "15000");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual(15000, dataResponse.Salary);

        }
        /// <summary>
        /// UC3
        /// Tests the add multiple entries. POST
        /// </summary>
        [TestMethod]
        public void TestAddMultipleEntriesUsingPostOperation()
        {
            ////Add multiple entries
            ////Created a list
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee { name = "Girish", Salary = 40000 });
            employeeList.Add(new Employee { name = "Harsh", Salary = 50000 });

            foreach (Employee employee in employeeList)
            {
                ////Used post method to add Data.
                ////"/employees" will append to the url
                RestRequest request = new RestRequest("/employees", Method.POST);
                JObject jObject = new JObject();
                jObject.Add("name", employee.name);
                jObject.Add("salary", employee.Salary);
                request.AddParameter("application/json", jObject, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                //derserializing object for assert and checking test case
                Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(employee.name, dataResponse.name);
            }
        }
        /// <summary>
        /// UC4
        ////Update salary using PUT operation
        /// </summary>
        [TestMethod]
        public void TestUpdateDataUsingPutOperation()
        {
            ////Request to update data of an employee
            RestRequest request = new RestRequest("employees/3", Method.PUT);
            ////Creating object to update data
            JObject jobject = new JObject();
            ////Updating name
            jobject.Add("name", "Tejaswi");
            jobject.Add("salary", "300000");
            //adding parameters in request
            //request body parameter type signifies values added using add.
            request.AddParameter("application/json", jobject, ParameterType.RequestBody);
            //executing request using client
            //IRest response act as a container for the data sent back from api.
            IRestResponse response = client.Execute(request);
            //checking status code of response
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            //deserializing content added in json file
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            //asserting for salary
            Assert.AreEqual(dataResponse.Salary, "300000");
        }


    }
}
