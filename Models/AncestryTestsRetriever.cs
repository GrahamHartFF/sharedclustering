﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace AncestryDnaClustering.Models
{
    internal class AncestryTestsRetriever
    {
        private HttpClient _ancestryClient;

        public AncestryTestsRetriever(HttpClient ancestryClient)
        {
            _ancestryClient = ancestryClient;
        }

        // Download the list of tests available to this user account.
        // As in the web site the tests are sorted with the user's own test, followed by the other tests alphabetically.
        public async Task<Dictionary<string, string>> GetTestsAsync()
        {
            using (var testsResponse = await _ancestryClient.GetAsync("dna/secure/tests"))
            {
                if (testsResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Invalid username or password" +
                        $"{Environment.NewLine}{Environment.NewLine}" +
                        "Your username or password is incorrect. Please try again.", "Invalid username or password", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                testsResponse.EnsureSuccessStatusCode();
                var tests = await testsResponse.Content.ReadAsAsync<Tests>();
                return tests.Data.CompleteTests
                    .OrderByDescending(test => test.UsersSelfTest)
                    .ThenBy(test => test.TestSubject.Surname)
                    .ThenBy(test => test.TestSubject.GivenNames)
                    .ToDictionary(test => test.TestSubject.FullName, test => test.Guid);
            }
        }

        private class Tests
        {
            public string Status { get; set; }
            public string Code { get; set; }
            public TestsData Data { get; set; }
        }

        private class TestsData
        {
            public List<TestResult> CompleteTests { get; set; }
        }

        private class TestResult
        {
            public string Guid { get; set; }
            public TestSubject TestSubject { get; set; }
            public bool UsersSelfTest { get; set; }
        }

        private class TestSubject
        {
            public string DisplayName { get; set; }
            public string GivenNames { get; set; }
            public string Surname { get; set; }
            public string UcdmId { get; set; }

            public string FullName => $"{GivenNames} {Surname}";
        }
    }
}
