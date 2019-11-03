using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace BestFitTrip.Models
{
    public class DestinationValue
    {
        public int ID { get; set; }
        public string Destination { get; set; }
        public double Distance { get; set; }
        public string Duration { get; set; }
        public int Order { get; set; }

        public int TripID { get; set; }
        public Trip Trip { get; set; }


        public static DistanceMatrix GetMatrix(string origin, List<string> destinations, string mode = "driving")
        {
            using (WebClient wc = new WebClient())
            {
                string url = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=" +
                    origin + "&destinations=";
                foreach (string destination in destinations)
                {
                    url += destination + "|";
                }
                url += "&key=AIzaSyClGGZWgr5ZIAJMe3yMmaW2dN1oAPqle88&mode=" + mode;

                var json = wc.DownloadString(url);
                DistanceMatrix matrix = JsonConvert.DeserializeObject<DistanceMatrix>(json);

                return matrix;

            }
        }

        public static Dictionary<string, DestinationValue> GetDistances(string origin, List<string> destinations, string mode = "driving")
        {
            DistanceMatrix matrix = new DistanceMatrix();
            matrix = GetMatrix(origin, destinations, mode);

            var distances = new Dictionary<string, DestinationValue>();


            int i = 0;
            foreach (var element in matrix.Rows[0].Elements)
            {
                if (element.Status == "OK")
                {
                    distances.Add(matrix.Destination_Addresses[i], new DestinationValue()
                    {
                        Distance = (double)(long)element.Distance["value"],
                        Duration = element.Duration["text"].ToString()
                    });

                }
                else
                {
                    distances.Add(matrix.Destination_Addresses[i], new DestinationValue()
                    {
                        Distance = 9999999999999,
                        Duration = "No Results"
                    });
                }

                i++;
            }
            return distances;
        }

        public static ICollection<DestinationValue> GetDistancesOrdered(string origin, List<string> destinations,
            string mode = "driving", string orderBy = "distance")
        {
            DistanceMatrix matrix = new DistanceMatrix();
            matrix = GetMatrix(origin, destinations, mode);
            var DestinationsFixed = matrix.Destination_Addresses;


            var distancesOrdered = new List<DestinationValue>() { new DestinationValue() {
                Destination = origin,
                Distance = 0,
                Duration = "Start",
                Order = 0 } };
            int count = destinations.Count;

            ///may have to write code to add destinationvalues to database

            for (int i = 1; i <= count; i++)
            {
                var distances = GetDistances(origin, DestinationsFixed, mode);
                var shortest = new KeyValuePair<string, DestinationValue>();
                if (orderBy == "distance")
                {
                    shortest = distances.OrderBy(kvp => kvp.Value.Distance).First();
                }
                else if (orderBy == "duration")
                {
                    shortest = distances.OrderBy(kvp => kvp.Value.Duration).First();
                }

                double distanceTemp = shortest.Value.Distance;
                if (distanceTemp == 9999999999999)
                {
                    distanceTemp = 0;
                }
                else
                {
                    distanceTemp = Math.Round(distanceTemp / 1609.344);
                }


                distancesOrdered.Add(new DestinationValue()
                {
                    Destination = shortest.Key,
                    Distance = distanceTemp,
                    Duration = shortest.Value.Duration,
                    Order = i
                });

                //next round
                origin = shortest.Key;
                DestinationsFixed.Remove(shortest.Key);



            }

            return distancesOrdered;
        }
    }
}
