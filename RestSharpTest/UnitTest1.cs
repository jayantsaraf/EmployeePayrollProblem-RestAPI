using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

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
        



    }
}
