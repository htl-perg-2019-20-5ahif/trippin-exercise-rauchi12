using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Trippin_Exercise
{
    public class Program
    {
        private static readonly HttpClient client = new HttpClient() { BaseAddress = new Uri("https://services.odata.org/TripPinRESTierService/(S(gztdtzlaudqtretea5i214ha))/") };

        static async Task Main(string[] args)
        {
            
            foreach (Person person in GetPersonFromFile())
            {
                if (!await CheckExist(person))
                {
                    AddPerson(person);
                }
            }
        }

        private static IEnumerable<Person> GetPersonFromFile()
        {
            return JsonSerializer.Deserialize<Person[]>(File.ReadAllText("users.json"));
        }

        public static async Task<bool> CheckExist(Person person)
        {
            Console.WriteLine("Check "+person.UserName);
            return (await client.GetAsync($"People('{person.UserName}')")).IsSuccessStatusCode;
        }

        public static async void AddPerson(Person person)
        {
            var newPerson = new
            {
                UserName = person.UserName,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Emails = new[] {
                    person.Email
                },

                AddressInfo = new[] {
                    new {
                        Address = person.Address,
                        City = new  {
                            Name = person.CityName,
                            CountryRegion = person.Country,
                            Region = "unknown"
                        }
                    }
                }
            };
            Console.WriteLine("Added "+person.UserName);
        }
    }
}


