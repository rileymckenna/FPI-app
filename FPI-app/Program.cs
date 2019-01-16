using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SODA;
using GeoData;

namespace FPI_app
{
    class Program
    {
        private static SodaClient sodaClient = new SodaClient(Constants.giswebsite, Constants.apiKey);
        private static List<Parcel> parcels = new List<Parcel>();
        private static List<Foreclosure> foreclosures = new List<Foreclosure>();
        static void Main(string[] args)
        {
            parcels = ParcelsAjax();
            foreclosures = ForeclosureAjax(); 
        }

        private static List<Parcel> GetParcels(string str)
        {
            return  JsonConvert.DeserializeObject<List<Parcel>>(str);

        }

        private static List<Foreclosure> GetForclosures(string str)
        {
            return Foreclosure.FromJson(str);
        }

        private static List<Foreclosure> ForeclosureAjax()
        {
            String str = String.Empty;
            string url = Constants.giswebsite;
            List<Foreclosure> listForeclosures = new List<Foreclosure>();
            for (int i = 0; i < 2; i++)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create($"{url}resource/{Constants.forclosureKey}?$limit=50000&$offset={i}");
                request.Method = "GET";
                request.ContentType = "application/json; charset=UTF-8";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader streamReader = new StreamReader(dataStream);
                    using (JsonTextReader reader = new JsonTextReader(streamReader))
                    {
                        reader.SupportMultipleContent = true;
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                listForeclosures.Add(JsonConvert.DeserializeObject<Foreclosure>(reader.ReadAsString()));
                            }
                        }
                        reader.Close();
                        dataStream.Close();
                    }
                }
            }
            return listForeclosures;
        }

        private static List<Parcel> ParcelsAjax()
        {
            String str = String.Empty;
            string url = Constants.giswebsite;
            List<Parcel> listParcels = new List<Parcel>();
            for (int i = 0; i < 10; i++)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create($"{url}resource/{Constants.parcelKey}?$limit=50000&$offset={i}");
                request.Method = "GET";
                request.ContentType = "application/json; charset=UTF-8";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader streamReader = new StreamReader(dataStream);
                    using (JsonTextReader reader = new JsonTextReader(streamReader))
                    {
                        reader.SupportMultipleContent = true;
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonToken.StartObject)
                            {
                                listParcels.Add(Parcel.FromJson(reader.ToString()));
                            }
                        }
                        reader.Close();
                        dataStream.Close();
                    }
                }
            }
            return listParcels;
        }

    }
}
